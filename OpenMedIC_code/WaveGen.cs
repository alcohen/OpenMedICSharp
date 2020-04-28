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
	/// Summary description for WaveGen.
	/// </summary>
	public abstract class WaveGen: FunctionGen
	{
        /// <summary>
        /// Frequency of the generated waveform, in hertz
        /// </summary>
        protected double waveFrequency;

		/// <summary>
		/// Creates new instance of a wavefrorm generator with the specified characteristics.
		/// </summary>
		/// <param name="secondsPerStep">Interval between samples, in seconds or fraction thereof,
		///					e.g. 1 KHz = 1 ms per sample would have secondsPerStep = 0.001</param>
		/// <param name="autoOutput">If true, then we push data out to the Followers;
		///					if false, we wait for whoever to pull out the data</param>
		/// <param name="waveformFrequency">Frequency, in Hertz, of the generated waveform.
		///					A value that would correspond to less than 2 samples per cycle
		///					will throw an exception.</param>
		public WaveGen( double secondsPerStep, bool autoOutput, double waveformFrequency )
			: base ( secondsPerStep, autoOutput )
		{
			if ( secondsPerStep * 2 > ( 1 / waveformFrequency ) ) 
			{
				throw new ArgumentException ( "The step size is too big for the specified frequency;  "
					+ "the values must be such that there will be at least 2 steps per cycle." );
			}

			waveFrequency = waveformFrequency;
		}

		/// <summary>
		/// Function frequency, in hertz (cycles per second).
		/// A 1-KHz function would have a frequency of 1000.
		/// </summary>
		public double frequency
		{
			set
			{
				waveFrequency = value;
			}
			get
			{
				return waveFrequency;
			}
		}

		/// <summary>
		/// Determines which cycle quarter we are in based on which step we are in.
		/// A quarter is one of:  1, 2, 3, or 4 (1 = 0-90deg; 2 = 90-180; 3 = 180-270;
		/// 4 = 270-360).
		/// </summary>
		/// <param name="step"></param>
		/// <returns></returns>
		protected double getAngle ( long step )
		{
			double angle = 4.0 * (double)(step) * this.frequency * stepPeriod ;
			angle = angle % 4.0;

			return angle;
		}

	}	// END of class
}
