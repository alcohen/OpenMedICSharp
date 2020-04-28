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
	/// Generic function generator class.
	/// </summary>
	public abstract class FunctionGen:DataSource
	{

		/// <summary>
		/// This is a multiplier for output amplitude;  note that, depending on the
		/// function, the actual output amplitude may have positive or negative values
		/// greater than this.
		/// </summary>
		protected float scale = 1;

		/// <param name="secondsPerStep">Interval between samples, in seconds or fraction thereof,
		///					e.g. 1 KHz = 1 ms per sample would have secondsPerStep = 0.001</param>
		/// <param name="autoOutput">If true, then we push data out to the Followers;
		///					if false, we wait for whoever to pull out the data</param>
		public FunctionGen ( double secondsPerStep, bool autoOutput ) 
				: base ( secondsPerStep, autoOutput )
		{
		}

		/// <summary>
		/// Initialize the object;  if this is .
		/// </summary>
		/// <param name="iData">Passed up only</param>
		public override void init ( ChainInfo iData )
		{
			iData.samplingPeriodSec = stepSize;		// Init. the sampling period
			base.init ( iData );
		}

		/// <summary>
		/// A multiplying factor for the function output.  
		/// Can be negative, but cannot be zero!
		/// </summary>
		public float outputMultiplier
		{
			get 
			{
				return this.scale;
			}
			set
			{
				if ( value == 0 )
				{
					throw new ArgumentException ( "The outputMultiplier cannot be set to zero" );
				}
				scale = value;
			}
		}

	}	// END OF class
}
