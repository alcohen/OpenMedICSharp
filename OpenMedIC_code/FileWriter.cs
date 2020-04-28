/* --- GPL ---
 *
 * Copyright (C) 2004-2006 Duke-River Engineering Company.
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
 *
 * --- GPL --- */

using System;
using System.IO;
using System.Text;

namespace OpenMedIC
{
	/// <summary>
	/// Creates a file with the desired path/filename and an appropriate header
	/// (or appends to an existing one if appropriate), and writes to it the passed
	/// input data, one row per sample.
	/// 
	/// The class offers options for:
	/// 
	/// -&gt; Writing the values as ASCII text (more universally readable)
	///				 or binary values (about 3x more compact);
	///	-&gt; Limiting the maximum file size (and what to do then);
	///	-&gt; Appending creation date and/or time to the name;
	///	-&gt; Starting a new file on a new day;
	///	-&gt; Replacing or appending to an existing file;
	///	-&gt; [FUTURE ENH - create dir if not existing];
	///	-&gt; [FUTURE ENH - put date in dir name not file name].
	/// 
	/// Default values:
	/// 
	/// -&gt; Writing the values as ASCII text:  default = TRUE;
	///	-&gt; Limiting the maximum file size:  default = FALSE;
	///	-&gt; Appending creation date and/or time:  default = FALSE;
	///	-&gt; Starting a new file on a new day:  default = FALSE;
	///	-&gt; Replacing or appending to an existing file:  default = FALSE;
	///	-&gt; [FUTURE ENH - create dir if not existing:  default = FALSE];
	///	-&gt; [FUTURE ENH - put date in dir name not file name:  default = FALSE].
	/// 
	/// Note the following required data to be passed to the init(ChainInfo) method:
	/// 
	/// -&gt; All cases:
	///		-&gt; -&gt; Data Sampling period, in seconds.
	///	
	///	-&gt; When writing the date to a file:
	///		-&gt; -&gt; DataInfo.dataDescription = Text Description of File Contents;
	///		-&gt; -&gt; DataInfo.startDateTime = Start Date/Time (can be left blank to 
	///					initialize to when this class instance is created);
	///		-&gt; -&gt; DataInfo.bitsPerSample = data sample accuracy.  For raw analog
	///					data, this is the number of bits used to generate the number;
	///					for calibrated-value digital inputs, this is the precision to
	///					which the values have been acquired, calibrated, and manipulated.
	///	
	/// </summary>
	public class FileWriter : IReceiver
	{
		/// <summary>
		/// Local storage of file data
		/// </summary>
		protected string	fPath;
		/// <summary>
		/// Local storage of file data
		/// </summary>
		protected string	fName;
		/// <summary>
		/// Local storage of file data
		/// </summary>
		protected string	fExt;
		/// <summary>
		/// Local storage of file data
		/// </summary>
		protected string	fCurFullName;	// full name of current file being written to

		/// <summary>
		/// How is data stored?  Ascii text (readable but inefficient) 
		/// vs. binary (efficient but unreadable)
		/// </summary>
		protected bool		writeAsAscii = true;

		/// <summary>
		/// How to handle if the file already exists:
		/// 
		///	-&gt; overwrite              =&gt; delete old file and create new;
		///	-&gt; !overwrite AND  append =&gt; append data to the end of the old file;
		///	-&gt; !overwrite and !append =&gt; throws an exception.
		/// </summary>
		protected bool		overwriteExisting = false;
		/// <summary>
		/// How to handle if the file already exists:
		/// 
		///	-&gt; overwrite              =&gt; delete old file and create new;
		///	-&gt; !overwrite AND  append =&gt; append data to the end of the old file;
		///	-&gt; !overwrite AND !append =&gt; throws an exception.
		///	</summary>
		protected bool		appendExisting = false;	// ignored if overwrite = true

		/// <summary>
		/// if TRUE, then it appends the creation date to the file name
		/// </summary>
		protected bool		wantDate = false;
		/// <summary>
		/// if TRUE, then it appends the creation time to the file name
		/// </summary>
		protected bool		wantTime = false;

		/// <summary>
		/// if TRUE AND wantDate is TRUE, then it starts a new file at the first
		/// write that occurs on a different day from the last write.  If wantDate
		/// is FALSE, then trying to set this to TRUE throws an exception.
		/// </summary>
		protected bool		recycleDaily = false;

		/// <summary>
		/// Day when we last wrote to the current file.  Set when the file is written to.
		/// </summary>
		protected DateTime	lastWriteDay;

		/// <summary>
		/// File counter for multi-file modes (incremented in buildHeader () )
		/// </summary>
		protected int		curFileCount = 0;

		/// <summary>
		/// Max allowed file size, in bytes;  0 = "no max size"
		/// </summary>
		protected long		maxSize = 0;

		/// <summary>
		/// if TRUE,  then we start a new file when the current one hits maxSize;
		/// if FALSE, then we stop writing when the current file hits maxSize.
		/// Trying to set to TRUE with maxSize = 0 throws an exception.
		/// </summary>
		protected bool		newWhenMax = false;

		/// <summary>
		/// Set to TRUE if we are to not write data to file anymore.  Typically this
		/// happens when maxSize > 0, newWhenMax = FALSE, and the file size has been
		/// exceeded.  Set and cleared in text only.
		/// </summary>
		protected bool		doneWriting = false;

		/// <summary>
		/// Header information storage -- save the actual ChainInfo object
		/// </summary>
		protected ChainInfo	initValues;


		// Buffer the data we write, so we don't write too often:
		/// <summary>
		/// How many seconds do we want to buffer
		/// </summary>
		protected float			bufferSeconds = 2F;
		/// <summary>
		/// minimum sample period below which we sample
		/// (and above which we don't sample)
		/// </summary>
		protected float			minBufferSampleSec = 1F;
		private StringBuilder	textBuffer;
		private float[]			binaryBuffer;
		private int				bufferContentsSize = 0;
		//private DateTime		lastWrite;
		private bool			writeInProgress;
		private bool			doBuffering;
		private int				buffSize;		// what we initialize the buffer size to

		/// <summary>
		/// basic constructor:  only requires file path and file name.
		/// 
		/// For greater flexibility in specifying a file name, see other constructors.
		/// </summary>
		/// <param name="filePath">Must be an existing file path, with or without trailing slash</param>
		/// <param name="fileName">Must be a valid file name that does NOT already exist in filePath</param>
		public FileWriter( string filePath, string fileName )
		{
			// Check for existing path:
			FileHandler.validatePath ( filePath );

			// Make sure file does not exist:
			FileHandler.fileNotExists ( filePath, fileName, false );

			// Try creating the new file:
			fCurFullName = getFileFullName ( FileHandler.cleanFileName(fileName) );
			FileStream fs = File.Create ( FileHandler.cleanPath(filePath) + fCurFullName );
			fs.Close();

			// If we created it successfully, then we're done -- just save the values:
			fPath = FileHandler.cleanPath ( filePath );
			fName = FileHandler.cleanFileName ( fileName );
			fExt = FileHandler.getExt ( fName );
		}


		/// <summary>
		/// Allows setting behavior dependent on reaching a certain file size.
		/// Using this constructor with createnewFileWhenMaxSize = TRUE
		/// will set appendDateToFileName and appendTimeToFileName to TRUE.
		/// </summary>
		/// <param name="filePath">Must be an existing file path, with or without 
		///				trailing slash</param>
		/// <param name="fileName">Must be a valid file name that does NOT already 
		///				exist in filePath (including any of its permutations of the
		///				form "nameroot*.ext")</param>
		/// <param name="writeValuesAsAscii">If TRUE, then values will be written in
		///				ASCII (human-readable) format;  if FALSE, they will be 
		///				written in binary format.</param>
		/// <param name="maxAllowedFileSize">If zero, then there is no limitation on
		///				file size;  otherwise, the file size will be limited to the
		///				specified number of bytes.</param>
		/// <param name="createNewFileWhenMaxSize">What to do if the file size grows
		///				past the specified number of bytes.  If TRUE, then a new
		///				file is created;  if FALSE, then writing stops.</param>
		public FileWriter ( string filePath, string fileName,
			bool writeValuesAsAscii,
			long maxAllowedFileSize, bool createNewFileWhenMaxSize )
		{
			// Check for existing path:
			FileHandler.validatePath ( filePath );

			// Make sure file and its permutations do not exist:
			FileHandler.fileNotExists ( filePath, fileName, true );

			if ( createNewFileWhenMaxSize )
			{
				// Need to set these before trying file creation:
				wantDate = true;
				wantTime = true;
			}
			// Try creating the new file:
			fCurFullName = getFileFullName ( FileHandler.cleanFileName(fileName) );
			FileStream fs = File.Create ( FileHandler.cleanPath(filePath) + fCurFullName );
			fs.Close();

			// If we created it successfully, then we're done -- just save the values:
			fPath = FileHandler.cleanPath ( filePath );
			fName = FileHandler.cleanFileName ( fileName );
			fExt = FileHandler.getExt ( fName );
			writeAsAscii = writeValuesAsAscii;
			if ( maxAllowedFileSize <= 0 )
				this.maxSize = 0;
			else
				maxSize = maxAllowedFileSize;

			newWhenMax = createNewFileWhenMaxSize;
		}

		// Filename-params Constructor:

		/// <summary>
		/// Collection of parameters for tweaking file name.
		/// </summary>
		/// <param name="filePath">Must be an existing file path, with or without 
		///				trailing slash</param>
		/// <param name="fileName">Must be a valid file name that does NOT already 
		///				exist in filePath (including any of its permutations of the
		///				form "nameroot*.ext")</param>
		/// <param name="writeValuesAsAscii">If TRUE, then values will be written in
		///				ASCII (human-readable) format;  if FALSE, they will be 
		///				written in binary format.</param>
		/// <param name="createNewFileDaily">If TRUE, then the first write on a date
		///				different from the last write causes a new file to be 
		///				created.  This requires that appendDateToFileName = TRUE</param>
		/// <param name="overwriteExistingFile">If TRUE, then file creation will
		///				overwrite an existing file with the same name.</param>
		/// <param name="appendExistingFile">If TRUE, then file creation will do
		///				nothing if a file with the same name already exists.  Note
		///				that this requires that overwriteExistingFile = FALSE</param>
		/// <param name="appendDateToFileName">If TRUE, then the creation date is
		///				added to the file name (before the extension, if any)</param>
		/// <param name="appendTimeToFileName">If TRUE, then the creation time is
		///				added to the file name (before the extension, if any).  If
		///				appendDateToFileName = TRUE, then the time is added AFTER
		///				the date.</param>
		public FileWriter ( string filePath, string fileName, 
			bool writeValuesAsAscii, bool createNewFileDaily,
			bool overwriteExistingFile, bool appendExistingFile,
			bool appendDateToFileName, bool appendTimeToFileName )
		{
			checkNewFileDaily ( createNewFileDaily, appendDateToFileName );

			// Check for existing path:
			FileHandler.validatePath ( filePath );

			if ( ! overwriteExistingFile && ! appendExistingFile )
			{
				// Make sure file and its permutations do not exist:
				FileHandler.fileNotExists ( filePath, fileName, true );

				// Need to set these before trying file creation:
				wantDate = appendDateToFileName;
				wantTime = appendTimeToFileName;

				// Try creating the new file:
				fCurFullName = getFileFullName ( FileHandler.cleanFileName(fileName) );
				FileStream fs = File.Create ( FileHandler.cleanPath(filePath) + fCurFullName );
				fs.Close();
			}
			else
			{
				// Need to set these before trying file creation:
				wantDate = appendDateToFileName;
				wantTime = appendTimeToFileName;

				fCurFullName = getFileFullName ( FileHandler.cleanFileName(fileName) );
				if ( ! File.Exists ( FileHandler.cleanPath(filePath) + fCurFullName ) )
				{
					// Not there -- create:
					FileStream fs = File.Create ( FileHandler.cleanPath(filePath) + fCurFullName );
					fs.Close();
				}
			}

			// If we got here, then we're done testing -- just save the values:
			fPath = FileHandler.cleanPath ( filePath );
			fName = FileHandler.cleanFileName ( fileName );
			fExt = FileHandler.getExt ( fName );
			writeAsAscii = writeValuesAsAscii;
			recycleDaily = createNewFileDaily;
			overwriteExisting = overwriteExistingFile;
			appendExisting = appendExistingFile;
			if ( maxAllowedFileSize <= 0 )
				maxSize = 0;
			else
				maxSize = maxAllowedFileSize;

			newWhenMax = createNewFileWhenMaxSize;
		}


		/// <summary>
		/// This constructor exposes all externally-settable parameters.  Note that
		/// some interdependencies between parameters may cause exceptions to be
		/// thrown if values don't mesh appropriately.
		/// </summary>
		/// <param name="filePath">Must be an existing file path, with or without 
		///				trailing slash</param>
		/// <param name="fileName">Must be a valid file name that does NOT already 
		///				exist in filePath (including any of its permutations of the
		///				form "nameroot*.ext")</param>
		/// <param name="writeValuesAsAscii">If TRUE, then values will be written in
		///				ASCII (human-readable) format;  if FALSE, they will be 
		///				written in binary format.</param>
		/// <param name="createNewFileDaily">If TRUE, then the first write on a date
		///				different from the last write causes a new file to be 
		///				created.  This requires that appendDateToFileName = TRUE</param>
		/// <param name="overwriteExistingFile">If TRUE, then file creation will
		///				overwrite an existing file with the same name.</param>
		/// <param name="appendExistingFile">If TRUE, then file creation will do
		///				nothing if a file with the same name already exists.  Note
		///				that this requires that overwriteExistingFile = FALSE</param>
		/// <param name="appendDateToFileName">If TRUE, then the creation date is
		///				added to the file name (before the extension, if any)</param>
		/// <param name="appendTimeToFileName">If TRUE, then the creation time is
		///				added to the file name (before the extension, if any).  If
		///				appendDateToFileName = TRUE, then the time is added AFTER
		///				the date.</param>
		/// <param name="maxAllowedFileSize">If zero, then there is no limitation on
		///				file size;  otherwise, the file size will be limited to the
		///				specified number of bytes.</param>
		/// <param name="createNewFileWhenMaxSize">What to do if the file size grows
		///				past the specified number of bytes.  If TRUE, then a new
		///				file is created;  if FALSE, then writing stops.  Note that
		///				TRUE requires that appendDateToFileName and/or 
		///				appendTimeToFileName be TRUE.</param>
		public FileWriter ( string filePath, string fileName, 
							bool writeValuesAsAscii, bool createNewFileDaily,
							bool overwriteExistingFile, bool appendExistingFile,
							bool appendDateToFileName, bool appendTimeToFileName,
							long maxAllowedFileSize, bool createNewFileWhenMaxSize )
		{
			checkNewFileDaily ( createNewFileDaily, appendDateToFileName );

			checkMaxSizeParams (maxAllowedFileSize, createNewFileWhenMaxSize,
								appendDateToFileName, appendTimeToFileName );

			// Check for existing path:
			FileHandler.validatePath ( filePath );

			if ( ! overwriteExistingFile && ! appendExistingFile )
			{
				// Make sure file and its permutations do not exist:
				FileHandler.fileNotExists ( filePath, fileName, true );

				// Need to set these before trying file creation:
				wantDate = appendDateToFileName;
				wantTime = appendTimeToFileName;

				// Get the file name:
				fCurFullName = getFileFullName ( FileHandler.cleanFileName(fileName) );
				// Try creating the new file:
				FileStream fs = File.Create ( FileHandler.cleanPath(filePath) + fCurFullName );
				fs.Close();
			}
			else
			{
				// Need to set these before trying file creation:
				wantDate = appendDateToFileName;
				wantTime = appendTimeToFileName;

				// Get the file name:
				fCurFullName = getFileFullName ( FileHandler.cleanFileName(fileName) );

				if ( ! File.Exists ( FileHandler.cleanPath(filePath) + fCurFullName ) )
				{
					// Not there -- create:
					FileStream fs = File.Create ( FileHandler.cleanPath(filePath) + fCurFullName );
					fs.Close();
				}
			}

			// If we got here, then we're done testing -- just save the values:
			fPath = FileHandler.cleanPath ( filePath );
			fName = FileHandler.cleanFileName ( fileName );
			fExt = FileHandler.getExt ( fName );
			writeAsAscii = writeValuesAsAscii;
			recycleDaily = createNewFileDaily;
			overwriteExisting = overwriteExistingFile;
			appendExisting = appendExistingFile;
			if ( maxAllowedFileSize <= 0 )
				maxSize = 0;
			else
				maxSize = maxAllowedFileSize;

			newWhenMax = createNewFileWhenMaxSize;
		}

		/// <summary>
		/// Release any resources that require manual releasing for proper shutdown
		/// </summary>
		~FileWriter ()
		{
			this.flushBuffer();
		}

		private void checkNewFileDaily (bool createNewFileDaily, bool appendDateToFileName)
		{
			if ( createNewFileDaily && ! appendDateToFileName )
			{
				throw new ArgumentException ( "createNewFileDaily (" + createNewFileDaily 
					+ ") must cannot be TRUE if appendDateToFileName is FALSE.",
					"createNewFileDaily" );
			}
		}

		/// <summary>
		/// Initialize the write buffer
		/// </summary>
		private void initBuffer ()
		{
			// Is something there already?
			if ( this.bufferContentsSize > 0 )
			{
				// Yes -- flush the buffer:
				this.flushBuffer();
			}

			if ( this.initValues.samplingPeriodSec < minBufferSampleSec )
			{
				// Sub-second sampling interval -- use bufferSeconds-second buffering:
				this.doBuffering = true;
				buffSize = (int) Math.Ceiling ( bufferSeconds / initValues.samplingPeriodSec );

				if ( this.writeAsAscii )
				{
					// Only need a text buffer - create to size:
					this.textBuffer = new StringBuilder ( buffSize );
				}
				else
				{
					// Create binary buffer at starting size:
					this.binaryBuffer = new float [ buffSize ];
					// We also may need a text buffer (for the header!)
					// but we don't know the size:
					this.textBuffer = new StringBuilder ();
				}
			}
			else
			{
				// second-plus sampling interval -- don't bother with buffering:
				this.doBuffering = false;
			}

			this.bufferContentsSize = 0;
		}


		#region IReceiver Members

		/// <summary>
		/// Begin file writing, by ensuring the file exists and applying a header to it.
		/// The header contents are based on the data in iData, and possibly other data.
		/// </summary>
		/// <param name="iData">Information on the data to be written</param>
		public void init(ChainInfo iData)
		{
			// Sanity check:
			if ( ! File.Exists ( fPath + fCurFullName ) )
			{
				throw new ApplicationException ( "Target file \"" + fPath + fCurFullName
					+ "\" should have been created by the Constructor but it does not exist;  cannot continue." );
			}
			// Save init data:
			initValues = iData;	// saves whole thing for later detailed use

			// (re)initialize the write buffer:
			initBuffer ();

			// Get then write header:
			string header = buildHeader ( );
			writeToFile ( header, true );
			// That's it!
		}

		/// <summary>
		/// Write the new value to the file.  Includes the logic to check for: 
		/// size overflow (if needed);
		/// "new day, new file" (if needed).
		/// </summary>
		/// <param name="newValue">new value to be written to file</param>
		public void addValue(Sample newValue)
		{
			// Check for new day:
			if ( recycleDaily &&
				 lastWriteDay.Date < DateTime.Today )
			{
				// Flush the buffer:
				flushBuffer ();
				// OK, time to get a new file:
				fCurFullName = getFileFullName();
				// Give it a header:
				writeToFile ( buildHeader(), true );
				// Reset flag as appropriate:
				doneWriting = false;
			}
			else if ( maxSize > 0 )
			{
				// Check for size overflow:
				int newSize = getDiskSize ( newValue.sampleValue );
				newSize += this.bufferContentsSize;
				FileInfo fi = new FileInfo ( this.fPath + this.fCurFullName );
				if ( fi.Length + newSize > maxSize )
				{
					// OK, it would go over:
					if ( newWhenMax )
					{
						// Flush the buffer:
						flushBuffer ();
						// OK, start a new file:
						fCurFullName = getFileFullName();
						// Give it a header:
						writeToFile ( buildHeader(), true );
						// Reset flag as appropriate:
						doneWriting = false;
						// NOTE:  if maxSize > newSize, we STILL write the header
						// plus one value!
					}
					else
					{
						// We stop writing (until further notice?):
						doneWriting = true;
					}
				}
			}

			if ( ! doneWriting )
			{
				// All set for writing -- go for it:
				writeToFile ( newValue.sampleValue, false );
			}
		}

		/// <summary>
		/// Write the array of values to the file.
		/// Currently just calls addValue ( Sample ) a number of times;  a more
		/// efficient algorithm would build a string to be added, and add it all
		/// at once, doing the necessary checks -- same as addValue ( Sample ) 
		/// does, only made more complex by the presence of a variable number of
		/// values, where some may fit in the old file but not all, etc.
		/// If that were done, then addValue ( Sample ) could in turn call this
		/// method...
		/// </summary>
		/// <param name="newValues">Array of input values</param>
		public void addValues(Samples newValues)
		{
			for ( int i = 0; i < newValues.size; i++ )
			{
				addValue ( newValues[i] );
			}
		}

		#endregion

		/// <summary>
		/// Retrieves appropriate header information from the InitInfo, and turns it
		/// into a long string formatted as tagname/value pair rows, like this:
        /// <code># [tagname]:  [value] [OpenMedICUtils.newLine]</code>
		/// The header is terminated by a row that starts with:
		/// <code>##</code>
		/// When parsing the file, one should ignore all that is on the same line
        /// as the header terminator, through (including) a [OpenMedICUtils.newLine] 
		/// that will end the line.
		/// </summary>
		/// <returns>A string that can be written to a file to serve as comprehensive
		///			header information</returns>
		private string buildHeader ( )
		{
			StringBuilder curHeader = new StringBuilder ();
			// Header Delimiter (if any):
			curHeader.Append ( FileHandler.headerStartDelim );
			// Start retrieving properties
			// - FileWriter-specific:
			addTagValuePairs (	curHeader, 
								ChainInfo.getTagText ( ChainInfo.varTags.dataFormat ),
								valuesWriteMode );
			addTagValuePairs (	curHeader,
								ChainInfo.getTagText ( ChainInfo.varTags.writerClassName ),
								this.GetType().ToString() );
			// - Local-specific:
			if ( this.createNewFileDaily || this.createNewFileWhenMaxSize )
			{
				// OK, we create new files -- track which file this is and when started:
				curFileCount++;
				addTagValuePairs (	curHeader, 
					ChainInfo.getTagText ( ChainInfo.varTags.multiFileNum ), 
					curFileCount.ToString() );
				addTagValuePairs (	curHeader, 
					ChainInfo.getTagText ( ChainInfo.varTags.startTime ), 
					DateTime.Now.ToLongDateString () + " " + DateTime.Now.ToLongTimeString () );
			}
			// - ChainInfo-specific:
			addTagValuePairs (	curHeader, 
								ChainInfo.getTagText ( ChainInfo.varTags.samplingPeriodSec ),
								initValues.samplingPeriodSec.ToString() );

			// - DataInfo-specific:
			DataInfo di = initValues.dataInfo;
			if (di == null)
			{
				// Data info not available:
				addTagValuePairs (	curHeader,
									ChainInfo.getTagText(ChainInfo.varTags.dataDescription),
									"[Data information not available]");
			}
			else
			{
				addTagValuePairs(curHeader,
									ChainInfo.getTagText(ChainInfo.varTags.dataDescription),
									di.DataDescription);
				addTagValuePairs(curHeader,
									ChainInfo.getTagText(ChainInfo.varTags.startDateTime),
									di.StartDateTime);
				addTagValuePairs(curHeader,
									ChainInfo.getTagText(ChainInfo.varTags.bitsPerSample),
									di.BitsPerSample.ToString());
				addTagValuePairs(curHeader,
									ChainInfo.getTagText(ChainInfo.varTags.zeroReferenceVoltage),
									di.ZeroReferenceVoltage);
				addTagValuePairs(curHeader,
									ChainInfo.getTagText(ChainInfo.varTags.fullScaleReferenceVoltage),
									di.FullScaleReferenceVoltage);
				addTagValuePairs(curHeader,
									ChainInfo.getTagText(ChainInfo.varTags.scaleMultiplier),
									di.ScaleMultiplier);
				addTagValuePairs(curHeader,
									ChainInfo.getTagText(ChainInfo.varTags.zeroOffset),
									di.ZeroOffset);
			}

			// - PatientInfo-specific:
			PatientInfo pi = initValues.patientInfo;
			if (pi == null)
			{
				// Data info not available:
				addTagValuePairs (	curHeader,
									ChainInfo.getTagText(ChainInfo.varTags.PatientID),
									"[Patient information not available]");
			}
			else
			{
				addTagValuePairs(curHeader,
									ChainInfo.getTagText(ChainInfo.varTags.PatientID),
									pi.PatientID);
				addTagValuePairs(curHeader,
									ChainInfo.getTagText(ChainInfo.varTags.FullName),
									pi.FullName);
				addTagValuePairs(curHeader,
									ChainInfo.getTagText(ChainInfo.varTags.FullAddress),
									pi.FullAddress);
				addTagValuePairs(curHeader,
									ChainInfo.getTagText(ChainInfo.varTags.Age),
									pi.Age.ToString());
				addTagValuePairs(curHeader,
									ChainInfo.getTagText(ChainInfo.varTags.DOB),
									pi.DOBstring);
			}

			// Now we add the header-end delimiter:
			curHeader.Append ( FileHandler.headerEndDelim );

			return curHeader.ToString ();
		}

		private void addTagValuePairs ( StringBuilder curHeader, string Tag, string Value )
		{
			if ( Tag != "[Unknown]" && ! OpenMedICUtils.isEmpty ( Value ) )
			{
				// - Add the starting delim, the Tag, and the separator:
				curHeader.Append ( FileHandler.headerRowLeader ).Append ( Tag ).Append ( FileHandler.headerTagDelim );
				// - Add the Value and the end delim:
				curHeader.Append ( Value ).Append ( FileHandler.headerRowTrailer );
			}
			else
			{
				// Unknown tag (therefore ambiguous value meaning),
				// OR no value available (therefore useless) --
				// Ignore this tag
			}
		}

		/// <summary>
		/// Writes the specified string to file, with buffering.  If immediate, then
		/// it flushes the buffer immediately.  Note that the string is written as
		/// ASCII characters, so this method is not usable to write binary data to 
        /// the file.  A NewLine (OpenMedICUtils.newLine) is added at the end of the string.
		/// </summary>
		/// <param name="Value">ASCII string to append to the file.  If this is null, then
		///				nothing is appended (not even the line feed).  To add an empty
		///				line, submit an empty string ("") instead</param>
		/// <param name="immediate">If TRUE, then write immediately what is in the buffer
		///				(i.e. flush the buffer whether it's time or not)</param>
		private void writeToFile ( string Value, bool immediate )
		{
			lastWriteDay = DateTime.Now;

			lock ( this )	// This is a bad place to have concurrent threads:
			{
				// Start by throwing this into the text buffer:
				if ( Value != null )	// Don't add NULL, BUT add empty string:
				{
					textBuffer.Append ( Value ).Append ( OpenMedICUtils.newLine );
					if ( writeAsAscii )
						bufferContentsSize ++;
				}

				// Time to write to disk?
				if (   ! doBuffering 
					|| bufferContentsSize >= buffSize
					|| ! writeAsAscii
					|| immediate )
				{
					// EITHER, we are not buffering;  OR, the buffer is full;  OR, we are in BINARY
					// mode and we're adding ASCII text -- flush the buffer(s):
					// - ASCII buffer (will check and flush the binary buffer if needed):
					writeTextBufferToFile ();
				}
			}
		}

		/// <summary>
		/// WARNING -- NEVER CALL THIS METHOD DIRECTLY!
		/// Always use writeToFile ( string, bool )
		/// </summary>
		private void writeTextBufferToFile ()
		{
			// If still writing, just WAIT:
			while ( this.writeInProgress )
			{
				System.Threading.Thread.Sleep ( 200 );	// wait 200 ms. before checking again
			}

			// - Binary buffer (if necessary):
			if ( ! writeAsAscii && bufferContentsSize > 0 )
			{
				// Already have stuff in the binary buffer -- write that first:
				writeBinaryBufferToFile ();
			}

			// - Text buffer (if necessary):
			if ( textBuffer.ToString().Length > 0 )
			{
				StreamWriter fNew = new StreamWriter ( fPath + fCurFullName, true );
				try
				{
					fNew.Write ( textBuffer.ToString() );
				}
				finally
				{
					fNew.Close();
				}
			}

			// Flush the buffer:
			textBuffer = new StringBuilder ( buffSize );
			bufferContentsSize = 0;
		}

		/// <summary>
		/// Manages call-backs for async file writes.
		/// </summary>
		private void writeCallback ()
		{
			// ...
			writeInProgress = false;
		}

		/// <summary>
		/// Writes the specified number to file.
		/// If we are writing values in ASCIII format, then this method calls
		/// writeToFile ( string );  if not, then it writes it as a binary value.  
		/// Note that values written as binary are NOT a human-readable series of 
		/// characters, and there are no delimiters between values.
		/// Note also, that if you only want to flush the buffers without writing 
		/// anything new, you cannot use this method;  use instead, 
		/// writeToFile ( (string) null, true ).
		/// </summary>
		/// <param name="Value">float value to append to the file</param>
		/// <param name="immediate">If TRUE, then write immediately what is in the buffer
		///				(i.e. flush the buffer whether it's time or not)</param>
		private void writeToFile ( float Value, bool immediate )
		{
			if ( this.writeAsAscii )
			{
				// Use the other method:
				writeToFile ( Value.ToString(), immediate );
			}
			else
			{
				lock ( this )	// This is a bad place to have concurrent threads:
				{
					lastWriteDay = DateTime.Now;

					// Start by throwing this into the float buffer:
					this.binaryBuffer[bufferContentsSize++] = Value;

					// Time to write to disk?
					if (   ! this.doBuffering 
						|| this.bufferContentsSize == this.buffSize
						|| immediate )
					{
						// EITHER, we are not buffering;  OR, the buffer is full;
						// OR, want immediate unconditional write -- flush the buffer:
						writeBinaryBufferToFile ();
					}
				}
			}
		}

		/// <summary>
		/// WARNING -- NEVER CALL THIS METHOD DIRECTLY!
		/// Always use writeToFile ( float, bool )
		/// </summary>
		private void writeBinaryBufferToFile ()
		{
			if ( ! writeAsAscii && bufferContentsSize > 0 )
			{
				// If still writing, just WAIT:
				while ( this.writeInProgress )
				{
					System.Threading.Thread.Sleep ( 200 );	// wait 200 ms. before checking again
				}

				// Open file for append(or create), write only:
				FileStream tgt = new FileStream(fPath + fCurFullName, 
												FileMode.Append, 
												FileAccess.Write );
				BinaryWriter writer = new BinaryWriter ( tgt );
				// Throw the data in there:
				try
				{
					for ( int i = 0; i < binaryBuffer.Length; i++ )
					{
						writer.Write ( binaryBuffer[i] );
					}
				}	
				finally
				{
					writer.Close ();
					tgt.Close ();
				}

				// Flush buffer:
				bufferContentsSize = 0;
			}

		}

		/// <summary>
		/// Writes whatever is in the buffers to file.
		/// </summary>
		private void flushBuffer ()
		{
			lock ( this )
			{
				// WriteTextBuffer does all we need:
				this.writeTextBufferToFile ();
			}
		}

		private int getDiskSize ( float val )
		{
			if ( this.writeAsAscii )
			{
				// Return the length of the text plus 1 for the newline delim:
				return val.ToString().Length + 1;
			}
			else
			{
				// return the actual storage size:  for a float, that's always 4:
				return 4;
			}
		}

		/// <summary>
		/// Generate the appropriate file name based on the root name and
		/// all the relevant flag values.
		/// </summary>
		/// <returns>A full file name</returns>
		private string getFileFullName ()
		{
			return getFileFullName ( this.fName );
		}

		/// <summary>
		/// Generate the appropriate file name based on the passed param and
		/// all the relevant flag values.
		/// 
		/// Relevant flags are:
		/// 
		/// -&gt; wantDate;
		///	-&gt; wantTime.
		/// </summary>
		/// <param name="fileName">a file name to use in building the full name</param>
		/// <returns>A full file name</returns>
		private string getFileFullName ( string fileName )
		{
			StringBuilder fullName = new StringBuilder ();
			fullName.Append( getPreExt ( fileName ) );
			string ext = FileHandler.getExt ( fileName );
			DateTime curStamp = DateTime.Now;

			if ( ext.Length > 0 )
			{
				// Add period to ext:
				ext = "." + ext;
			}
			// Start adding as appropriate:
			if ( this.wantDate )
			{	// Add:  "_YYYY-MM-DD"
				fullName.Append ( "_" ).Append ( curStamp.Year.ToString("0000") );
				fullName.Append ( "-" ).Append ( curStamp.Month.ToString("00") );
				fullName.Append ( "-" ).Append ( curStamp.Day.ToString("00") );
			}
			if ( this.wantTime )
			{	// Add:  "_HH-NN-SS"
				fullName.Append ( "_" ).Append ( curStamp.Hour.ToString("00") );
				fullName.Append ( "-" ).Append ( curStamp.Minute.ToString("00") );
				fullName.Append ( "-" ).Append ( curStamp.Second.ToString("00") );
			}

			// Put back the extension, if any: 
			fullName.Append ( ext );

			// That's it for now:
			return fullName.ToString();
		}

		/// <summary>
		/// Returns all the text in fileName that comes before the extension.
		/// If there is no extension, the returned string is identical to fileName.
		/// If there is an extension, the period is NOT returned.
		/// </summary>
		/// <param name="fileName">File name to parse</param>
		/// <returns>All the text in fileName that comes before the extension</returns>
		private string getPreExt ( string fileName )
		{
			int pos = fileName.LastIndexOf ( "." );
			if ( pos == -1 )
			{
				return fileName;
			}
			else
			{
				return fileName.Substring ( 0, pos );
			}
		}

		/// <summary>
		/// The path to the file;  returned value will always include a trailing delimiter
		/// </summary>
		public string filePath
		{
			get
			{
				return fPath;
			}
//			set
//			{
//				fPath = FileHandler.cleanPath (value);
//			}
		}

		/// <summary>
		/// Root of the name of the file.  If any filename modifier is used (e.g.,
		/// appendDateToFileName = TRUE and/or appendTimeToFileName = TRUE), then
		/// the actual filename is different (see fileFullName);  otherwise they
		/// are the same.
		/// </summary>
		public string fileName
		{
			get
			{
				return this.fName;
			}
		}

		/// <summary>
		/// Name of the actual file being written to.  In some cases this is
		/// the same as fileName; if any filename modifier is used (e.g.,
		/// appendDateToFileName = TRUE and/or appendTimeToFileName = TRUE),
		/// then they are different.
		/// </summary>
		public string fileFullName
		{
			get
			{
				return this.getFileFullName ();
			}
		}

		/// <summary>
		/// path and name of the actual file being written to.  This is 
		/// equivalent to this.filePath + this.fileFullName .
		/// </summary>
		public string filePathName
		{
			get
			{
				return this.filePath + this.fileFullName;
			}
		}

		/// <summary>
		/// Whether the data values are written as ASCII character or as
		/// binary values.  A float value will occupy up to 14 ASCII
		/// characters (i.e. 14 bytes on disk), but always exactly 4 bytes
		/// as a binary.  ASCII has the advantage of being readable by 
		/// humans and many other programs, such as spreadsheet programs.
		/// </summary>
		public bool writeValuesAsAscii
		{
			get
			{
				return writeAsAscii;
			}
		}

		/// <summary>
		/// What mode is being used for data writing:  "ASCII"  or "Binary"
		/// </summary>
		public string valuesWriteMode
		{
			get
			{
				if ( writeAsAscii )
					return "ASCII";
				else
					return "Binary";
			}
		}

		/// <summary>
		/// Whether we overwrite an existing file when attempting to create a
		/// new file.  If this is false, the behavior depends on the value of
		/// appendExistingFile.
		/// </summary>
		public bool overwriteExistingFile
		{
			get
			{
				return overwriteExisting;
			}
		}

		/// <summary>
		/// What to do if the file we are trying to create already exists, and
		/// overwriteExistingFile is FALSE.  If this is TRUE, then we append to
		/// the existing file (with a new header);  if this is FALSE, then we
		/// raise an exception.  Note also that this cannot be TRUE if 
		/// writeValuesAsAscii is false, since (at this time) binary-value 
		/// files are incompatible with multiple headers.
		/// </summary>
		public bool appendExistingFile
		{
			get
			{
				return appendExisting;
			}
		}

		/// <summary>
		/// If TRUE, then the actual filename is formed as follows:
		/// [fileName minus extension, if any]_YYYY-MM-DD[.extension, if any]
		/// where YYYY-MM-DD are the date of file creation.
		/// This can be combined with appendTimeToFileName;  if so, the actual
		/// filename is formed as follows:
		/// [fileName minus extension, if any]_YYYY-MM-DD_HH-NN-SS[.extension, if any]
		/// </summary>
		public bool appendDateToFileName
		{
			get
			{
				return wantDate;
			}
		}

		/// <summary>
		/// If TRUE, then the actual filename is formed as follows:
		/// [fileName minus extension, if any]_HH-NN-SS[.extension, if any]
		/// where HH-NN-SS are the hour, minutes, second of file creation.
		/// This can be combined with appendDateToFileName;  if so, the actual
		/// filename is formed as follows:
		/// [fileName minus extension, if any]_YYYY-MM-DD_HH-NN-SS[.extension, if any]
		/// </summary>
		public bool appendTimeToFileName
		{
			get
			{
				return wantTime;
			}
		}

		/// <summary>
		/// If TRUE, then the first write on a day different from the last write
		/// creates a new file, complete with header.  This option requires that
		/// appendDateToFileName be TRUE.
		/// </summary>
		public bool createNewFileDaily
		{
			get
			{
				return recycleDaily;
			}
		}

		/// <summary>
		/// Does sanity-checking on the passed values before saving them to
		/// the internal variables.
		/// </summary>
		private void checkAndSetMaxSizeParams ( long maxAllowSize,
			bool createNewWhenSize )
		{
			if ( checkMaxSizeParams(maxAllowSize, createNewWhenSize,
				 this.wantDate, this.wantTime) )
			{
				// If no exception, set the local values:
				if ( doneWriting && maxSize > 0 && maxSize < maxAllowSize )
				{
					// Reset blocking flag:
					doneWriting = false;
				}
				this.maxSize = maxAllowSize;
				this.newWhenMax = createNewWhenSize;
			}
		}

		/// <summary>
		/// Does sanity-checking on the passed input parameters.
		/// </summary>
		/// <returns>TRUE if the params are consistent with each other</returns>
		private bool checkMaxSizeParams ( long maxAllowSize,
			bool createNewWhenSize, bool wantDateInName, bool wantTimeInName )
		{
			if ( maxAllowSize > 0 )
			{
				// Otherwise, the others don't matter
				if ( createNewWhenSize )
				{
					// Otherwise the others don't matter
					if ( ! wantDateInName && ! wantTimeInName )
					{
						throw new ArgumentException
							( "You cannot set createNewFileWhenMaxSize to TRUE and "
							+ "maxAllowedFileSize > 0 when appendDateToFileName AND "
							+ "appendTimeToFileName are both false." );
					}
				}
			}
			// If no exception, return success:
			return true;
		}

		/// <summary>
		/// Maximum allowed file size, in bytes.  If the next write would cause
		/// the file to exceed the specified size, then writing to that file stops.
		/// If createNewFileWhenMaxSize is TRUE, then a new file is created;  this
		/// also requires that appendDateToFileName and/or appendTimeToFileName
		/// be TRUE.  If createNewFileWhenMaxSize is FALSE, then all writing stops.
		/// 
		/// Note that if this value is changed after starting, then the next write
		/// will take it into account, but the previous writes will be unaffected.
		/// In other words, if you set this to less than the current file size, the
		/// size will not become smaller.
		/// </summary>
		public long maxAllowedFileSize
		{
			get
			{
				return maxSize;
			}
			set
			{
				checkAndSetMaxSizeParams ( value, newWhenMax );
			}
		}

		/// <summary>
		/// Specifies what to do when the file grows to maxAllowedFileSize.
		/// If TRUE, then a new file is created (this requires that 
		/// appendDateToFileName and/or appendTimeToFileName be TRUE);
		/// If FALSE, then all writing stops.
		/// If maxAllowedFileSize is 0 (i.e. off), this flag is not used.
		/// </summary>
		public bool createNewFileWhenMaxSize
		{
			get
			{
				return newWhenMax;
			}
			set
			{
				checkAndSetMaxSizeParams ( maxSize, value );
			}

		}

	}	// END of class FileWriter
}
