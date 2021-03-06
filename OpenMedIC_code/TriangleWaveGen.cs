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
	/// Generates a triangle wave, of the desired frequency and step size.
	/// 
	/// The frequency is in hertz (cycles per second);  the step size in seconds.
	/// 
	/// If there are less than 2 steps per cycle the constructor will fail.
	/// </summary>
	public class TriangleWaveGen:WaveGen
	{
		/// <summary>
		/// Creates new instance of a triangle wave generator with the specified characteristics.
		/// </summary>
		/// <param name="secondsPerStep">Interval between samples, in seconds or fraction thereof,
		///					e.g. 1 KHz = 1 ms per sample would have secondsPerStep = 0.001</param>
		/// <param name="autoOutput">If true, then we push data out to the Followers;
		///					if false, we wait for whoever to pull out the data</param>
		/// <param name="triangleWaveFrequency">Frequency, in Hertz, of the generated triangle wave.
		///					A value that would correspond to less than 2 samples per cycle
		///					will throw an exception.</param>
		public TriangleWaveGen( double secondsPerStep, bool autoOutput, double triangleWaveFrequency )
					: base ( secondsPerStep, autoOutput, triangleWaveFrequency )
		{
		}

		/// <summary>
		/// Generate the next triangle wave value.
		/// The current "angle" is:  ( stepPeriod / (1/frequency)=period ) * (2 pi) * (lastStep + 1)
		/// This is equal to:  ( 2 * PI * (lastStep+1) ) * (frequency * stepPeriod)
		/// The return amplitude is based on (angle / (PI/2)), i.e. which cycle quarter we're in
		///		= ( (2 PI * (lastStep+1) * freq * stepPeriod) / (0.5 PI) ) modulo 4
		///		= ( 4 * (lastStep+1) ) * (freq * stepPeriod) modulo 4
		/// -&gt; If (angle / PI/2) .gt.= 0 and (angle / PI/2) .lt. 1:  output = angle;
		///	-&gt; If (angle / PI/2) .gt.= 1 and (angle / PI/2) .lt. 3:  output = 2 - angle;
		///	-&gt; If (angle / PI/2) .gt.= 3 and (angle / PI/2) .lt. 4:  output = -4 + angle.
		/// </summary>
		/// <returns>Next appropriate value</returns>
		protected override float nextVal ()
		{
/*
			double angle = 4.0 * (double)(++this.lastStep) * this.frequency * this.stepPeriod ;
			angle = angle % 4.0;
			int quartile = (int) Math.Floor ( angle );
 */
			double angle = this.getAngle(++lastStep);
			int quartile = (int) Math.Floor ( angle );
			double val;

			switch ( quartile )
			{
				case 0:		// first quarter
					val = angle;
					break;
				case 1:		// second quarter
				case 2:		// third quarter
					val = 2 - angle;
					break;
				default:	// fourth quarter
					val = -4 + angle;
					break;
			}
			return (float) ( val * this.scale );
		}


	}	//  END OF class
}
