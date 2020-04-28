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
	/// The FIRFilter is a Finite Impulse Response filter.
    /// It doesn't have risk of instability of perpetual propagation of impulses.
    /// This initial version expects the order and coefficients to be passed in,
    /// thereby forcing the instantiator to calculate coefficients -- but the coefficients
    /// will include all parameters for the filter.
    /// The filter will implement a delay equal to the filter order minus one, to avoid
    /// start-up erratic behavior.
	/// </summary>
	public class FIRFilter:Filter
	{
        /// <summary>
        /// Stores the filter order
        /// </summary>
        protected int order;
        /// <summary>
        /// Stores the array of FIR coefficients
        /// </summary>
        protected double[] coeffs;
		/// <summary>
		/// Indicates whether we apply the filter or bypass it
		/// </summary>
		protected bool bypass = false;

        /// <summary>
        /// The only input is the coefficients array, from which we deduce the order and
        /// the delay.
        /// </summary>
        /// <param name="coefficients">Array of coefficients, where the first element of
        ///     the array is the one applied to the current value, and the last element is
        ///     applied to the oldest value.</param>
        public FIRFilter (ref double[] coefficients) 
            : base ( coefficients.Length - 1 )
{
            order = coefficients.Length;
            coeffs = coefficients;
            for (int i = 0; i < order - 1; i++)
            {
                delayBuffer.AddPoint ( new Sample() );
            }
		}

        private float calcValue(Sample newValue)
        {
            // We ALWAYS want the first coefficient applied to the new value:
            float outVal = (float)((double)newValue.sampleValue * coeffs[0]);

            if (this.filterDelay == 0)
            {
                // Trivial case -- what came in goes out!                
            }
            else
            {
                lock (delayBuffer)
                {
                    for (int samp = 1; samp < order; samp++)
                    {
                        outVal += (float) ( (double)delayBuffer.getPoint(samp-1).sampleValue
                                                            * coeffs[samp] );
                    }
                }
            }
            return outVal;
        }

		/// <summary>
		/// Output a sample delayed by our internal delay, then add the new sample 
		/// to the delay circular buffer.
		/// </summary>
		/// <param name="newValue">New input value</param>
		public new void addValue ( Sample newValue )
		{
			if (bypass)
			{
				this.sendValue(newValue);
			}
			else
			{
				Sample outValue = new Sample(calcValue(newValue));
				delayBuffer.AddPoint(newValue);

				this.sendValue(outValue);
			}
		}

		/// <summary>
		/// Generic AddValues:  propagates the value to all followers.  Note that it will 
		/// lock the delay buffer to avoid data corruption.
		/// </summary>
		/// <param name="newValues">Zero-based array of samples.
		///                         Note that the OLDEST sample is newValues[0]</param>
		public override void addValues ( Samples newValues )
		{
			if (bypass)
			{
				if (newValues.size > 0)
					this.sendValues(newValues);
			}
			else
			{
				Samples outValues = new Samples(newValues.size);

				lock (delayBuffer)
				{
					for (int i = 0; i < newValues.size; i++)
					{
						outValues[i].sampleValue = calcValue(newValues[i]);
						delayBuffer.AddPoint(newValues[i]);
					}
				}

				if (outValues.size > 0)
					this.sendValues(outValues);
			}
		}


		/// <summary>
		/// Allows bypassing the filtering, i.e. making this object behave 
		/// like a straight pass-through filter instead of a FIR filter.
		/// </summary>
		public bool Bypass
		{
			get { return bypass; }
			set { bypass = value; }
		}


	}	// END of class
}
