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
	/// Creates a NEW file with the specified path/filename and an appropriate header,
	/// and writes to it the passed input data, one row per sample.
	/// 
	/// The class is hard-coded for:
	/// 
	/// -&gt; Writing the values as ASCII text (more universally readable)
	///	-&gt; Appending creation date and/or time to the name;
	/// 
	/// The class offers options for:
	/// 
	///	-&gt; Limiting the maximum file size (and what to do then);
	///	-&gt; Starting a new file on a new day;
	///	-&gt; [FUTURE ENH - create dir if not existing];
	///	-&gt; [FUTURE ENH - put date in dir name not file name].
	/// 
	/// Default values:
	/// 
	///	-&gt; Limiting the maximum file size:  default = FALSE;
	///	-&gt; Starting a new file on a new day:  default = FALSE;
	///	-&gt; [FUTURE ENH - create dir if not existing:  default = FALSE];
	///	-&gt; [FUTURE ENH - put date in dir name not file name:  default = FALSE].
	/// 
	/// Note the following required data to be passed to the init(ChainInfo) method:
	/// 
	/// -&gt; A file name prefix (goes before the date/time stamp and extension)
	///	
	/// </summary>
	public class FileLogger : FileWriter
	{
		/// <summary>
		/// Constructor:  only requires file path and root of file name.
		/// -&gt; Writing the values as ASCII text:  TRUE;
		///	-&gt; Limiting the maximum file size:  FALSE;
		///	-&gt; Appending creation date and/or time:  TRUE;
		///	-&gt; Starting a new file on a new day:  FALSE;
		///	-&gt; Replacing or appending to an existing file:  FALSE;
		///	-&gt; [FUTURE ENH - create dir if not existing:  FALSE];
		///	-&gt; [FUTURE ENH - put date in dir name not file name:  FALSE].
		/// </summary>
		/// <param name="filePath">Must be a valid, writeable path</param>
		/// <param name="fileName">Must be a valid file name that does NOT already exist in filePath</param>
		public FileLogger(string filePath, string fileName)
			: base( filePath,			// Path
					fileName + ".txt",	// Name root
					true,				// writeValuesAsAscii?
					false,				// createNewFileDaily?
					true,				// overwriteExistingFile?
					false,				// appendExistingFile?
					true,				// appendDateToFileName?
					true,				// appendTimeToFileName?
					0,					// maxAllowedFileSize
					false)				// createNewFileWhenMaxSize?
		{
			// That's it for now.
		}

	}
}
