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
using System.Collections;

namespace OpenMedIC
{
	/// <summary>
	/// Stores the data needed for initialization of a chain.
	/// Note the following required data:
	/// 
	/// -&gt; All cases:
	///		-&gt; -&gt; Data Sampling period, in seconds.
	///	
	///	-&gt; When writing the date to a file:
	///		-&gt; -&gt; DataInfo.dataDescription = Text Description of File Contents;
	///		-&gt; -&gt; DataInfo.startDateTime = Start Date/Time (can be left blank to 
	///					initialize to when this class instance is created);
	///		-&gt; -&gt; DataInfo.bitsPerSample = data sample accuracy.  For raw analog
	///					data, this is the number of bits used to generate the number;
	///					for calibrated-value digital inputs, this is the precision to
	///					which the values have been acquired, calibrated, and manipulated.
	/// </summary>
	public class ChainInfo
	{

		#region Static methods/properties/etc. for ChainInfo management

		/// <summary>
		/// Tag Name-Value Pair objects:
		/// </summary>
		public struct tagValuePair { 
			/// <summary>
			/// Name of the tag
			/// </summary>
			public string tagName; 
			/// <summary>
			/// Value of the tag
			/// </summary>
			public string tagValue; }

		/// <summary>
		/// List of tags relating to properties available from this class.
		/// </summary>
		public enum varTags 
		{ 
			// - Generic:
            /// <summary>
            /// Tag for the sampling period, in seconds (20 Hz would correspond to 0.05)
            /// </summary>
			samplingPeriodSec,
			// - File-related:
            /// <summary>
            /// Tag for the data format (binary or text)
            /// </summary>
			dataFormat,
            /// <summary>
            /// Tag for name of the writer class being used
            /// </summary>
            writerClassName,
            /// <summary>
            /// Tag for current file number in multi-file-writing mode
            /// </summary>
            multiFileNum,
            /// <summary>
            /// Tag for the time when the file was started
            /// </summary>
            startTime,
			// - Data Info-related:
            /// <summary>
            /// Tag for the verbose data description
            /// </summary>
            dataDescription,
            /// <summary>
            /// Tag for the date and time when data acquisition started 
            /// </summary>
            startDateTime,
            /// <summary>
            /// Tag for the number of bits in a sample
            /// </summary>
            bitsPerSample,
            /// <summary>
            /// Tag for the integer (data in) value that corresponds to zero 
            /// volts
            /// </summary>
            zeroReferenceVoltage,
            /// <summary>
            /// Tag for the integer (data in) value that corresponds to "full 
            /// scale"
            /// </summary>
            fullScaleReferenceVoltage,
            /// <summary>
            /// Tag for the value by which the data in values must be multiplied,
            /// to obtain a correct voltage out value
            /// </summary>
            scaleMultiplier,
            /// <summary>
            /// Tag for how much to offset the data in so that a value of zero
            /// corresponds to zero volts
            /// </summary>
            zeroOffset,
			// - Patient Info-related:
            /// <summary>
            /// Tag for patient name's prefix (e.g., "Mr.", "Ms.", "Dr.")
            /// </summary>
            Prefix,
            /// <summary>
            /// Tag for patient's first name (given name)
            /// </summary>
            FirstName,
            /// <summary>
            /// Tag for patient's middle name, possibly the maiden name
            /// </summary>
            MiddleName,
            /// <summary>
            /// Tag for the patient's last name (surname)
            /// </summary>
            LastName,
            /// <summary>
            /// Tag for patient name's suffix (e.g., "MD", "PhD")
            /// </summary>
            Suffix,
            /// <summary>
            /// Tag for patient's address, line 1
            /// </summary>
            Addr1,
            /// <summary>
            /// Tag for patient's address, line 2
            /// </summary>
            Addr2,
            /// <summary>
            /// Tag for patient's address, name of city/post office
            /// </summary>
            City,
            /// <summary>
            /// Tag for patient's address, name of state
            /// </summary>
            State,
            /// <summary>
            /// Tag for patient's address, zip code
            /// </summary>
            Zip,
            /// <summary>
            /// Tag for patient's address, name of country
            /// </summary>
            Country,
            /// <summary>
            /// Tag for a Patient ID number
            /// </summary>
            PatientID,
            /// <summary>
            /// Tag for patient's age at the time of data acquisition
            /// </summary>
            Age,
            /// <summary>
            /// Tag for patient's date of birth
            /// </summary>
            DOB,
            /// <summary>
            /// Tag for the full patient name, including prefix, first, middle, 
            /// last, and suffix
            /// </summary>
            FullName,
            /// <summary>
            /// Tag for the full patient address, including addr1, addr2, city,
            /// state, zip, and country
            /// </summary>
            FullAddress
		};

		/// <summary>
		/// Ordered storage of tag-value pairs
		/// </summary>
		protected static Hashtable tagValues;

		/// <summary>
		/// Initializes the tagValues Hashtable (required for 
		/// at least some static methods).
		/// </summary>
		static ChainInfo ()
		{
			tagValues = new Hashtable ( Enum.GetValues( typeof(varTags) ).Length );
			// Generic:
			tagValues.Add ( varTags.samplingPeriodSec, "Sampling Period" );
			// File-related:
			tagValues.Add ( varTags.dataFormat, "Data Format (ASCII or Binary)" );
            tagValues.Add(varTags.writerClassName, "OpenMedIC File-Writing class name");
			tagValues.Add ( varTags.multiFileNum, "Multi-file acquisition, file number" );
			tagValues.Add ( varTags.startTime, "File Start Time" );
			// Data Info-related:
			tagValues.Add ( varTags.dataDescription, "Description" );
			tagValues.Add ( varTags.startDateTime, "Start Date/Time" );
			tagValues.Add ( varTags.bitsPerSample, "Bits per Sample" );
			tagValues.Add ( varTags.zeroReferenceVoltage, "Zero-reference Voltage" );
			tagValues.Add ( varTags.fullScaleReferenceVoltage, "Full-scale-reference Voltage" );
			tagValues.Add ( varTags.scaleMultiplier, "Scale Multiplier" );
			tagValues.Add ( varTags.zeroOffset, "Zero Offset" );
			// Patient Info-related:
			tagValues.Add ( varTags.PatientID, "Patient Identifier" );
			tagValues.Add ( varTags.Prefix, "Patient Name Prefix" );
			tagValues.Add ( varTags.FirstName, "Patient First Name" );
			tagValues.Add ( varTags.MiddleName, "Patient Middle Name" );
			tagValues.Add ( varTags.LastName, "Patient Last Name" );
			tagValues.Add ( varTags.Suffix, "Patient Suffix" );
			tagValues.Add ( varTags.Addr1, "Patient Address 1" );
			tagValues.Add ( varTags.Addr2, "Patient Address 2" );
			tagValues.Add ( varTags.City, "Patient Address City" );
			tagValues.Add ( varTags.State, "Patient Address State" );
			tagValues.Add ( varTags.Zip, "Patient Address Zipcode" );
			tagValues.Add ( varTags.Country, "Patient Address Country" );
			tagValues.Add ( varTags.Age, "Patient Age" );
			tagValues.Add ( varTags.DOB, "Patient Date of Birth" );
			tagValues.Add ( varTags.FullName, "Patient Name" );
			tagValues.Add ( varTags.FullAddress, "Patient Address" );
		}
		/// <summary>
		/// Returns the tag text corresponding to the passed varTags value.
		/// If tagID is not recognized, then it returns "[Unknown]".
		/// </summary>
		/// <param name="tagID">A varTags value identifying the property whose tag
		///				text is wanted</param>
		/// <returns>the tag text corresponding to the tagID.  If it's not recognized,
		///				it returns "[Unknown]"</returns>
		public static string getTagText ( Enum tagID )
		{
			string tag = (string)tagValues[tagID];

			if ( tag == null )
				return "[unknown]";	// default

			return tag;
		}

		/// <summary>
		/// Returns the tag that the given value corresponds to, or throws an
		/// exception if there is no match.
		/// Note that the match must be exact and case-sensitive!
		/// </summary>
		/// <param name="val">Tag Value to match to varTags tags</param>
		/// <returns>The appropriate tag;  throws an exception if no match.
		///		Note that the match must be exact and case-sensitive!</returns>
		public static varTags getTagFromValue ( string val )
		{
			DictionaryEntry dict;
			foreach ( DictionaryEntry tgt in tagValues )
			{
				dict = (DictionaryEntry) tgt;
				if ( (string)dict.Value == val )
					return (varTags) dict.Key;
			}
			// should never get here -- we already checked that it's a valid value:
			throw new ArgumentException ( "Value '" + val + "' is not a recognized Tag value." );
		}

		#endregion

		#region Non-static methods/properties for using this instance

		private double samplePeriod = 0;
		private PatientInfo pInfo = null;
		private DataInfo dInfo = null;
		private FileHandler fInfo = null;

		/// <summary>
		/// Creates a duplicate copy of this object and all its data.
		/// </summary>
		/// <returns>A new ChainInfo object identical to this one</returns>
		public ChainInfo clone ()
		{
			ChainInfo newData = new ChainInfo ();
			newData.samplingPeriodSec = this.samplingPeriodSec;
			newData.patientInfo		  = this.pInfo.clone();
			newData.dataInfo		  = this.dInfo.clone();
			newData.fileInfo		  = this.fInfo.clone();

			return newData;
		}

		/// <summary>
		/// Assign a property referencing it by the tag associated with it.
		/// </summary>
		/// <param name="tag">A varTags value</param>
		/// <param name="val">Value to assign</param>
		/// <returns>TRUE if the assignment was successful, FALSE otherwise.</returns>
		public bool setByTag ( varTags tag, string val )
		{
			if ( tag == varTags.samplingPeriodSec )
			{
				this.samplePeriod = Convert.ToDouble ( val );
				return true;
			}
			return false;
		}


		/// <summary>
		/// Basic constructor;  requires no parameters.
		/// Note that the sampling rate will be filled in by the data source.
		/// </summary>
		public ChainInfo ()
		{
		}

        /// <summary>
        /// Full constructor, requiring the patient info and the data information.
        /// </summary>
        /// <param name="patientData">Object containing all relevant information 
        ///     about the patient the data belongs to (can be null)</param>
        /// <param name="dataParams">Object containing all available information
        ///     about the data itself (can be null)</param>
		public ChainInfo ( PatientInfo patientData, DataInfo dataParams )
			: this ()
		{
			pInfo = patientData;
			dInfo = dataParams;
		}

		/// <summary>
		/// The current sampling period, in seconds
		/// </summary>
		public double samplingPeriodSec
		{
			set
			{
				samplePeriod = value;
			}
			get
			{
				return samplePeriod;
			}
		}

        /// <summary>
        /// Exposes the current DataInfo structure, which contains 
        /// all relevant information about the data.
        /// </summary>
		public DataInfo dataInfo
		{
			get
			{
				return dInfo;
			}
			set
			{
				dInfo = value;
			}
		}

        /// <summary>
        /// Exposes the current PatientInfo structure, which contains 
        /// all relevant information about the current patient.
        /// </summary>
		public PatientInfo patientInfo
		{
			get
			{
				return pInfo;
			}
			set
			{
				pInfo = value;
			}
		}

        /// <summary>
        /// Exposes the current FileInfo structure, which contains 
        /// methods and information for the file being written to (if any).
        /// </summary>
		public FileHandler fileInfo
		{
			get
			{
				return this.fInfo;
			}
			set
			{
				fInfo = value;
			}
		}

		#endregion

	}	// END of class ChainInfo
}
