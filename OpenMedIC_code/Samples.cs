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
	/// This is a variable-size array of Sample objects.  Its purpose is to initialize
	/// to a fixed max. size, then have its Sample objs. be updated without re-instantiating
	/// them each time.
	/// 
	/// The "current size" of the array is a window of the desired size into a subset 
	/// of the internal array;  again, resizing does not require re-instantiation.
	/// </summary>
	public class Samples
	{
		private Sample [] sampArray;
		private int myMaxSize;
		private int curSize;

		/// <summary>
		/// Constructor.  Note that size is also initialized to maxSize.
		/// </summary>
		/// <param name="maxSize">maximum number of samples that this Samples object can hold.  
		///							Must be between 0 and one giga (1024 * 1024 * 1024).</param>
		public Samples( int maxSize )
		{
			if ( maxSize > (1024 * 1024 * 1024) 
			  || maxSize < 0 )
			{
				throw new ArgumentOutOfRangeException ( "maxSize must be between 0 and 1024 * 1024 * 1024; passed value is " + maxSize );
			}
			myMaxSize = maxSize;
			sampArray = new Sample [maxSize];
			curSize = maxSize;	// reasonable default
			// Initialize:
			for ( int i = 0; i < maxSize; i++ )
				sampArray [i] = new Sample ();
		}

		/// <summary>
		/// The maximum allowed size for the array, as determined at initialization time.
		/// </summary>
		public int maxSize
		{
			get
			{
				return myMaxSize;
			}
		}

		/// <summary>
		/// The current valid-data size of the array.  This value is to be set by the code
		/// that populates the array, and retrieved by the code that reads the array.
		/// 
		/// size cannot be less than 0 or greater than maxSize;  attempting to set it 
		/// to such a value will throw an ArgumentOutOfRangeException.
		/// </summary>
		public int size
		{
			get
			{
				return curSize;
			}
			set
			{
				if ( value > myMaxSize )
				{
					throw new ArgumentOutOfRangeException ( "size cannot be greater than maxSize (currently " + myMaxSize + ") - size passed = " + value );
				}
				if ( value < 0 )
				{
					throw new ArgumentOutOfRangeException ( "size cannot be less than 0 - size passed = " + value );
				}

				curSize = value;
			}

		}

		/// <summary>
		/// Used to retrieve an individual Sample for setting or retrieving its value(s).
		/// 
		/// Note that this method does not know or care about how values are read from or
		/// written into a Sample.
		/// </summary>
		public Sample this [ int pos ]
		{
			get
			{
				if ( pos >= curSize )
				{
					throw new ArgumentOutOfRangeException ( "maximum index allowed is (size - 1) (currently " + curSize + ") - index passed = " + pos );
				}
				if ( pos < 0 )
				{
					throw new ArgumentOutOfRangeException ( "minimum index allowed is 0 - index passed = " + pos );
				}
				return sampArray [ pos ];
			}
		}

		/// <summary>
		/// Returns the range of values starting at index "from" through index "to", inclusive.
		/// </summary>
		/// <param name="range">where to write the desired range</param>
		/// <param name="from">index (zero-based) of first returned value</param>
		/// <param name="to">index (zero-based) of last returned value</param>
		public void getRange ( Samples range, int from, int to )
		{
			if ( from < 0 || from > size )
			{
				throw new ArgumentOutOfRangeException ( "from", from, 
					"Parameter 'from' (" + from + ") is out of range -- acceptable values are between 0 and current size (" 
					+ size + ")" );
			}
			if ( to < 0 || to > size )
			{
				throw new ArgumentOutOfRangeException ( "to", to, 
					"Parameter 'to' (" + to + ") is out of range -- acceptable values are between 0 and current size (" 
					+ size + ")" );
			}
			if ( from > to )
			{
				throw new ArgumentOutOfRangeException ( "to", to, 
					"Parameter 'to' (" + to + ") must be equal to or greater than 'from' parameter (" + from + ")" );
			}

			range.size = to - from + 1;
			for ( int pos = from; pos <= to; pos++ )
			{
				range[pos - from].copyFrom ( this[pos] );
			}

		}

		/// <summary>
		/// Sets the range of values starting at index "from" through index "to", inclusive,
		/// to the values contained in the input range.
		/// </summary>
		/// <param name="range">data to apply to the desired range</param>
		/// <param name="from">index (zero-based) of first returned value</param>
		/// <param name="to">index (zero-based) of last returned value</param>
		public void setRange ( Samples range, int from, int to )
		{
			if ( from < 0 || from > size )
			{
				throw new ArgumentOutOfRangeException ( "from", from, 
					"Parameter 'from' (" + from + ") is out of range -- acceptable values are between 0 and current size (" 
					+ size + ")" );
			}
			if ( to < 0 || to > size )
			{
				throw new ArgumentOutOfRangeException ( "to", to, 
					"Parameter 'to' (" + to + ") is out of range -- acceptable values are between 0 and current size (" 
					+ size + ")" );
			}
			if ( from > to )
			{
				throw new ArgumentOutOfRangeException ( "Parameter 'to' (" + to 
					+ ") must be equal to or greater than parameter 'from' (" + from + ")" );
			}
			if ( range.size < to - from + 1 )
			{
				throw new ArgumentOutOfRangeException ( "The input range's size (" + range.size
					+ ") must be equal to or greater than the difference between the 'to' parameter (" + to 
					+ ") and the 'from' parameter (" + from + ")" );
			}

			for ( int pos = from; pos <= to; pos++ )
			{
				this[pos].copyFrom ( range[pos - from] );
			}

		}

	}	// END of class
}
