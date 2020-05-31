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
	/// Stores the data for a Waveform in a circular buffer, exposing methods for
	/// retrieving the data in formats that are convenient for the Display class.
	/// 
	/// The fact that a WaveformBuffer is a BuildingBlock means that it can be followed
	/// by other BuildingBlocks and IReceivers, such as filters, FileWriters, other
	/// WaveformBuffers, etc.
	/// </summary>
	public class WaveformBuffer : BuildingBlock
	{
		private CircularBuffer Buff;

        /// <summary>
        /// Constructor;  initializes the circular buffer used by this instance.
        /// </summary>
        /// <param name="BufferSize"></param>
		public WaveformBuffer( int BufferSize ) : base ()
		{
			// Initialize the Circular Buffer:
			Buff = new CircularBuffer ( BufferSize );
		}

		/// <summary>
		/// Adds the new Value to the Circular Buffer, then resumes default behavior
		/// </summary>
		/// <param name="newValue"></param>
		public override void addValue ( Sample newValue )
		{
			// Add value to circular buffer:
			this.Buff.AddPoint ( newValue );

			// Maintain default behavior:
			base.addValue ( newValue );
		}

		/// <summary>
		/// Adds the new Values to the Circular Buffer, then resumes default behavior
		/// </summary>
		/// <param name="newValues">Zero-based array of samples.
		///                         Note that the OLDEST sample is newValues[0]</param>
		public override void addValues ( Samples newValues )
		{
			// Add value to circular buffer:
			this.Buff.AddPoints ( newValues );

			// Maintain default behavior:
			base.addValues ( newValues );
		}

		/// <summary>
		/// Retrieves the desired data points and puts them in the passed Samples object.
		/// If the requested number of points is more than are available, it returns
		/// what it has.
		/// 
		/// Note that if StartingIndex is more than MaxSeconds in the past, only the last
		/// MaxSeconds worth of data is returned.
		/// </summary>
		/// <param name="Points">Object for data point storage</param>
		/// <param name="MaxSeconds">Max. number of seconds of data to return</param>
		/// <param name="StartingIndex">Starting index of data to return</param>
		/// <returns>The actual Starting index of the returned data.  This is the same 
		///				as StartingIndex if StartingIndex is no more than MaxSeconds 
		///				in the past.</returns>
		public long getPoints ( Samples Points, float MaxSeconds, long StartingIndex )
		{
			double wantedSeconds;	// number of seconds corresponding to the steps that
									// start at StartingIndex
			long startIndex = StartingIndex;

			// Sanity Checks:
			if ( MaxSeconds <= 0 )
			{
				throw new ArgumentOutOfRangeException ( "MaxSeconds", MaxSeconds,
					"MaxSeconds (" + MaxSeconds + ") must be greater than zero!" );
			}
			if ( StartingIndex < 0 )
			{
				throw new ArgumentOutOfRangeException ( "StartingIndex", StartingIndex,
					"StartingIndex (" + StartingIndex + ") cannot be less than 0!" );
			}

			// Determine whether to use MaxSeconds or StartingIndex:
			double absMaxSeconds = MaxSeconds;
			if (absMaxSeconds > (Points.maxSize * this.stepPeriod))
			{	// Can't fit MaxSeconds worth of samples -- adjust:
				absMaxSeconds = Points.maxSize * this.stepPeriod;
			}
			wantedSeconds = this.stepPeriod * ( this.Buff.CurrInputIndex - StartingIndex + 1 );
			if (wantedSeconds > absMaxSeconds)
			{
				// Must use absMaxSeconds -- calculate corresponding startIndex:
				startIndex = this.Buff.CurrInputIndex - (long)(absMaxSeconds / this.stepPeriod) + 1;
				// Final sanity check:
				if ( startIndex < 0 )
					startIndex = 0;
			}
			else
			{
				// startIndex is already set
			}

			// Retrieve the data:
			if ( this.Buff.CurrInputIndex < 0 )
			{
				// No data available:
				Points.size = 0;
			}
			else
			{
				this.Buff.GetPointsSince ( startIndex, Points );
			}

			// Return where the data actually started from:
			return startIndex;
		}


		/// <summary>
		/// Retrieves the desired data points and puts them in the passed Samples object.
		/// </summary>
		/// <param name="Points">Object for data point storage</param>
		/// <param name="MaxSeconds">Max. number of seconds of data to return;  if there are
		///					less than that number of seconds available, it returns only
		///					what is available.</param>
		/// <returns>The Starting index for the returned data.</returns>
		public long getPoints ( Samples Points, float MaxSeconds )
		{
			return this.getPoints ( Points, MaxSeconds, 0 );
		}

		/// <summary>
		/// The sample period (seconds per sample) of data acquisition.
		/// a 1 KHz sampling rate corresponds to a sample period of 0.001 seconds
		/// per sample.
		/// </summary>
		public double samplePeriod
		{
			get
			{
				return stepPeriod;
			}
		}

		/// <summary>
		/// Clears the buffer to an empty state
		/// </summary>
		public void Clear()
		{
			Buff.Reset();
		}


        /// <summary>
        /// The size of the current WaveformBuffer, i.e. the number of samples it
        /// is capable of containing.
        /// </summary>
        public int BufferSize
        {
            get
            {
                if (Buff == null)
                {
                    return 0;
                }
                else
                {
                    return Buff.BuffLength;
                }
            }
        }


        /// <summary>
        /// The index (a.k.a. sample number) of the most recently added sample in the
        /// Waveform Buffer.
        /// Note that this value continues ad infinitum, wrapping back to zero only if
        /// the storage capacity of a signed long is exceeded 
        /// (2**63 - 1 = 9,223,372,036,854,775,807).
        /// </summary>
        public ulong CurrentInputIndex
        {
            get
            {
                if (Buff == null)
                {
                    return 0;
                }
                else if (Buff.CurrInputIndex < 0)
                {
                    return 0;
                }
                else
                {
                    return (ulong)Buff.CurrInputIndex;
                }
            }
        }


        /// <summary>
        /// The number of valid samples in the current WaveformBuffer.
        /// </summary>
        public int NumSamples
        {
            get
            {
                if (Buff == null)
                {
                    return 0;
                }
                else
                {
                    if (Buff.BuffLength >= Buff.CurrInputIndex)
                    {
                        return (int)Buff.CurrInputIndex;
                    }
                    else
                    {
                        return Buff.BuffLength;
                    }
                }
            }
        }

	}	// END of class WaveformBuffer
}
