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
	/// Reads the header and data from a file, typically generated by a FileWriter
	/// class, and outputs the data as a DataSource would.
	/// 
	/// Note that, for now, a FileReader will only read data from a single file; if
	/// a FileWriter wrote multiple files for a single acquisition (e.g. because 
	/// they were changed every day, or were changed whenever they reached a 
	/// certain size), then new functionality would be needed to read the 
	/// consecutive files.
	/// </summary>
	public class FileReader : DataSource
	{

		#region local (protected & private) variables

		// Store the file path & name:
		/// <summary>
		/// Path location for the file being read
		/// </summary>
		protected string fPath;
		/// <summary>
		/// Name of file being read
		/// </summary>
		protected string fName;
		/// <summary>
		/// Path/Name being read ( = fPath + "/" + fName)
		/// </summary>
		protected string fPathName;	// = fPath + fName

		// Read Cache (buffering):
		/// <summary>
		/// How many seconds do we want to buffer
		/// </summary>
		protected float			bufferSeconds = 2F;
		/// <summary>
		/// minimum sample period below which we buffer
		/// (and above which we don't buffer)
		/// </summary>
		protected float			minBufferSampleSec = 1F;

		/// <summary>
		/// Buffer for ASCII files
		/// </summary>
		private string[]		textBuffer;
		/// <summary>
		/// Buffer for binary files
		/// </summary>
		private float[]			binaryBuffer;
		/// <summary>
		/// Current value in the buffer (i.e. the next one to retrieve)
		/// </summary>
		private int				bufferPointer = 0;

		//private bool			readInProgress;	// Used for async reading with look-ahead
		//private bool			doBuffering;
		private int				buffSize;		// what we initialize the buffer size to
		private bool			doneReading = false;

		/// <summary>
		/// True if we start again from the start of the file after hitting the end
		/// </summary>
		protected bool			repeat = false;

		/// <summary>
		/// Pointer inside the file, for SEEK methods:
		/// </summary>
		private int				curReadStartPos = 0;

		/// <summary>
		/// How is data stored?  Ascii text (readable but inefficient) 
		/// vs. binary (efficient but unreadable)
		/// </summary>
		protected bool		writtenAsAscii = true;

		/// <summary>
		/// Header information storage -- save the actual ChainInfo object
		/// </summary>
		protected ChainInfo	initValues;

		#endregion


		#region constructors

		/*	Disable this constructor -- there is no way right now to set file path & name in 
		 * another way!

		/// <summary>
		/// Minimal constructor.  Note that if you use this constructor, you must
		/// set filePath and fileName before calling init(), otherwise an exception
		/// will be raised.
		/// Also loads header from file.
		/// </summary>
		/// <param name="secondsPerStep">Interval between samples, in seconds or fraction thereof,
		///					e.g. 1 KHz = 1 ms per sample would have secondsPerStep = 0.001.  If
		///					autoOutput is TRUE and the file header has its own sample period value,
		///					then this param is ignored and the value in the file header is used 
		///					instead.</param>
		/// <param name="autoOutput">If TRUE, then we push data out to the Followers AND try to
		///					use the sample period value from the file header; if FALSE, we wait
		///					for whoever to pull out the data.</param>
		public FileReader ( double secondsPerStep, bool autoOutput )
			: base ( secondsPerStep, autoOutput )
		{
		}
//		*/

		/// <summary>
		/// Full constructor:  requires file path and file name.
		/// Also loads header from file.
		/// </summary>
		/// <param name="secondsPerStep">Interval between samples, in seconds or fraction thereof,
		///					e.g. 1 KHz = 1 ms per sample would have secondsPerStep = 0.001.  If
		///					autoOutput is TRUE and the file header has its own sample period value,
		///					then this param is ignored and the value in the file header is used 
		///					instead.</param>
		/// <param name="autoOutput">If TRUE, then we push data out to the Followers AND try to
		///					use the sample period value from the file header; if FALSE, we wait
		///					for whoever to pull out the data.</param>
		/// <param name="filePath">Must be an existing file path, with or without trailing slash</param>
		/// <param name="fileName">Must be an existing file name</param>
		public FileReader( double secondsPerStep, bool autoOutput,
							string filePath, string fileName )
			: base ( secondsPerStep, autoOutput )
		{
			// Save values:
			fPath = FileHandler.cleanPath ( filePath );
			fName = FileHandler.cleanFileName ( fileName );
			fPathName = fPath + fName;

			// Check for existing path:
			FileHandler.validatePath ( fPath );

			// Make sure file does exist:
			FileHandler.fileExists ( fPathName );

			// Try opening the new file for reading:
			FileStream fs = File.OpenRead ( fPathName );
			fs.Close();

			// Some inits:
			// Create the appropriate data structure:
			initValues = new ChainInfo();
			initValues.patientInfo = new PatientInfo("", "", "");
			initValues.dataInfo = new DataInfo();
			initValues.fileInfo = new FileHandler();

			// Read the header from the input file:
			curReadStartPos = 0;
			readHeader();

			// Update the sampling period:
			stepSize = initValues.samplingPeriodSec;
			// Note, we now discard the header data and read it again at Init() time
		}

		#endregion


		#region private methods

		/// <summary>
		/// Initialize the read buffer
		/// </summary>
		private void initBuffer ()
		{
			lock ( this )	// Bad place for multi-thread overlap:
			{
				// Is something there already?
				if ( this.bufferPointer < this.buffSize - 1 )
				{
					// We have previous stored data in the buffer!

					// SORRY, for now that's thrown out.
				}

				if ( this.initValues.samplingPeriodSec < minBufferSampleSec )
				{
					// Sub-second sampling interval -- use bufferSeconds-second buffering:
					//this.doBuffering = true;
					buffSize = (int) Math.Ceiling ( bufferSeconds / initValues.samplingPeriodSec );

					if ( writtenAsAscii )
					{
						// Only need a text buffer - create to size:
						this.textBuffer = new string [ buffSize ];
					}
					else
					{
						// Create binary buffer at starting size:
						this.binaryBuffer = new float [ buffSize ];
						// Note:  we do NOT need a text buffer for the header;
						// that is retrieved all at once and processed immediately.
					}
					// Retrieve contents for the buffer:
					readFileIntoBuffer ( buffSize, 0 );
				}
				else
				{
					// second-plus sampling interval -- don't bother with buffering:
					//this.doBuffering = false;
					this.buffSize = 1;
				}

				this.bufferPointer = 0;
			}
		}


		/// <summary>
		/// Shifts the data, from bufferPointer to the end of the array, to the
		/// beginning of the array.
		/// </summary>
		/// <param name="buffer">Buffer array values</param>
		private void shiftArray ( Array buffer )
		{
			for ( int i = bufferPointer;  i < buffSize; i++ )
			{
				buffer.SetValue ( buffer.GetValue(i), i - bufferPointer );
			}
		}


		private void readFileIntoBuffer ( int size, int offset )
		{
			if ( this.writtenAsAscii )
				readTextFileIntoBuffer ( size, offset );
			else
				readBinaryFileIntoBuffer ( size, offset );
		}

		/// <summary>
		///  Reads UP TO the specified number of values from a text file to the
		///  text buffer.  If the specified number is not available, buffSize 
		///  is updated accordingly.
		/// </summary>
		/// <param name="size">Max number of values to read into the buffer;
		///			cannot be greater than buffSize - offset</param>
		///	<param name="offset">Offset into array to write to (e.g., if we don't
		///			want to wipe out part of the array)</param>
		private void readTextFileIntoBuffer ( int size, int offset )
		{
			//char [] charBuff = new char [ 1 ];	// just for syncing
			//int count;
			string newVal;
			StreamReader fNew;

			if ( offset > this.buffSize )
			{
				throw new ArgumentOutOfRangeException ( "offset", offset,
					"offset cannot be greater than buffSize (" + buffSize + ")." );
			}
			if ( size + offset > this.buffSize )
			{
				throw new ArgumentOutOfRangeException ( "size + offset", size + offset,
					"size + offset cannot be greater than buffSize (" + buffSize + ")." );
			}

			fNew = new StreamReader ( fPathName );
			try
			{
				if ( curReadStartPos > 0 )
				{
					// Scan forward to desired starting point:
					fNew.BaseStream.Seek ( curReadStartPos, System.IO.SeekOrigin.Begin );
				}
				// Read specified number of values into the buffer:
				for ( int i = offset;  i < size + offset;  i++ )
				{
					newVal = fNew.ReadLine();

					if ( newVal == null )
					{	// Out of file -- update things accordingly:
						buffSize = i;
						doneReading = true;
						break;	// stop trying to read
					}
					else
					{	// Valid value -- save it and update position pointer:
						textBuffer[i] = newVal;
						curReadStartPos += newVal.Length + OpenMedICUtils.newLine.Length;
					}
				}
			}
			finally
			{
				fNew.Close();
			}

		}

		/// <summary>
		///  Reads UP TO the specified number of values from a binary file to the
		///  binary buffer.  If the specified number is not available, buffSize 
		///  is updated accordingly.
		/// </summary>
		/// <param name="size">Max number of values to read into the buffer;
		///			cannot be greater than buffSize</param>
		///	<param name="offset">Offset into array to write to (e.g., if we don't
		///			want to wipe out part of the array)</param>
		private void readBinaryFileIntoBuffer ( int size, int offset )
		{
			float newVal;
			System.IO.BinaryReader fNew;

			if ( offset > this.buffSize )
			{
				throw new ArgumentOutOfRangeException ( "offset", offset,
					"offset cannot be greater than buffSize (" + buffSize + ")." );
			}
			if ( size + offset > this.buffSize )
			{
				throw new ArgumentOutOfRangeException ( "size + offset", size + offset,
					"size + offset cannot be greater than buffSize (" + buffSize + ")." );
			}
			fNew = new BinaryReader ( File.OpenRead(fPathName) );
			try
			{
				// Scan forward to desired starting point:
				if ( curReadStartPos > 0 )
				{
					// Scan forward to desired starting point:
					fNew.BaseStream.Seek ( curReadStartPos, System.IO.SeekOrigin.Begin );
				}
				// Read specified number of values into the buffer:
				for ( int i = offset;  i < size + offset;  i++ )
				{
					try 
					{
						newVal = fNew.ReadSingle();
						// No Exception -> Valid value -- save it and update position pointer:
						curReadStartPos += 4;
						this.binaryBuffer[i] = newVal;
					}
					catch ( EndOfStreamException )
					{	// Out of file -- update things accordingly:
						buffSize = i;
						doneReading = true;
						break;		// stop trying to read
					}
				}
			}
			finally
			{
				fNew.Close();
			}

		}


		private void readHeader()
		{
			ChainInfo.tagValuePair val = new ChainInfo.tagValuePair();
			string headerRow;
			StreamReader fNew = new StreamReader(fPathName);
			try
			{
				// Scan forward to desired starting point:
				if (curReadStartPos > 0)
				{
					// Scan forward to desired starting point:
					fNew.BaseStream.Seek(curReadStartPos, System.IO.SeekOrigin.Begin);
				}

				for (int i = 0; i < 10000; i++)	// more than 10K lines of header would be a problem...
				{
					headerRow = fNew.ReadLine();
					this.curReadStartPos += headerRow.Length + OpenMedICUtils.newLine.Length;

					if (headerRow == FileHandler.headerEndDelim)
					{	// End of the header
						// We are done:
						break;
					}
					else if (!headerRow.StartsWith(FileHandler.headerRowLeader))
					{	// NOT  a new header row
						// Hmmm, this is a continuation of the last value!
						val.tagValue += FileHandler.headerRowTrailer + headerRow;
						// Now overwrite what we wrote on the last write:
						setInitValueByTag(initValues, val);
						// If there are even more rows, we'll update this again later...
					}
					else if (!OpenMedICUtils.isEmpty(headerRow))
					{	// NOT empty (must be a new header row)
						val = parseHeaderRow(headerRow);
						// Update initValues -- this is either the "live" object or
						// it's the local-only object, depending on our mode:
						setInitValueByTag(initValues, val);
					}
					// Check for run-away (unterminated) headers:
					if (i > 9998)
					{
						// Bad!
						throw new FileLoadException("The header is not terminated or too big!  "
							+ "Already processed " + i + " rows without finding the Header Terminator row (\""
							+ FileHandler.headerEndDelim + "\")!",
							this.fPathName);
					}
				}
			}
			finally
			{
				fNew.Close();
			}
		}


		/// <summary>
		/// Parses the input row, returning the Tag and Value in a tagValuePair.
		///  Note: this method REQUIRES that the rowLeader and RowTrailer be present
		/// </summary>
		/// <param name="row">String containing a row of data from the file header</param>
		/// <returns>The Tag-Value pair extracted from the row</returns>
		private ChainInfo.tagValuePair parseHeaderRow(string row)
		{
			ChainInfo.tagValuePair val = new ChainInfo.tagValuePair();
			string curRow = row.Substring(FileHandler.headerRowLeader.Length);
			int pos = curRow.IndexOf(FileHandler.headerTagDelim);
			val.tagName = curRow.Substring(0, pos);
			val.tagValue = curRow.Substring(pos + FileHandler.headerTagDelim.Length);

			// Take care of RowTrailer, if necessary:
			if (!FileHandler.headerRowTrailer.StartsWith(OpenMedICUtils.newLine))
			{
				// Must remove everything up to the newLine, if any:
				pos = FileHandler.headerRowTrailer.IndexOf(OpenMedICUtils.newLine);
				if (pos > -1)
				{	// There IS a newLine -- remove everything up to that
				}
				else
					// No newLine -- remove whole rowTrailer:
					pos = FileHandler.headerRowTrailer.Length;

				val.tagValue = val.tagValue.Substring(0, val.tagValue.Length - pos);
			}

			return val;
		}

		private void setInitValueByTag(ChainInfo initSet, ChainInfo.tagValuePair tagVal)
		{
			// Get the tag:
			ChainInfo.varTags tag = ChainInfo.getTagFromValue(tagVal.tagName);
			// Set appropriately -- try ChainInfo first:
			bool matched = initSet.setByTag(tag, tagVal.tagValue);
			if (!matched)	// It was not an ChainInfo, try DataInfo:
				matched = initSet.dataInfo.setByTag(tag, tagVal.tagValue);
			if (!matched)	// Not a DataInfo, try a PatientInfo
				matched = initSet.patientInfo.setByTag(tag, tagVal.tagValue);
			if (!matched)
				matched = initSet.fileInfo.setByTag(tag, tagVal.tagValue);
			if (!matched)
			{
				// None of the above -- ????
			}
		}

		#endregion


		#region overrides

		/// <summary>
		/// Determines the next value in the sequence - in this case, the
		/// next value from the read buffer that must be output.
		/// </summary>
		/// <returns>The next available value</returns>
		protected override float nextVal ()
		{
			float val;

			if ( bufferPointer >= buffSize )
			{
				// We ran out of buffer (possibly not buffering at all):
				if ( doneReading )
				{
					// Ran out of file!
					if ( this.repeat )
					{
						// Start the file from the beginning:
						curReadStartPos = 0;
						readHeader ();
						// Reset flag(s) appropriately:
						doneReading = false;
						initBuffer ();
						// And, continue
					}
					else if ( this.autoSend )
					{	// Non-repeat, AND, we auto-generate values
						// Stop the timer then quit:
						this.Terminate ();
						return 0F;
					}
					else
					{	// Non-repeat, non-auto-generate
						// Return a string of zeros:
						return 0F;
					}
				}
				else
				{	// Normal case:
					this.readFileIntoBuffer ( buffSize, 0 );
					bufferPointer = 0;
				}
			}

			if ( this.writtenAsAscii )
			{
				val = Convert.ToSingle ( textBuffer[bufferPointer] );
			}
			else
			{
				val = binaryBuffer[bufferPointer];
			}
			bufferPointer++;

			// Are we out of data?
			if ((bufferPointer == buffSize) &&		// this buffer is empty
				(doneReading) &&					// the file is empty
				(!repeat))							// we won't repeat the file
			{	// It means we REALLY don't have any more data!  Set the flag:
				dataFinished = true;
			}

			return val;
		}

		/// <summary>
		/// Override default behavior to:
		/// - Save a local copy
		/// - Get file header info
		/// - Update iData accordingly
		/// - Allow using header info to drive the output rates
		/// </summary>
		public override void init ( ChainInfo iData )
		{
			// Save a local handle:
			// - If we're AutoSending, we're the authority on the data, so we can override the
			//		contents of iData.  That means we point to iData and modify that.
			// - If we're not AutoSending, we can tell anyone downstream what the actual data
			//		params are, but we shouldn't mess with the original iData to avoid affecting
			//		the source.
			if ( autoSend )
			{	// Point to the input data structure:
				initValues = iData;
			}
			else
			{	// Make our own data structure:
				initValues = new ChainInfo ();
				initValues.samplingPeriodSec = iData.samplingPeriodSec;
			}

			if ( autoSend && doneReading )	// We terminated the timer earlier - restart it:
			{
				// Init the TimerThread:
				sendTimer = new TimerThread ( 
					new System.Threading.ThreadStart(this.next), 
					false );
			}

			// Init. what needs it:
			curReadStartPos = 0;
			doneReading = false;
			dataFinished = false;	// in case it was set to true previously
			if ( initValues.patientInfo == null )
			{
				initValues.patientInfo = new PatientInfo ( "", "", "" );
			}
			if ( initValues.dataInfo == null )
			{
				initValues.dataInfo = new DataInfo ( );
			}
			if ( initValues.fileInfo == null )
			{
				initValues.fileInfo = new FileHandler ( );
			}

			// Read the header from the input file:
			readHeader ();

			// Init. the sampling period:
			this.stepSize = initValues.samplingPeriodSec;

			// Init output buffer:
			initBuffer ();

			// Apply values and propagate downstream:
			base.init ( iData );	// iData may have been changed or not
		}

		#endregion


		#region public methods

		#endregion


		#region public properties

		/// <summary>
		/// If TRUE, then this reader will start again from the beginning of
		/// the file after it reaches the end;  if FALSE, it will stop 
		/// outputting data (if autoOutput) or output all zeros (if NOT 
		/// autoOutput).
		/// </summary>
		public bool repeatAfterEndOfFile
		{
			get 
			{
				return repeat;
			}
			set
			{
				repeat = value;
			}
		}

		/// <summary>
		/// Return whether the file reading has completed.  Returns FALSE if reading
		/// hasn't started or if reading hasn't reached the end of the file yet, otherwise
		/// returns true.
		/// </summary>
		public bool ReadComplete
		{
			get
			{
				return doneReading;
			}
		}

		#endregion


	}	// END of class
}

