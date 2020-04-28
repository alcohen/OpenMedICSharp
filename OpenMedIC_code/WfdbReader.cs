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
	/// This class reads the header and data from a PhysioNet Database record, and outputs
	/// the data as a DataSource would.  Note that, for now, a FileReader will only read data
	/// from the first signal of a record.
    /// <para>NOTE:  It is recommended to call the ReleaseWfdb() method when releasing an
    /// instance of WfdbReader class.</para>
    /// <para>NOTE:  The core Physionet WFDB code i NOT thread-safe.  This means that
    /// calling it from multiple threads or instances of WfdbReader will cause unpredictable
    /// behavior (possibly errors or wrong data being received).  The problem comes from
    /// having only one instance of each file handle shared between all threads.</para>
	/// <para>NOTE:  The init(...) method call GC.Collect() to force garbage collection
	/// on the managed-code environment.  If you are running unmanaged code, before calling 
	/// that method you must ensure that your unmanaged storage allocations are properly
	/// protected or released.</para>
	/// <para>NOTE:  The sample values are outputted as actual mV values.</para>
	/// </summary>
	public class WfdbReader : DataSource
	{

		#region Private/Protected Variables

		private string searchPath;		// Optional record search path
		private string recordName;		// Name of the record being read
		private int adcZeroRef;			// signal value corresponding to 0.0
		private int adcResBits;			// Number of bits in A/D
		private int adcMaxValue;		// Max. signal value; it's 2**adcResBits
		private int baseline;			// signal baseline value (sig. value corresponding to 0.0V input)
		private double ampGain;			// How much the signal is amplified before sampling
		private string dataUnits;		// What units the values are in
        private double sampFreq;		// Sampling frequency for the signal (in samples/sec.)

        public double SamplingPeriod
        {
            get { return (1.0/sampFreq); }
            set { sampFreq = 1.0/value; }
        }

		private int numSignals;			// Number of signals in the record (only first one is actually used)
		private Wfdb.WFDB_SiginfoArray siArray;	// The Signal Information array for this record
		private long numSamples = 0;	// Total number of samples in the first signal of the record
		private long dispSamples;		// Number of samples already read and displayed
		private string sigName;			// Name assigned to signal being read

		private int[] frameBuffer;		// Buffer for the data;  stores one frame's worth of samples
		private int bufferPointer = 0;	// Current value in the buffer (i.e. the next one to retrieve)
		private int buffSize;			// what we initialize the buffer size to (= frame size)
		private bool doneReading;		// Gets set when we hit the end of the record

		/// <summary>
		/// Header information storage -- save the actual ChainInfo object
		/// </summary>
		protected ChainInfo initValues;

		#endregion	Private/Protected Variables

		#region Constructors & Destructor

/*
		/// <summary>
		/// Minimal constructor.  Note that if you use this constructor, you must set 
		/// recordName before calling init(), otherwise an exception will be raised.
		/// </summary>
		/// <param name="secondsPerStep">Interval between samples, in seconds or fraction thereof,
		///					e.g. 1 KHz = 1 ms per sample would have secondsPerStep = 0.001.  If
		///					autoOutput is TRUE and the file header has its own sample period value,
		///					then this param is ignored and the value in the file header is used 
		///					instead.</param>
		/// <param name="autoOutput">If TRUE, then we push data out to the Followers AND try to
		///					use the sample period value from the file header; if FALSE, we wait
		///					for whoever to pull out the data.</param>
		public WfdbReader ( double secondsPerStep, bool autoOutput )
			: base ( secondsPerStep, autoOutput )
		{	// Nothing to do.
		}
//	*/

		/// <summary>
		/// Full constructor:  requires a record name and an optional path.
		/// </summary>
		/// [param name="secondsPerStep">Interval between samples, in seconds or fraction thereof,
		///					e.g. 1 KHz = 1 ms per sample would have secondsPerStep = 0.001.  If
		///					autoOutput is TRUE and the file header has its own sample period value,
		///					then this param is ignored and the value in the file header is used 
		///					instead.[/param>
		/// <param name="autoOutput">If TRUE, then we push data out to the Followers AND try to
		///					use the sample period value from the file header; if FALSE, we wait
		///					for whoever to pull out the data.</param>
		/// <param name="path">A search path for the record;  if null, then the default path is used.</param>
		/// <param name="rName">Must be an existing record name within the path.</param>
		public WfdbReader(/* double secondsPerStep, */ bool autoOutput,
							string path, string rName)
			: base ( /*secondsPerStep*/ 1, autoOutput )
		{
			string errMsg = "";
			// Save values:
			searchPath = path.Trim();
			recordName = rName.Trim();

			// Try the path and record name:
			if (searchPath != null)
			{
				WfdbAccess.SetSearchPath(searchPath);
			}
			sampFreq = WfdbAccess.GetSamplingFrequency(recordName);
			if (sampFreq < 1)
			{	// Problem reading header!
				//System.Windows.Forms.MessageBox("Unable to access record '" + recordName + "' in path '" + searchPath + "'!");
				Exception ex = new ArgumentException("Unable to access header for record '" + recordName + "' in path '" + searchPath + "'.");
				throw ex;
			}
			else
			{	// Set secondsPerStep:
				this.stepSize = 1 / sampFreq;
			}
			numSignals = WfdbAccess.GetSignalCount(recordName, ref errMsg);
			if (numSignals < 1)
			{
				//System.Windows.Forms.MessageBox("Unable to access record '" + recordName + "' in path '" + searchPath + "'!");
				Exception ex = new ArgumentException("Unable to access record '" + recordName + "' in path '" + searchPath + "'.");
				throw ex;
			}
		}

		/// <summary>
		/// Release Wfdb connections.
		/// </summary>
		~WfdbReader()
		{	// Release as needed:
			ReleaseWfdb();
		}

		#endregion	Constructors & Destructor

		#region Private Methods

		private bool ReadBuffer()
		{
			string errMsg = null;
			Wfdb.WFDB_SampleArray frameData = null;
			bool result = WfdbAccess.GetSignalFrame(ref frameData, siArray, numSignals, ref errMsg);
			if (result && (frameData.getitem(0) == 0))
			{	// Something weird!
				errMsg = null;	// just for a flag
			}
			if (!result)
			{
				MessageBox.Show(errMsg);
				doneReading = true;		// give up actual reading
				return false;
			}
			// No error -- read frame data into buffer:
			for (int samp = 0; samp < buffSize; samp++)
			{
				frameBuffer[samp] = frameData.getitem(samp);
				//if (frameBuffer[samp] == 0)
				//{	// Something weird!
				//    result = false;	// just for a flag
				//}
			}
			// And, reset buffer pointer
			bufferPointer = 0;

			return true;
		}

		private void GoToPositionInFile(long samps, ref string errMsg)
		{
			if (samps > (int)samps)
			{	// out of range -- go to max. int. val.:
				samps = Int32.MaxValue;
			}
			WfdbAccess.Seek((int)samps, ref errMsg);
			// Adjust cur-pos reference:
			dispSamples = samps;
		}

		#endregion	Private Methods

		#region Method overrides


		/// <summary>
		/// Determines the next value in the sequence - in this case, retrieves the next value from the
		/// frame buffer.
		/// </summary>
		/// <returns>The next available value</returns>
		protected override float nextVal()
		{
			float val;		// what we return
			int sample;		// the value from the record

			if (bufferPointer >= buffSize)
			{
				// We ran out of buffer (possibly not buffering at all):
				if (doneReading)
				{
					// Ran out of file!
					if (autoSend)
					{	// we auto-generate values
						//MessageBox.Show("Data Record is complete.");
						// Stop the timer then quit:
						this.Terminate();
						return 0F;
					}
					else
					{	// non-auto-generate
						// Return a string of zeros:
						return 0F;
					}
				}
				else
				{	// Normal case - get another frame:
					if (!ReadBuffer())
					{	// Failed!
						return 0F;
					}
				}
			}
			// Now we retrieve and return the next value from the buffer:
			sample = frameBuffer[bufferPointer];
			// Center and scale:
            val = (Single)((Double)(sample - baseline) / ampGain);
			bufferPointer++;
			// Update point tracking:
			dispSamples ++;
			if (dispSamples >= numSamples)
			{	// That's it!
				doneReading = true;
			}

			return val;
		}

		/// <summary>
		/// Override default behavior to:
		/// - Save a local copy
		/// - Get file header info
		/// - Update iData accordingly
		/// - Allow using header info to drive the output rates.
        /// <para>NOTE:  This method call GC.Collect() to force garbage collection on the 
        /// managed-code environment.  If you are running unmanaged code, before calling 
        /// this method you must ensure that your unmanaged storage allocations are properly 
        /// protected or released.</para>
        /// </summary>
		public override void init(ChainInfo iData)
		{
			string errMsg = null;
			// Reset THOROUGHLY:
            GC.Collect();
            GC.WaitForPendingFinalizers();
            WfdbAccess.CloseWfdb();
            GC.Collect();
            GC.WaitForPendingFinalizers();
			// Read record header:
			siArray = WfdbAccess.GetSigInfoArray(numSignals, ref errMsg);
			bool result = WfdbAccess.GetSignalInfo(recordName, siArray, numSignals, ref errMsg);
			if (!result)
			{
				//System.Windows.Forms.MessageBox("Unable to access header for record '" + recordName + "' in path '" + searchPath + "'!");
				Exception ex = new ArgumentException("Unable to access header for record '" + recordName + "' in path '" + searchPath + "'.");
				throw ex;
			}
			// Read useful info from header:
			WFDB_Siginfo sigInfo = siArray.getitem(0);
			buffSize = sigInfo.spf;
			frameBuffer = new int[buffSize];
			numSamples = sigInfo.nsamp * sigInfo.spf;
			sigName = sigInfo.desc;
			//sampFreq = WfdbAccess.GetSamplingFrequency(recordName);
			//if (sampFreq < 1)
			//{	// Problem reading header!
			//    //System.Windows.Forms.MessageBox("Unable to access record '" + recordName + "' in path '" + searchPath + "'!");
			//    Exception ex = new ArgumentException("Unable to access header for record '" + recordName + "' in path '" + searchPath + "'.");
			//    throw ex;
			//}
			adcZeroRef = sigInfo.adczero;
			adcResBits = sigInfo.adcres;
			adcMaxValue = (int)Math.Pow(2, adcResBits);
			baseline = sigInfo.baseline;
			ampGain = sigInfo.gain;
			if (ampGain == 0)
			{	// Use default:
				ampGain = 200;
			}
			dataUnits = sigInfo.units;
			if ((dataUnits == null) || (dataUnits.Length == 0))
			{	// Assign arbitrarily?
				dataUnits = "mV";
			}

			// Save a local handle:
			if (autoSend)
			{	// Point to the input data structure:
				initValues = iData;
			}
			else
			{	// Make our own data structure:
				initValues = new ChainInfo();
				initValues.samplingPeriodSec = iData.samplingPeriodSec;
			}

			if (autoSend && doneReading)	// We previously terminated the timer - restart it:
			{	// Init the TimerThread:
				sendTimer = new TimerThread(
					new System.Threading.ThreadStart(this.next),
					true);
			}

			// Init. what needs it:
			dispSamples = 0;		// just started, nothing retrieved yet
			doneReading = false;
			if (initValues.patientInfo == null)
			{
				initValues.patientInfo = new PatientInfo("", "", "");
			}
			if (initValues.dataInfo == null)
			{
				initValues.dataInfo = new DataInfo(sigInfo.desc, DateTime.Now.ToString());
			}
			// And, apply the REAL values:
			initValues.dataInfo.BitsPerSample = adcResBits;
			initValues.dataInfo.ScaleMultiplier = ampGain.ToString();
			if (adcResBits == 0)
			{	// Missing some info -- go with a default:
				initValues.dataInfo.FullScaleReferenceVoltage = "2.0";
				initValues.dataInfo.ZeroOffset = "0.0";
				initValues.dataInfo.ZeroReferenceVoltage = "-2.0";
			}
			else
			{
				initValues.dataInfo.FullScaleReferenceVoltage = ((adcMaxValue - baseline) / ampGain).ToString();
				initValues.dataInfo.ZeroOffset = baseline.ToString();
				initValues.dataInfo.ZeroReferenceVoltage = (-baseline / ampGain).ToString();
			}
			initValues.dataInfo.ValueUnits = dataUnits;

			if (initValues.fileInfo == null)
			{
				initValues.fileInfo = new FileHandler();
			}
			if (initValues.samplingPeriodSec != stepSize)
			{
				// Update the sampling period:
				initValues.samplingPeriodSec = stepSize;
			}

			//// Init. the sampling period:
			//this.stepSize = initValues.samplingPeriodSec;

			// Init output buffer:
			ReadBuffer();

			// Apply values and propagate downstream:
			base.init(iData);	// iData may have been changed or not
		}


		#endregion	Method overrides

		#region Public Methods & Variables

		/// <summary>
		/// Defines Forward and Backward directions, for (among others) the Skip method.
		/// </summary>
		public enum Direction
		{
			/// <summary>
			/// Forward direction
			/// </summary>
			Forward = 1,
			/// <summary>
			/// Backward direction
			/// </summary>
			Backward = 0
		}

		/// <summary>
		/// Releases all Wfdb-related resources.
		/// </summary>
		public void ReleaseWfdb()
		{	// Release as needed:
			WfdbAccess.CloseWfdb();
		}

		/// <summary>
		/// True if record reading is complete;  false otherwise.
		/// </summary>
		public bool recordComplete
		{
			get
			{
				return doneReading;
			}
		}

        /// <summary>
        /// The current record search path for the WFDB methods.  The default is
        /// typically ". /wfdb/database http://www.physionet.org/physiobank/database".
        /// NOTE:  Directory names cannot contain spaces, since the directories list
        /// is space-delimited.  However, directory names may be mapped to DOS 8.3 
        /// format if needed to get around this restriction.
        /// </summary>
        public static string WfdbPath
        {
            get
            {
                return WfdbAccess.GetSearchPath();
            }
            set
            {
                WfdbAccess.SetSearchPath(value);
            }
		}

		/// <summary>
		/// Go to the specified time position in the current open file.
		/// If the reader hasn't been init'd or it has completed reading the current file,
		/// then the method returns without doing anything.
		/// </summary>
		/// <param name="posSecs">Exact time in file to go to</param>
		public void GoTo(uint posSecs)
		{
			string errMsg = null;
			long samps = (long)(posSecs * sampFreq);
			// Now go to that position:
			GoToPositionInFile(samps, ref errMsg);
		}

		/// <summary>
		/// Skip, or jump, in the desired direction by the desired number of seconds.
		/// This will cause discontinuity in the data, with unknown effect on any
		/// downstream post-processing (peak detection, heart rate calculation, filtering,
		/// etc.).
		/// If the reader hasn't been init'd or it has completed reading the current file,
		/// then the method returns without doing anything.
		/// </summary>
		/// <param name="dir">Direction to skip (forward or backward)</param>
		/// <param name="secs">How many seconds to skip, up to all the way to the
		///			beginning (dir=Backward) or the end (dir=Forward)</param>
		public void Skip(Direction dir, uint secs)
		{
			string errMsg = null;
			long samps;

			// First of all:  have we inited yet?  Have we not finished yet?
			if ((numSamples == 0) ||	// Not initialized yet
				(doneReading))			// Already finished
			{	// Just ignore the command
				return;
			}

			// Convert secs to # of samples:
			samps = (long) (secs * sampFreq);
			// Trim according to current position and direction:
			if (dir == Direction.Forward)
			{
				// How much is left before the end?
				if ((numSamples - dispSamples) < samps)
				{	// Not enough left -- adjust samps:
					samps = numSamples - dispSamples - 1;	// -1 to allow at least 1 sample after the skip!
				}
				// Otherwise, no problem
				// Convert # of samples offset to absolute # of samples:
				samps += dispSamples;	// current position plus offset
			}
			else
			{
				// How far are we from the start?
				if (dispSamples < samps)
				{	// Not enough passed -- adjust samps:
					samps = dispSamples;	// Takes us back to the very beginning
				}
				// Otherwise, no problem
				// Convert # of samples offset to absolute # of samples:
				samps = dispSamples - samps;	// current position minus offset
			}
			// Now move that number of samples in the specified direction:
			GoToPositionInFile(samps, ref errMsg);
		}

		#endregion Public Methods & Values



	}
}
