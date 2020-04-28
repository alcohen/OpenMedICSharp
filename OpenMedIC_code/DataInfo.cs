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
	/// Stores data-related information for an acquisition run.
	/// </summary>
	public class DataInfo
	{
		/// <summary>
		/// Description of this data acquisition run
		/// </summary>
		private string description;
		/// <summary>
		/// data run start date/time;  default = Now
		/// </summary>
		private DateTime startTime;
		/// <summary>
		/// Number of bits per sample (usually it's the number of bits in the A/D)
		/// </summary>
		private int bitsPerSamp;
		/// <summary>
		/// Voltage that corresponds to a digital value of 0.0
		/// </summary>
		private float zeroRefVoltage;
		/// <summary>
		/// Voltage that corresponds to a digital value of [all 1s]
		/// </summary>
		private float fullScaleRefVoltage;
		/// <summary>
		/// Units (label) that apply to the data values, e.g., "mV" or "microAmps"
		/// </summary>
		private string valUnits;
		/// <summary>
		/// Scale multiplier (if calibration was performed)
		/// </summary>
		private float scaleMultip;
		/// <summary>
		/// Offset from zero of the values (if calibration was performed)
		/// </summary>
		private float offset;


		/// <summary>
		/// Creates a new identical copy of this object.  The returned clone is completely
		/// independent of this object, and the two can be changed/updated/deleted without
		/// one affecting the other.
		/// </summary>
		/// <returns>An identical copy of this object</returns>
		public DataInfo clone ()
		{
			DataInfo newInfo = new DataInfo ( description, StartDateTime );
			newInfo.BitsPerSample = bitsPerSamp;
			newInfo.zeroRefVoltage = zeroRefVoltage;
			newInfo.fullScaleRefVoltage = fullScaleRefVoltage;
			newInfo.scaleMultip = scaleMultip;
			newInfo.offset = offset;

			return newInfo;
		}

		/// <summary>
		/// Assign a property referencing it by the tag associated with it.
		/// </summary>
		/// <param name="tag">A varTags value</param>
		/// <param name="val">Value to assign</param>
		/// <returns>TRUE if the assignment was successful, FALSE otherwise.</returns>
		public bool setByTag ( ChainInfo.varTags tag, string val )
		{
			switch ( tag )
			{
				case ChainInfo.varTags.dataDescription:
					DataDescription = val;
					return true;
				case ChainInfo.varTags.startDateTime:
					StartDateTime = val;
					return true;
				case ChainInfo.varTags.bitsPerSample:
					BitsPerSample = Convert.ToInt32 ( val );
					return true;
				case ChainInfo.varTags.zeroReferenceVoltage:
					ZeroReferenceVoltage = val ;
					return true;
				case ChainInfo.varTags.fullScaleReferenceVoltage:
					FullScaleReferenceVoltage = val;
					return true;
				case ChainInfo.varTags.scaleMultiplier:
					ScaleMultiplier = val;
					return true;
				case ChainInfo.varTags.zeroOffset:
					ZeroOffset = val;
					return true;
			}
			return false;
		}

		/// <summary>
		/// Default constructor;  empty description, startTime = now
		/// </summary>
		public DataInfo()
		{
			// Defaults:
			description = "";
			startTime = DateTime.Now;
		}

		/// <summary>
		/// Default constructor;  initializes description and startTime from the 
		/// passed parameters.
		/// </summary>
		/// <param name="dataDescription">Verbose description of the data</param>
		/// <param name="startDateTime">start date/time of the data's acquisition</param>
		public DataInfo ( string dataDescription, string startDateTime )
		{
			description = dataDescription;
			StartDateTime = startDateTime;
		}

		/// <summary>
		/// Description of this data acquisition run
		/// </summary>
		public string DataDescription 
		{
			get
			{
				return description;
			}
			set
			{
				description = value;
			}
		}

		/// <summary>
		/// data run start date/time;  default = Now
		/// </summary>
		public string StartDateTime 
		{
			get
			{
				return startTime.ToString ();
			}
			set
			{
				startTime = Convert.ToDateTime ( value );
			}
		}

		/// <summary>
		/// Number of bits per sample (usually it's the number of bits in the A/D)
		/// </summary>
		public int BitsPerSample
		{
			get
			{
				return bitsPerSamp;
			}
			set
			{
				bitsPerSamp = value;
			}
		}

		/// <summary>
		/// Sample value that corresponds to a digital value of [all zeros].
		/// </summary>
		public string ZeroReferenceVoltage 
		{
			get
			{
				return zeroRefVoltage.ToString();
			}
			set
			{
				zeroRefVoltage = Convert.ToSingle ( value );
			}
		}

		/// <summary>
		/// Sample value that corresponds to a digital value of [all 1s]
		/// </summary>
		public string FullScaleReferenceVoltage 
		{
			get
			{
				return fullScaleRefVoltage.ToString();
			}
			set
			{
				fullScaleRefVoltage = Convert.ToSingle ( value );
			}
		}

		/// <summary>
		/// Units that apply to the data values.  This can be any arbitrary label, e.g., "mV" 
		/// or "microAmps" or "Smoots"
		/// </summary>
		public string ValueUnits
		{
			get
			{
				return valUnits;
			}
			set
			{
				valUnits = value;
			}
		}

		/// <summary>
		/// Scale multiplier (if calibration was performed).  This is the value by which we divide
		/// the A/D integer output to get a value in the specified (or implied) units.
		/// </summary>
		public string ScaleMultiplier 
		{
			get
			{
				return scaleMultip.ToString();
			}
			set
			{
				scaleMultip = Convert.ToSingle ( value );
			}
		}

		/// <summary>
		/// Offset from zero of the values (if calibration was performed).  This is the value in
		/// A/D units that corresponds to an input of 0.0 ValueUnits.
		/// </summary>
		public string ZeroOffset 
		{
			get
			{
				return offset.ToString();
			}
			set
			{
				offset = Convert.ToSingle ( value );
			}
		}

	}	// END of class DataInfo
}
