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
	/// The PassThroughFilter is the simplest filter:  it passes inputs straight
	/// through, without altering the value and without delay.
	/// </summary>
	public class PassThroughFilter:Filter
	{
        /// <summary>
        /// Constructor;  functionally it's a pass-through to the base class'
        /// (the Filter class) constructor.
        /// </summary>
        /// <param name="delayBins">Delay (in number of samples) between filter input 
        ///     and output.</param>
		public PassThroughFilter (int delayBins) : base ( delayBins )
		{
		}

		/// <summary>
		/// Output a sample delayed by our internal delay, then add the new sample 
		/// to the delay circular buffer.
		/// </summary>
		/// <param name="newValue">New input value</param>
		public new void addValue ( Sample newValue )
		{
			Sample outValue;

			if ( this.filterDelay == 0 )
			{
				// Trivial case -- what came in goes out:
				outValue = newValue;
			}
			else
			{
				lock ( delayBuffer )
				{
					outValue = delayBuffer.getPoint ( filterDelay );
					delayBuffer.AddPoint ( newValue );
				}
			}

			this.sendValue ( outValue );
		}

		/// <summary>
		/// Generic AddValues:  propagates the value to all followers.  Note that it will 
		/// lock the delay buffer to avoid data corruption.
		/// </summary>
		/// <param name="newValues">Zero-based array of samples.
		///                         Note that the OLDEST sample is newValues[0]</param>
		public override void addValues ( Samples newValues )
		{
			// This gets complicated by the fact, that the input may contain more samples than
			// we store in our delayBuffer.  If this is the case, we need to create a new
			// output Samples to store the desired output, then apply the desired subset of the
			// input to the delayBuffer.
			long startIndex;
			Samples outValues;
			if ( this.filterDelay == 0 )
			{
				// Trivial case -- what came in goes out:
				outValues = newValues;
			}
			else
			{
				lock ( delayBuffer )
				{
					// Get the start index into the delay buffer:
					startIndex = delayBuffer.CurrInputIndex - this.filterDelay + 1;
					if ( startIndex >= 0 )	
					{
						outValues = new Samples ( newValues.size );
					}
					else	// Not enough samples in the queue yet
					{
						// Sanitize startIndex and outValues size:
						startIndex = 0;
						// output size is the new total buffer content (currInputIndex + new size)
						// minus the delay (note - may be zero or greater than the delay buffer size):
						int newSize = (int)Math.Max ( 0, 
							delayBuffer.CurrInputIndex 
							+ newValues.size 
							- filterDelay
							+ 1 );
						outValues = new Samples ( newSize );
					}

					if ( newValues.size <= this.filterDelay ) 
					{
						// Simple case -- just get the desired values from our buffer,
						// then update the delay buffer from the input:
						delayBuffer.GetPointsSince ( startIndex, outValues.size, outValues );
						delayBuffer.AddPoints ( newValues );
					}
					else
					{
						// Unpleasant case -- must do complex array compositions:
						// 0.  Smallest-possible intermediate buffer:
						Samples xfer = new Samples ( Math.Max (newValues.size - filterDelay, filterDelay) );

						// 1.  Take the WHOLE (available) delay, and put it at the start of the output:
						delayBuffer.GetPointsSince ( startIndex, xfer );	// resizes xfer if needed
						if ( xfer.size > 0 )
							outValues.setRange ( xfer, 0, xfer.size - 1 ); 

						// 2.  Take the input from the start to (end-filterDelay-1), and put at the end of the output:
						xfer.size = newValues.size - filterDelay;
						newValues.getRange ( xfer, 0, newValues.size - filterDelay - 1 );
						outValues.setRange ( xfer, outValues.size - xfer.size, outValues.size - 1 );

						// 3.  Take the input from (end-filterDelay) to the end, and put it in the delay:
						xfer.size = filterDelay;
						newValues.getRange ( xfer, newValues.size - filterDelay, newValues.size - 1 );
						delayBuffer.AddPoints ( xfer );
					}
				}
			}

			if ( outValues.size > 0 )
				this.sendValues ( outValues );
		}


	}	// END of class
}
