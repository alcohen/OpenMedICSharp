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

namespace OpenMedIC
{
	/// <summary>
	/// Stores general-purpose utility methods and constants.
	/// </summary>
	public class OpenMedICUtils
	{
        /// <summary>
        /// Flag used to enable special functionality for debugging.  See code for details.
        /// </summary>
		public static bool debugMode = false;
        /// <summary>
        /// Debugging-mode flag to enable stdout printing of debug statements.  See code for details.
        /// </summary>
		public static bool doPrint = false;

        /// <summary>
        /// Definition of new-line character sequence, used in writing to files 
        /// or to screen.  The current value is the DOS/Windows/Notepad new-line
        /// sequence, CR-LF.
        /// </summary>
		public const string newLine = "\r\n";

        /// <summary>
        /// Determines whether a string is null or empty, and returns true if it's either.
        /// </summary>
        /// <param name="val">string to be tested</param>
        /// <returns>True if the string is null OR empty;  false if it's neither.</returns>
		public static bool isEmpty (string val)
		{
			return ( val == null || val.Length == 0 );
		}

		/// <summary>
        /// Conditional debug print statement:  if OpenMedICUtils.debugMode then it
		/// prints, otherwise it's a no-op
		/// </summary>
		/// <param name="msg">text to be displayed</param>
		public static void debugPrint ( string msg )
		{
			if ( debugMode && doPrint )
			{
				System.Diagnostics.Debug.WriteLine ( msg );
			}
		}

    }	// END of class OpenMedICUtils
}
