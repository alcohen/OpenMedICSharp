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
using System.Text;

namespace OpenMedIC
{
	/// <summary>
	/// Stores and returns patient info as desired.
	/// If convenient, you can assign the full name to any one of the Name fields,
	/// and the whole address to any one of the Address fields (except Country), and
	/// they will be returned correctly by the fullName and fullAddress properties.
	/// </summary>
	public class PatientInfo
	{
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
				case ChainInfo.varTags.PatientID:
					this.PatientID = val;
					return true;
				case ChainInfo.varTags.Prefix:
					this.Prefix = val;
					return true;
				case ChainInfo.varTags.FirstName:
					this.FirstName = val;
					return true;
				case ChainInfo.varTags.MiddleName:
					this.MiddleName = val;
					return true;
				case ChainInfo.varTags.LastName:
					this.LastName = val;
					return true;
				case ChainInfo.varTags.Suffix:
					this.Suffix = val;
					return true;
				case ChainInfo.varTags.Addr1:
					this.Addr1 = val;
					return true;
				case ChainInfo.varTags.Addr2:
					this.Addr2 = val;
					return true;
				case ChainInfo.varTags.City:
					this.City = val;
					return true;
				case ChainInfo.varTags.State:
					this.State = val;
					return true;
				case ChainInfo.varTags.Zip:
					this.Zip = val;
					return true;
				case ChainInfo.varTags.Country:
					this.Country = val;
					return true;
				case ChainInfo.varTags.Age:
					this.Age = Convert.ToInt32 ( val );
					return true;
				case ChainInfo.varTags.DOB:
					this.DOBstring = val;
					return true;
				case ChainInfo.varTags.FullName:
					this.FullName = val;
					return true;
				case ChainInfo.varTags.FullAddress:
					this.FullAddress = val;
					return true;
			}
			return false;
		}

		private string patientID;
		private string firstName;
		private string middleName;
		private string lastName;
		private string prefix;
		private string suffix;
		private string addr1;
		private string addr2;
		private string city;
		private string state;
		private string zip;
		private string country;
		private int    age;
		private string dOB;

		// These are for when we are only given the full info, not the pieces:
		private string fullAddrInt = "";
		private string fullNameInt = "";

		/// <summary>
		/// Creates a duplicate copy of this object and all its data.
		/// </summary>
		/// <returns>A new PatientInfo object identical to this one</returns>
		public PatientInfo clone ()
		{
			PatientInfo newInfo = new PatientInfo ( firstName, middleName, lastName,
				addr1, addr2, city, state, zip, country );
			newInfo.age = age;
			newInfo.dOB = dOB;

			if ( ! OpenMedICUtils.isEmpty ( fullAddrInt ) )
				newInfo.fullAddrInt = fullAddrInt;
			if ( ! OpenMedICUtils.isEmpty ( fullNameInt ) )
				newInfo.fullNameInt = fullNameInt;

			return newInfo;
		}

		/// <summary>
		/// This constructor only sets patient name.
		/// If convenient, you can assign the full name to any one of the Name fields,
		/// and it will still be returned correctly by the fullName property.
		/// </summary>
		public PatientInfo ( string FirstName, string MiddleName, string LastName )
		{
			initNames ( FirstName, MiddleName, LastName );
		}

		private void initNames ( string FirstName, string MiddleName, string LastName )
		{
			firstName = FirstName;
			middleName = MiddleName;
			lastName = LastName;
		}

		/// <summary>
		/// This constructor only sets patient address.
		/// If convenient, you can assign the whole address to any one of the Address 
		/// fields (except Country), and it will be returned correctly by the 
		/// fullAddress property.
		/// </summary>
		public PatientInfo ( string Addr1, string Addr2, string City, 
							string State, string Zip, string Country )
		{
			addr1 = Addr1;
			addr2 = Addr2;
			city = City;
			state = State;
			zip = Zip;
			country = Country;
		}

        /// <summary>
        /// Storage for patient information, to be propagated to all the followers of
        /// this object at init time.
        /// All parameters can be left blank, but can't be left out.
        /// </summary>
        /// <param name="FirstName">First Name (Given Name)</param>
        /// <param name="MiddleName">Middle Name</param>
        /// <param name="LastName">Last Name (Surname)</param>
        /// <param name="Addr1">Address line 1</param>
        /// <param name="Addr2">Address line 2</param>
        /// <param name="City">Address city</param>
        /// <param name="State">Address state</param>
        /// <param name="Zip">Address zip code</param>
        /// <param name="Country">Address country</param>
		public PatientInfo ( string FirstName, string MiddleName, string LastName,
							string Addr1, string Addr2, string City, 
							string State, string Zip, string Country )
			: this ( Addr1, Addr2, City, State, Zip, Country )
		{	
			initNames ( FirstName, MiddleName, LastName );
		}

        /// <summary>
        /// Unique patient identifier string
        /// </summary>
		public string PatientID
		{
			get
			{
				return patientID;
			}
			set
			{
				patientID = value;
			}
		}

        /// <summary>
        /// Patient's first name (Given Name)
        /// </summary>
		public string FirstName
		{
			get
			{
				return firstName;
			}
			set
			{
				firstName = value;
			}
		}

        /// <summary>
        /// Patient's middle name
        /// </summary>
		public string MiddleName
		{
			get
			{
				return middleName;
			}
			set
			{
				middleName = value;
			}
		}

        /// <summary>
        /// Patient's last name (Surname)
        /// </summary>
		public string LastName
		{
			get
			{
				return lastName;
			}
			set
			{
				lastName = value;
			}
		}

        /// <summary>
        /// Patient's prefix to their name, e.g., "Mr.", "Ms.", "Dr."
        /// </summary>
		public string Prefix
		{
			get
			{
				return prefix;
			}
			set
			{
				prefix = value;
			}
		}

        /// <summary>
        /// Patient's suffix to their name, e.g., "MD, PhD", "Jr."
        /// </summary>
		public string Suffix
		{
			get
			{
				return suffix;
			}
			set
			{
				suffix = value;
			}
		}

        /// <summary>
        /// Patient's address, line 1
        /// </summary>
		public string Addr1
		{
			get
			{
				return addr1;
			}
			set
			{
				addr1 = value;
			}
		}

        /// <summary>
        /// Patient's address, line 2
        /// </summary>
		public string Addr2
		{
			get
			{
				return addr2;
			}
			set
			{
				addr2 = value;
			}
		}

        /// <summary>
        /// Patient's address, city
        /// </summary>
		public string City
		{
			get
			{
				return city;
			}
			set
			{
				city = value;
			}
		}

        /// <summary>
        /// Patient's address, state
        /// </summary>
		public string State
		{
			get
			{
				return state;
			}
			set
			{
				state = value;
			}
		}

        /// <summary>
        /// Patient's address zip code
        /// </summary>
		public string Zip
		{
			get
			{
				return zip;
			}
			set
			{
				zip = value;
			}
		}

        /// <summary>
        /// Patient's address, name of the country
        /// </summary>
		public string Country
		{
			get
			{
				return country;
			}
			set
			{
				country = value;
			}
		}

        /// <summary>
        /// Patient's age at this time
        /// </summary>
		public int Age
		{
			get
			{
				return age;
			}
			set
			{
				age = value;
			}
		}

        /// <summary>
        /// Patient's date of birth, as a formatted string
        /// </summary>
		public string DOBstring
		{
			get
			{
				return dOB;
			}
			set
			{
				dOB = value;
			}
		}

        /// <summary>
        /// Patient's date of birth, as a DateTime number
        /// </summary>
		public DateTime DOB
		{
			get
			{
				return Convert.ToDateTime ( dOB );
			}
			set
			{
				dOB = Convert.ToString ( value );
			}
		}

		/// <summary>
		/// The full name as a formatted concatenation of 
		/// prefix, first, middle, last, suffix
		/// </summary>
		public string FullName
		{
			get
			{
				StringBuilder full = new StringBuilder ();

				if ( !OpenMedICUtils.isEmpty(prefix) )
					full.Append ( prefix ).Append ( " " );

				if ( !OpenMedICUtils.isEmpty(firstName) )
					full.Append ( firstName ).Append ( " " );

				if ( !OpenMedICUtils.isEmpty(middleName) )
				{
					if ( middleName.Length == 1 )
						// Want a period after it:
						full.Append ( middleName ).Append ( "." ).Append ( " " );
					else
						full.Append ( middleName ).Append ( " " );
				}

				if ( !OpenMedICUtils.isEmpty(lastName) )
					full.Append ( lastName ).Append ( " " );

				if ( !OpenMedICUtils.isEmpty(suffix) )
					full.Append ( suffix );

				// Check whether all the above was in vain:
				if ( full.Length == 0 && this.fullNameInt.Length > 0 )
					return fullNameInt;

				return full.ToString().Trim();
			}
			set
			{
				fullNameInt = value;
			}
		}

		/// <summary>
		/// The address as a formatted concatenation of 
		/// Addr1, Addr2, City, State Zip (Country)
		/// </summary>
		public string FullAddress
		{
			get
			{
				StringBuilder full = new StringBuilder ();

				if ( !OpenMedICUtils.isEmpty(addr1) )
					full.Append ( addr1 ).Append ( ", " );

				if ( !OpenMedICUtils.isEmpty(addr2) )
					full.Append ( addr2 ).Append ( ", " );

				if ( !OpenMedICUtils.isEmpty(city) )
					full.Append ( city ).Append ( ", " );

				if ( !OpenMedICUtils.isEmpty(state) )
					full.Append ( state ).Append ( " " );

				if ( !OpenMedICUtils.isEmpty(zip) )
					full.Append ( zip ).Append ( " " );

				if ( !OpenMedICUtils.isEmpty(country) )
					full.Append ( "(" ).Append ( country ).Append ( ")" );

				// Check whether all the above was in vain:
				if ( full.Length == 0 && fullAddrInt.Length > 0 )
					return fullAddrInt;

				// Final clean-up:
				string result = full.ToString().Trim();
				if ( result.EndsWith ( "," ) )
					return result.TrimEnd ( ',' );
				// else
				return result;
			}
			set
			{
				fullAddrInt = value;
			}
		}

	}	// END of class PatientInfo
}
