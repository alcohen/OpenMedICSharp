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
	/// Summary description for Sample.
	/// </summary>
	public class Sample
	{
		private float myValue;

        /// <summary>
        /// Constructor;  initializes the value to 0
        /// </summary>
		public Sample()
		{
			myValue = 0;
		}

        /// <summary>
        /// Constructor;  initializes the value to the passed param.
        /// </summary>
        /// <param name="sampleValue">Initial value for new Sample</param>
		public Sample( float sampleValue )
		{
			myValue = sampleValue;
		}

        /// <summary>
        /// Get or set the Sample's value
        /// </summary>
		public float sampleValue
		{
			set
			{
				myValue = value;
			}
			get
			{
				return myValue;
			}
		}

        /// <summary>
        /// Set this Sample's value to be the same as the passed Sample's value
        /// </summary>
        /// <param name="newVal">Sample whose value to copy</param>
		public void copyFrom ( Sample newVal )
		{
			this.myValue = newVal.sampleValue;
		}

        /// <summary>
        /// Sets the passed Sample's value to be the same as this Sample's value
        /// </summary>
        /// <param name="newVal">Sample whose value to update</param>
		public void copyTo ( Sample newVal )
		{
			newVal.sampleValue = this.myValue;
		}

        /// <summary>
        /// Returns the Sample's current value as a string.
        /// </summary>
        /// <returns>The Sample's current value as a string</returns>
		public override string ToString ()
		{
			return this.myValue.ToString ();
		}

        /// <summary>
        /// Returns the Sample's current value as a string, rounded to the specified number
        /// of significant digits
        /// </summary>
        /// <param name="significantDigits">Max. number of significant digits in the returned 
        ///     string</param>
        /// <returns>The Sample's current value as a string, rounded to the specified number
        ///     of significant digits</returns>
		public string ToString ( int significantDigits)
		{
			return Math.Round (myValue, significantDigits).ToString ();
		}

	}	// END of class
}
