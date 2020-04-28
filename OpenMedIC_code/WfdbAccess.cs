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
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Wfdb;

namespace OpenMedIC
{
	/// <summary>
	/// PhysioNet is an Internet resource for biomedical research and development sponsored
	/// by the NIH's National Center for Research Resources.  PhysioNet, PhysioBank, WFDB, 
	/// and PhysioToolkit are the product of collaborative efforts by numerous people, too 
	/// numerous to mention here; please visit the PhysioNet website (www.physionet.org) for 
	/// details.
	/// The term "Wfdb" is used throughout to indicate the suite of functions included in the
	/// WFDB Library.
	/// This is a static class to simplify and/or document the accessing of the WFDB methods
	/// in wfdb-sharp.dll (namespace Wfdb).
	/// </summary>
	internal static class WfdbAccess
	{
		#region Common Definitions

		/// <summary>
		/// Enumerator of functions that may return an integer 
		/// </summary>
		public enum LastCall
		{
			/// <summary>
			/// Identifies the last called function as GetSignalCount (...).
			/// </summary>
			LastCall_SignalCount,
			/// <summary>
			/// Identifies the last called function as GetSignalInfo (...).
			/// Note that GetSignalInfo(...) doesn't return an integer, but it uses it internally.
			/// </summary>
			LastCall_SignalInfo
			//LastCall_Signal,
			//LastCall_AnnInfo
		}

		/// <summary>
		/// New-Line string for multi-line strings.
		/// </summary>
		public static string newLine = "\r\n";	// New-line character

		#endregion

		#region Common Methods

		/// <summary>
		/// Gets the current record search path for the WFDB methods.  The default is
		/// typically ". /wfdb/database http://www.physionet.org/physiobank/database".
		/// Note that directory names may be mapped to DOS 8.3 format if needed.
		/// </summary>
		/// <returns>The current search path, as a space-delimited string.</returns>
		public static string GetSearchPath()
		{
			try
			{
				string path = wfdb.getwfdb();
				return path;
			}
			catch (Exception getwfdbException)
			{
				while (getwfdbException.InnerException != null)
				{	// Drill down:
					getwfdbException = getwfdbException.InnerException;
				}
				MessageBox.Show("Exception occurred trying getwfdb(): \n"
					+ getwfdbException.ToString());
				return "";
			}
		}

		/// <summary>
		/// Sets the current record search path for the WFDB methods.  The default is
		/// typically ". /wfdb/database http://www.physionet.org/physiobank/database".
		/// The items in the path must be space- or semicolon-delimited; they can be 
		/// relative or absolute paths (under Windows that means "d:/xxx/yyy/zzz"); 
		/// under Windows they can contain backslashes instead of forward slashes.
		/// Note dat directory names cannot contain spaces (because space is a 
		/// delimiter), so names with spaces must be converted to DOS format 
		/// ("This Directory" becomes "ThisDi~1").
		/// </summary>
		/// <param name="path">The new search path.</param>
		/// <returns>True if successful, False otherwise.</returns>
		public static bool SetSearchPath(string path)
		{
			try
			{
				wfdb.setwfdb(path);
			}
			catch (Exception setwfdbException)
			{
				while (setwfdbException.InnerException != null)
				{	// Drill down:
					setwfdbException = setwfdbException.InnerException;
				}
				MessageBox.Show("Exception occurred trying setwfdb(...): \n"
					+ setwfdbException.ToString());
				return false;
			}
			return true;
		}

		/// <summary>
		/// Closes all open connections to objects in WFDB, including headers, data files,
		/// and annotation files.  Recommended as a clean-up.
		/// </summary>
		public static void CloseWfdb()
		{
			try
			{
				Wfdb.wfdb.wfdbquit();
			}
			catch { }
		}

		/// <summary>
		/// Returns a descriptive error message to interpret the returned integer value from
		/// several functions.
		/// </summary>
		/// <param name="returnID">The integer value to be interpreted.</param>
		/// <param name="calledFn">The function that returned returnID.</param>
		/// <returns>A descriptive error message.</returns>
		public static string GetErrMsgFromReturnID(int returnID, LastCall calledFn)
		{
			string result = "";
			switch (calledFn)
			{
				case LastCall.LastCall_SignalCount:
					switch (returnID)
					{
						case 0:
							result = "No signals found.";
							break;
						case -1:
							result = "Unable to read header file (probably incorrect record name).";
							break;
						case -2:
							result = "Incorrect header file format.";
							break;
						default:
							if (returnID < 0)
							{	// Unknown error:
								result = "[unknown error]";
							}
							else
							{	// Not an error:
								result = returnID + " signals found.";
							}
							break;
					}
					break;

				case LastCall.LastCall_SignalInfo:
					switch (returnID)
					{
						case (0):
							// No input signals available
							result = "No input signals available!";
							break;
						case (-1):
							// Unable to read header file
							result = "Unable to read header file!";
							break;
						case (-2):
							// Incorrect header file format
							result = "Incorrect header file format!";
							break;
						case (-999):
							// Exception occurred;  already reported
							break;
						default:
							if (returnID < 0)
							{	// Some other error, undefined
								result = "[unknown error]";
							}
							else
							{	// No error:
								result = "";
							}
							break;
					}
					break;

				//case LastCall.LastCall_AnnInfo:
				//    switch (returnID)
				//    {
				//        case 0:
				//            result = "";
				//            break;
				//        default:
				//            result = "[unknown error]";
				//            break;
				//    }
				//    break;

				//case LastCall.LastCall_Signal:
				//    switch (returnID)
				//    {
				//        case 0:
				//            result = "";
				//            break;
				//        default:
				//            result = "[unknown error]";
				//            break;
				//    }
				//    break;
				default:
					result = "[unknown error in unknown call]";
					break;
			}
			return result;
		}

		#endregion

		#region Signal Header Methods

		/// <summary>
		/// Retrieves the number of signals in the specified record.
		/// If an error occurs or the signal is not found, errMsg is filled with a meaningful
		/// error message.
		/// </summary>
		/// <param name="record">[path/]name of the desired record.</param>
		/// <param name="errMsg">If an error occurs, this contains the description of the error.</param>
		/// <returns>The number of signals if successful, otherwise 0 or negative.</returns>
		public static int GetSignalCount(string record, ref string errMsg)
		{
			int result = 0;
			errMsg = "";
			try
			{
				result = wfdb.isigopen(record, null, 0);
				if ( result < 1 )
				{	// Something failed -- find out what:
					errMsg = wfdb.wfdberror();
					while (errMsg.EndsWith("\n") || errMsg.EndsWith("\r"))
					{
						errMsg = errMsg.Substring(0, errMsg.Length - 1);
					}
					errMsg += newLine;
				}
			}
			catch (Exception isigopenException)
			{
				while (isigopenException.InnerException != null)
				{	// Drill down:
					isigopenException = isigopenException.InnerException;
				}
				errMsg += isigopenException.ToString() + newLine;
				MessageBox.Show("Exception occurred trying isigopen(...): \n"
					+ isigopenException.ToString() + "\n");
				result = -3;
			}

			return result;
		}

		/// <summary>
		/// Retrieves the data sampling frequency for the specified record.  If an error occurs,
		/// it returns:  -1 for "Unable to read header file", -2 for "Incorrect header file format".
		/// </summary>
		/// <param name="record">[path/]name of the desired record.</param>
		/// <returns>The sampling frequency if successful, a negative number if an error occurs.</returns>
		public static double GetSamplingFrequency(string record)
		{
			double freq = 0;
			try
			{
				freq = wfdb.sampfreq ( record );
			}
			catch (Exception sampfreqException)
			{
				while (sampfreqException.InnerException != null)
				{	// Drill down:
					sampfreqException = sampfreqException.InnerException;
				}
				MessageBox.Show("Exception occurred trying sampfreq(...): \n"
					+ sampfreqException.ToString());
				return 0;
			}
			return freq;
		}

		/// <summary>
		/// Retrieves signal information for the specified record, and stores it in siArray.
		/// numSignals must match the dimension of siArray, and must be equal to or greater than
		/// the number of signals in the first (and usually, the only) group of the record.
		/// </summary>
		/// <param name="record">[path/]name of the desired record.</param>
		/// <param name="siArray">Storage for signal information to be returned.</param>
		/// <param name="numSignals">Number of signals in the first (and usually, the only) group of the record.</param>
		/// <param name="errMsg">Storage for verbose error messages.</param>
		/// <returns>True if successful, False if an error occurs.</returns>
		public static bool GetSignalInfo(string record, WFDB_SiginfoArray siArray, int numSignals, ref string errMsg)
		{
			int result = 0;
			errMsg = "";
			try
			{
				result = Wfdb.wfdb.isigopen(record, siArray.cast(), numSignals);
				if ( result < 1 )
				{	// Something failed -- find out what:
					errMsg = wfdb.wfdberror();
					while (errMsg.EndsWith("\n") || errMsg.EndsWith("\r"))
					{
						errMsg = errMsg.Substring(0, errMsg.Length - 1);
					}
					errMsg = "isigopen(...) error:" + newLine + "\t" + errMsg + newLine;
					// Append the by-the-numbers error:
					errMsg += GetErrMsgFromReturnID(result, LastCall.LastCall_SignalInfo) + newLine;
					return false;
				}
			}
			catch (Exception isigopenException)
			{
				while (isigopenException.InnerException != null)
				{	// Drill down:
					isigopenException = isigopenException.InnerException;
				}
				errMsg = isigopenException.ToString() + newLine;
				MessageBox.Show("Exception occurred trying isigopen(...): \n"
					+ isigopenException.ToString());
				//result = -3;
				return false;
			}
			return true;
		}

		/// <summary>
		/// Retrieves the information strings from the Header file.  These are effectively
		/// free-text comments for the data.
		/// </summary>
		/// <param name="record">[path/]name of the desired record.</param>
		/// <param name="errMsg">Storage for verbose error messages.</param>
		/// <returns>The information strings for the specified record.</returns>
        public static string GetHeaderInfoStrings(string record, ref string errMsg)
        {
            string info = null;
            string row;
            errMsg = "";
            try
            {
                row = wfdb.getinfo(record);
                if (row != null)
                {
                    info = row.Trim();
                }
                while (row != null)
                {   // Additional string:
                    row = wfdb.getinfo(null);
                    if ( (row != null) && (row.Trim().Length > 0) )
                    {   // Additional string:
                        info += "\n\t\t\t" + row.Trim();
                    }
                }   // while(row != null)
            }   // try
            catch (Exception getinfoException)
            {
                while (getinfoException.InnerException != null)
                {	// Drill down:
                    getinfoException = getinfoException.InnerException;
                }
                errMsg += getinfoException.ToString() + newLine;
                MessageBox.Show("Exception occurred trying getinfo(...): \n"
                    + getinfoException.ToString() + "\n");
            }   // catch

            return info;
        }

		/// <summary>
		/// Initializes and returns a blank SigInfoArray object containing numItems SigInfo objects.
		/// </summary>
		/// <param name="numItems">Number of SigInfo objects in SigInfoArray</param>
		/// <param name="errMsg">Storage for verbose error messages, if any.</param>
		/// <returns>A blank SigInfoArray object of dimension numItems.</returns>
		public static WFDB_SiginfoArray GetSigInfoArray(int numItems, ref string errMsg)
		{
			WFDB_SiginfoArray siArray;
			errMsg = "";
			// First try opening the array:
			try
			{
				siArray = new WFDB_SiginfoArray(numItems);
			}
			catch (Exception siarrayException)
			{
				while (siarrayException.InnerException != null)
				{	// Drill down:
					siarrayException = siarrayException.InnerException;
				}
				errMsg = siarrayException.ToString() + newLine;
				MessageBox.Show("Exception occurred executing 'siArray = new WFDB_Siginfo()': \n"
					+ siarrayException.ToString());
				return null;
			}
			// No exceptions -- init the array:
			for (int i = 0; i < numItems; i++)
			{
				WFDB_Siginfo newInfo = new WFDB_Siginfo();
				//				  1234567890 234567890 234567890 234567890 234567890 234567890 234567890 234567890 234567890 234567890 234567890 234567890
				//						   1         2         3         4         5         6         7         8         9        10        11        12
				newInfo.desc  = "                                                                                                                        ";
				newInfo.fname = "                                                                                                                        ";
				newInfo.units = "                                                                                                                        ";
				siArray.setitem(i, newInfo);
			}
			return siArray;
		}

		#endregion

		#region Signal Handling Methods

		/// <summary>
		/// Retrieves a frame of data for each signal in the group or record.
		/// </summary>
		/// <param name="frameData">Data class to be initialized and filled with data.</param>
		/// <param name="siArray">Signal Information array for the signals being read.</param>
		/// <param name="numSignals">Number of signals being read (must match the dimension of siArray).</param>
		/// <param name="errMsg">String where error messages, if any, are written.</param>
		/// <returns>True if successful, False otherwise.  If False, then errMsg contains the explanation why.</returns>
		public static bool GetSignalFrame (
			ref WFDB_SampleArray frameData, 
			WFDB_SiginfoArray siArray, 
			int numSignals, 
			ref string errMsg )
		{
			int numSamples;

			numSamples = siArray.getitem(0).spf * numSignals;
			errMsg = "";
			try
			{
				frameData = new WFDB_SampleArray(numSamples);
				int result = wfdb.getframe(frameData.cast());
				if (result == -1)
				{	// End of data
					errMsg = "End of data reached.";
					return false;
				}
				else if (result == -3)
				{	// Failure:  unexpected physical end of file
					errMsg = "Failure:  unexpected physical end of file.";
					return false;
				}
				else if (result == -4)
				{	// Failure:  checksum error (detected only at end of file)
					errMsg = "Failure:  checksum error (detected at end of file).";
					return false;
				}
				else if (result < numSignals)
				{	// Something wrong!  We didn't get the signals we expect!
					errMsg = "Failure:  requested " + numSignals + " signals, but only received data for " + result + ".";
					return false;
				}
				// else, it's OK
			}
			catch (Exception getframeException)
			{
				while (getframeException.InnerException != null)
				{	// Drill down:
					getframeException = getframeException.InnerException;
				}
				errMsg += getframeException.ToString() + newLine;
				MessageBox.Show("Exception occurred trying getframe(...): \n"
					+ getframeException.ToString() + "\n");
				return false;
			}
			// No exception?  Success!
			return true;
		}

		#endregion

		#region Annotation Handling Methods

        /// <summary>
        /// Opens a SINGLE annotation (closing any other open annotations), according to
        /// the passed parameters, and initializing and returning a WFDB_AnninfoArray object.
        /// </summary>
        /// <param name="record">Name of the record whose annotation we are opening.</param>
        /// <param name="aiArray">Annotation Info array (one element), built and populated
        ///                         by this method.</param>
        /// <param name="annotName">Name (i.e., file extension) of the Annotation.</param>
		/// <param name="openForRead">True to open for reading, False to open for writing.</param>
        /// <param name="errMsg">Contains any error messages relevant to failed execution.</param>
        /// <returns>True if successful, False if failed.  If failed, the cause is in errMsg.</returns>
        public static bool OpenAnnotation(
            string record,
            ref WFDB_AnninfoArray aiArray,
            string annotName,
            bool openForRead,
            ref string errMsg)
        {
            try
            {
                aiArray = new WFDB_AnninfoArray(1);
                WFDB_Anninfo annInfo = new WFDB_Anninfo();
                annInfo.name = annotName;
                if (openForRead)
                {
                    annInfo.stat = wfdb.WFDB_READ;
                }
                else
                {
                    annInfo.stat = wfdb.WFDB_WRITE;
                }
                aiArray.setitem(0, annInfo);
                wfdb.annopen(record, aiArray.cast(), 1);
            }
            catch (Exception annopenException)
            {
                while (annopenException.InnerException != null)
                {	// Drill down:
                    annopenException = annopenException.InnerException;
                }
                errMsg += annopenException.ToString() + newLine;
                MessageBox.Show("Exception occurred trying annopen(...): \n"
                    + annopenException.ToString() + "\n");
                return false;
            }
            // No exception?  Success!
            return true;
        }


        /// <summary>
        /// Retrieves the next annotation from the currently open annotation file.
        /// Annotation files are opened using OpenAnnotation (...).
        /// </summary>
        /// <param name="errMsg">Stores any error messages.</param>
        /// <returns>The next annotation, if successful, otherwise null.</returns>
        public static WFDB_Annotation GetAnnotation(
            ref string errMsg)
        {
            WFDB_Annotation annot = null;
            errMsg = "";
            try
            {
                annot = new WFDB_Annotation();
                wfdb.getann(0, annot);
            }
            catch (Exception getannException)
            {
                while (getannException.InnerException != null)
                {	// Drill down:
                    getannException = getannException.InnerException;
                }
                errMsg += getannException.ToString() + newLine;
                MessageBox.Show("Exception occurred trying getann(...): \n"
                    + getannException.ToString() + "\n");
                return null;
            }
            // No exception?  Success!
            return annot;
        }

        /// <summary>
        /// Closes the open annotation (only one can be open at a time using this set of methods).
        /// </summary>
        public static void CloseAnnotation()
        {
            try
            {
                wfdb.iannclose(0);
            }
            catch (Exception ianncloseException)
            {
                while (ianncloseException.InnerException != null)
                {	// Drill down:
                    ianncloseException = ianncloseException.InnerException;
                }
                MessageBox.Show("Exception occurred trying iannclose(...): \n"
                    + ianncloseException.ToString() + "\n");
            }
            // No exception?  Success!

        }

		#endregion

		/// <summary>
		/// Moves the current position within the currently open file by the 
		/// specified number of samples.
		/// If numSamples is negative, then we move backwards, otherwise we
		/// move forwards.
		/// </summary>
		/// <param name="numSamplesFromStart">Number of samples from start of file,
		///		where we want to be</param>
		/// <param name="errMsg">If not successful, this string contains a meaningful
		///		message describing the problem</param>
		/// <returns>true if successful, false if not.  If not successful, then 
		///		errMsg is populated with a meaningful message</returns>
		public static bool Seek(int numSamplesFromStart, ref string errMsg)
		{
			errMsg = "";
			// Do the actual seek to the desired point:
			try
			{
				int result = wfdb.isigsettime(numSamplesFromStart);
				// Result?
				if (result != 0)
				{	// Something went wrong -- what?
					errMsg = wfdb.wfdberror();
					return false;
				}
			}
			catch (Exception isigsettimeE)
			{
				while (isigsettimeE.InnerException != null)
				{	// Drill down:
					isigsettimeE = isigsettimeE.InnerException;
				}
				errMsg += newLine + isigsettimeE.ToString();
				MessageBox.Show("Exception occurred trying isigsettime(...): \n"
					+ isigsettimeE.ToString() + "\n");
				return false;
			}
			return true;	// success
		}

	}
}
