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
using System.Collections;

namespace OpenMedIC
{
	/// <summary>
	/// This class implements file-generic functionality.
	/// </summary>
	public class FileHandler
	{

		#region Instance-specific functionality

		// File data params (retrieved from the header):
		private string			iDataFormat;
		private string			iWriterClassName;
		private string			iMultiFileNum;
		private string			iStartTime;

		/// <summary>
		/// Creates a new identical copy of this object.  The returned clone is completely
		/// independent of this object, and the two can be changed/updated/deleted without
		/// one affecting the other.
		/// </summary>
		/// <returns>An identical copy of this object</returns>
		public FileHandler clone ()
		{
			FileHandler newHdlr = new FileHandler ();

			newHdlr.iDataFormat = iDataFormat;
			newHdlr.iWriterClassName = iWriterClassName;
			newHdlr.iMultiFileNum = iMultiFileNum;
			newHdlr.iStartTime = iStartTime;

			return newHdlr;
		}

		/// <summary>
		/// Data Formatting information (as text)
		/// </summary>
		public string dataFormat
		{
			get
			{
				return iDataFormat;
			}
			set
			{
				iDataFormat = value;
			}
		}

		/// <summary>
		/// Name of the class used to actually write the data
		/// </summary>
		public string writerClassName
		{
			get
			{
				return iWriterClassName;
			}
			set
			{
				iWriterClassName = value;
			}
		}

		/// <summary>
		/// Current file number in a multi-file data set
		/// </summary>
		public string multiFileNum
		{
			get
			{
				return iMultiFileNum;
			}
			set
			{
				iMultiFileNum = value;
			}
		}

		/// <summary>
		/// Data acquisition start time
		/// </summary>
		public string startTime
		{
			get
			{
				return iStartTime;
			}
			set
			{
				iStartTime = value;
			}
		}

		/// <summary>
		/// Assign a property referencing it by the tag associated with it.
		/// </summary>
		/// <param name="tag">A varTags value</param>
		/// <param name="val">Value to assign</param>
		/// <returns>TRUE if the assignment was successful, FALSE otherwise.</returns>
		public bool setByTag ( ChainInfo.varTags tag, string val )
		{
			switch ( tag )
			{
				case ChainInfo.varTags.dataFormat:
					iDataFormat = val;
					return true;
				case ChainInfo.varTags.writerClassName:
					iWriterClassName = val;
					return true;
				case ChainInfo.varTags.multiFileNum:
					iMultiFileNum = val;
					return true;
				case ChainInfo.varTags.startTime:
					iStartTime = val;
					return true;
			}
			// We have no properties to set!
			return false;
		}

		#endregion

		#region Static section


		// Some public constants that we want to expose:
		/// <summary>
		/// Sequence of characters that indicate the start of a header.  When parsing,
		/// this should be looked for then thrown out.  If empty (""), then there is no
		/// start-of-header delimiter and none should be expected.
		/// </summary>
		public	const	string	headerStartDelim	= "";
		/// <summary>
		/// Sequence of characters that begins a header row.  The first character after
		/// the headerRowLeader sequence is the beginning of a header Tag.
		/// </summary>
		public	const	string	headerRowLeader		= "# ";
		/// <summary>
		/// Sequence of characters that separate a tag from its value.  It cannot be
		/// empty, and it must be a sequence that will never appear in a tag.
		/// </summary>
		public	const	string	headerTagDelim		= ":  ";
		/// <summary>
		/// Sequence of characters that mark the end of a header row.
		/// Note that this CAN be empty and/or appear in a header value;  when parsing,
		/// you must look for either ( headerRowTrailer + headerRowLeader ) or
		/// ( headerRowTrailer + headerEndDelim ) as the end of the value.
        /// The value of this constant is RECOMMENDED TO BE an OpenMedICUtils.newLine,
		/// so the file can be read using the ReadLine(...) method(s).
		/// </summary>
		public	const	string	headerRowTrailer	= OpenMedICUtils.newLine;
		/// <summary>
		/// Sequence of characters that mark the end of a header (and start of 
		/// data).  Note that, in all cases, the headerEndDelim will be followed 
		/// by an asciiValueDelim before the actual data starts.
		/// </summary>
		public	const	string	headerEndDelim		= "##";
		/// <summary>
		/// Sequence of characters that delimit values in an ASCII-value-formatted file,
		/// i.e. a file where the values are written as text rather than as binary 
		/// numbers.  This cannot be empty, and cannot start or end with one of these
		/// characters:  [0123456789+-.E]
        /// The value of this constant is RECOMMENDED TO BE an OpenMedICUtils.newLine,
		/// so the file can be read using the ReadLine(...) method(s).
		/// </summary>
		public	const	string	asciiValueDelim		= headerRowTrailer;


		/// <summary>
		/// Determines whether filePath is a valid, existing path on the current system.
		/// </summary>
		/// <param name="filePath">directory path</param>
		public static void validatePath ( string filePath )
		{
			if ( ! System.IO.Directory.Exists ( filePath ) )
			{
				throw new ArgumentException ( "filePath (" + filePath + ") must point to an existing path on this system.",
					filePath );
			}
		}

		/// <summary>
		/// Determines whether the specified path/file exists on the current system
		/// </summary>
		/// <param name="filePathAndName">file name and location</param>
		public static void fileExists ( string filePathAndName )
		{
			// Make sure file (and its permutations?) do not exist:
			string errText = "filePath/fileName (" + filePathAndName 
										+ ") must be an existing file.";

			if ( ! File.Exists ( filePathAndName ) )
			{
				throw new ArgumentException ( errText, filePathAndName );
			}
		}

		/// <summary>
		/// Checks that the specified path/name, with or without certain used variants, 
		/// do(es) not already exist.
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="fileName"></param>
		/// <param name="withVariants"></param>
		public static void fileNotExists ( string filePath, string fileName, bool withVariants )
		{			
			// Make sure file (and its permutations?) do not exist:
			string errText;
			string tmpFileName = cleanFileName(fileName);
			if ( withVariants )
			{
				// Look for variants of the name, as well:
				string tmpExt = getExt(tmpFileName);
				tmpFileName = tmpFileName.Substring (0, tmpFileName.Length - tmpExt.Length - 1)
					+ "*." + tmpExt;
				errText = "fileName (" + fileName + ") must be a non-existing file name in the given path " 
					+ "(including any of its permutations of the form \"nameroot*.ext\").";
			}
			else
			{
				// Regular error msg:
				errText = "fileName (" + fileName + ") must be a non-existing file name in the given path.";
			}
			if ( File.Exists ( cleanPath(filePath) + tmpFileName ) )
			{
				throw new ArgumentException ( errText, fileName );
			}
		}

		/// <summary>
		/// Cleans up the passed path.
		/// For now it removes leading/trailing whitespace, and makes sure that
		/// the path ends with a delimiter.
		/// </summary>
		/// <param name="inPath">File Path to clean</param>
		/// <returns>cleaned file path</returns>
		public static string cleanPath ( string inPath )
		{
			string path = inPath.Trim ();
			if ( path.EndsWith ( @"\" ) || path.EndsWith ( @"/" ) )
			{
				// OK, already delimited at the end
			}
			else	// Add an appropriate delimiter:
			{
				if ( path.IndexOf (@"/") > -1 )
				{
					// Fore-slash-delimited:
					path += @"/";
				}
				else if ( path.IndexOf (@"\") > -1 )
				{
					// Back-slash-delimited:
					path += @"\";
				}
				else
				{
					// Unknown - go with fore-slash (more universal):
					path += @"/";
				}
			}
			// OK:
			return path;
		}

		/// <summary>
		/// Removes unwanted stuff from the passed file name.
		/// For now it just removes leading and trailing whitespace.
		/// </summary>
		/// <param name="fileName">file name to be cleaned</param>
		/// <returns>Cleaned file name</returns>
		public static string cleanFileName ( string fileName )
		{
			return fileName.Trim ();
		}

		/// <summary>
		/// Extracts the extension from the file name;  if there is none, it
		/// returns an empty string ("").
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public static string getExt ( string fileName )
		{
			int pos = fileName.LastIndexOf ( "." );

			if ( pos == -1 )
			{
				// No extension:
				return "";
			}
			else
			{
				// Get what follows the last period:
				string ext = fileName.Substring ( pos + 1 );
				return ext;
			}
		}

		/// <summary>
		/// Returns the tag that the given value corresponds to, or throws an
		/// exception if there is no match.
		/// Note that the match must be exact and case-sensitive!
		/// </summary>
		/// <param name="tagValue">Tag Value to match to varTags tags</param>
		/// <returns>The appropriate tag;  throws an exception if no match.
		///		Note that the match must be exact and case-sensitive!</returns>
		public static ChainInfo.varTags getTagFromLabel ( string tagValue )
		{
			return ChainInfo.getTagFromValue ( tagValue );
		}


		/// <summary>
		/// Parses pathAndFileName to extract the leading path component, including
		/// a trailing delimiter.
		/// If the passed value doesn't contain a path, then it returns an empty string.
		/// </summary>
		/// <param name="pathAndFileName">A path and file name string with the appropriate
		///			delimiters for the current OS</param>
		/// <returns>The path component, including a trailing delimiter</returns>
		public static string getPath(string pathAndFileName)
		{
			string param = pathAndFileName.Trim();
			char dirDelim;
			if (param.IndexOf(Path.DirectorySeparatorChar) < 0)
			{	// No instances of DirectorySeparatorChar;  try the alt char.:
				if (param.IndexOf(Path.AltDirectorySeparatorChar) < 0)
				{	// No directory at all:
					return "";
				}
				else
				{	// using alt separator:
					dirDelim = Path.AltDirectorySeparatorChar;
				}
			}
			else
			{	// Using std. separator:
				dirDelim = Path.DirectorySeparatorChar;
			}
			// Now extract actual path:
			int lastDelim = param.LastIndexOf(dirDelim);
			string path = param.Substring(0, lastDelim + 1);	// +1 is to include the delimiter
			return path;
		}


		/// <summary>
		/// Parses pathAndFileName to extract the trailing filename component, NOT including
		/// a leading delimiter.
		/// If the passed value doesn't contain a file name, then it returns an empty string.
		/// </summary>
		/// <param name="pathAndFileName">A path and file name string with the appropriate
		///			delimiters for the current OS</param>
		/// <returns>The file name component, NOT including a leading delimiter</returns>
		public static string getFileName(string pathAndFileName)
		{
			string param = pathAndFileName.Trim();
			char dirDelim;
			if (param.IndexOf(Path.DirectorySeparatorChar) < 0)
			{	// No instances of DirectorySeparatorChar;  try the alt char.:
				if (param.IndexOf(Path.AltDirectorySeparatorChar) < 0)
				{	// No directory separator at all (and thus no directory):
					return param;
				}
				else
				{	// using alt separator:
					dirDelim = Path.AltDirectorySeparatorChar;
				}
			}
			else
			{	// Using std. separator:
				dirDelim = Path.DirectorySeparatorChar;
			}
			// Now extract actual path:
			int lastDelim = param.LastIndexOf(dirDelim);
			string name = param.Substring(lastDelim + 1);	// +1 is to leave out the delimiter
			return name;
		}

		#endregion


	}	// END of class

}
