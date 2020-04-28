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
	/// Summary description for Filter.
	/// 
	/// The Filter class exists for the purpose of defining a common structure
    /// for all OpenMedIC filters.
	/// 
	/// As the filters become more complex, this class can evolve to encompass
	/// all the new needed functionality.
	/// </summary>
	public abstract class Filter:BuildingBlock
	{
		/// <summary>
		/// Delay, in number of bins, between input and output of values in this
		/// filter.
		/// </summary>
		private int delay;

		/// <summary>
		/// Buffer used to store values for the delay
		/// </summary>
		protected CircularBuffer delayBuffer;

		/// <summary>
		/// Allowed types of filter
		/// </summary>
		public enum filterTypes { 
			/// <summary>
			/// Infinite Impulse Response filter
			/// </summary>
			IIR, 
			/// <summary>
			/// Finite Impulse Response filter
			/// </summary>
			FIR, 
			/// <summary>
			/// Any other type, TBD (should not be used;  rather, new types should be added)
			/// </summary>
			Other };

		/// <summary>
		/// Initializes new Filter object
		/// </summary>
		/// <param name="delayBins">Delay (in number of samples) between filter input and output.</param>
		public Filter (int delayBins)
		{
			if ( delayBins < 0 || delayBins > 1024 * 1024 * 1024 )
			{
				throw new ArgumentOutOfRangeException ( "delayBins must be between 0 and 1024 * 1024 * 1024; passed value is " + delayBins );
			}
			delay = delayBins;
			if ( delayBins > 0 )
				// Allocate what we need:
				delayBuffer = new CircularBuffer ( delayBins );
			else
				// Allocate a minimum to have a working object (even if not used):
				delayBuffer = new CircularBuffer(1);
		}

		/// <summary>
		/// Delay, in number of samples, between input and output of values in this
		/// filter.
		/// </summary>
		public int filterDelay
		{
			get
			{
				return delay;
			}
		}

	}  // END OF class Filter

} // END OF file
