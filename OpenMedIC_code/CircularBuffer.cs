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
	/// A circular buffer of Samples.  Handles wrap-around and multi-threading appropriately.
	/// </summary>
	public class CircularBuffer
	{
		Samples buffer;			// Stores the circular buffer data.

		Samples outputBuffer;	// Used to return data when requested.

		/// <summary>
		/// Create a new CircularBuffer object of the specified size (length).
		/// </summary>
		/// <param name="Length">Maximum number of samples that will be storable in this
		///					CircularBuffer.</param>
		public CircularBuffer(int Length)
		{
			buffer = new Samples (Length);	// Note that Samples.size is initialized to the full size
			outputBuffer = new Samples (Length);	// Note that Samples.size is initialized to the full size
			hasData = false;
		}

		/// <summary>
		/// Set to false before until data is added
		/// </summary>
		private bool hasData;

		/// <summary>
		/// The index of the next waveform element to be written; 
		/// keeps growing, does not roll back to zero when 
		/// the last element of buffer is passed
		/// </summary>
		private long nextInputIndex;

		/// <summary>
		/// The index of the most recent waveform element to be written; 
		/// keeps growing, does not roll back to zero when 
		/// the last element of buffer is passed
		/// Returns -1 if there is no data yet
		/// </summary>
		public long CurrInputIndex
		{
			get
			{
				if (!hasData)
				{
					return -1;
				}
				else if (nextInputIndex == 0)
				{
					lock ( buffer )
						return buffer.size - 1;
				}
				else
				{
					return nextInputIndex - 1;
				}
			}
		}

		/// <summary>
		/// The index of the next buffer element to be written; 
		/// Rolls back to zero when 
		/// the last element of buffer is passed
		/// </summary>
		private int nextBufferIndex;

		/// <summary>
		/// Adds a single point to the end of the buffer
		/// </summary>
		/// <param name="point">Point being added</param>
		public void AddPoint ( Sample point )
		{
			lock ( this )
			{
				// Copy over the value:
				buffer[nextBufferIndex].copyFrom ( point );

				OpenMedICUtils.debugPrint ( "Added a point with value " + point.sampleValue
										+ " at position " + nextBufferIndex );

				// Update nextBufferIndex, WITH WRAPPING:
				if (nextBufferIndex == (buffer.size - 1))
				{
					nextBufferIndex = 0;
				}
				else
				{
					nextBufferIndex++;
				}
				// update next input index:
                nextInputIndex++;
                if (nextInputIndex < 0)
                {   // Overflowed and wrapped around:
                    nextInputIndex = 0; // This is the only practical thing to do...
                }
				hasData = true;
			}
		}

		/// <summary>
		/// Adds an array of data points to the buffer
		/// </summary>
		/// <param name="Points">Array of floats to add</param>
		public void AddPoints(Samples Points)
		{
			//add the points to the circular buffer
			if (Points.size > 0)
			{
				lock ( this )
				{
					// Cycle through the input array:
					for (int i = 0; i < Points.size; i++)
					{
						this.AddPoint ( Points[i] );
					}
				}
			}
		}

		/// <summary>
		/// Clears the buffer and resets it to a newly-initialized state
		/// </summary>
		public void Reset()
		{
			lock ( this )
			{
				nextInputIndex = 0;
				nextBufferIndex = 0;
				hasData = false;
			}
		}

		/*
		/// <summary>
		/// Returns the NumLatestPoints most recent points
		/// </summary>
		/// <param name="LatestPoints">Array in which latest points are passed back</param>
		/// <param name="NumLatestPoints">Number of points passed back</param>
		public void GetLatestPoints(Samples LatestPoints)
		{
			//test to see if we're being asked to return too many points
			if (LatestPoints.size > buffer.size)
			{
				//too many points; throw an exception
				throw (new ArgumentOutOfRangeException("The size of LatestPoints ("
					+ LatestPoints.size + ") greater than circular buffer length (" 
					+ buffer.size + ")."));
			}
		}
		*/

		/// <summary>
		/// Returns all points added to circular buffer since a given InputIndex
		/// </summary>
		/// <param name="SinceIndex">Return points after this InputIndex</param>
		/// <param name="PointsSince">Array in which points are passed back</param>
		public void GetPointsSince ( long SinceIndex, Samples PointsSince )
		{
			int MaxCount = this.BuffLength; // (int)(nextInputIndex - SinceIndex) - 1;
			GetPointsSince ( SinceIndex, MaxCount, PointsSince );
		}

		/// <summary>
		/// Returns up to MaxCount points added to circular buffer since a given InputIndex
		/// </summary>
		/// <param name="SinceIndex">Return points starting at this InputIndex</param>
		/// <param name="MaxCount">Max. number of points to return; actual returned points may be less!</param>
		/// <param name="PointsSince">Array in which points are passed back</param>
		public void GetPointsSince ( long SinceIndex, int MaxCount, Samples PointsSince )
		{
			int NumPointsSince;	// number of points being returned

			lock ( this )	// To avoid the buffer changing while we are reading it!
			{
				//test for index
				if (SinceIndex < 0)
				{
					//Invalid SinceIndex; throw an exception
					throw (new ArgumentOutOfRangeException("SinceIndex", SinceIndex, "Invalid SinceIndex: "
						+ SinceIndex.ToString() + ". Must be 0 or greater."));
				}

				//test to see if SinceIndex is too high
				if (SinceIndex > CurrInputIndex + 1)
				{
					//Bad index; throw an exception
					throw (new ArgumentOutOfRangeException("SinceIndex", SinceIndex, "SinceIndex ("
						+ SinceIndex.ToString() + ") greater than current signal index + 1 (" 
						+ (CurrInputIndex + 1).ToString() + ")."));
				}
		
				NumPointsSince = (int)(nextInputIndex - SinceIndex);

				//test to see if we're being asked to return too many points
				if ( NumPointsSince > buffer.size )
				{
					// Too many points;  must truncate:
					NumPointsSince = buffer.size;
					SinceIndex = nextInputIndex - NumPointsSince;
					////too many points; throw an exception
					//throw (new ArgumentOutOfRangeException("SinceIndex", SinceIndex, "Starting point ("
					//    + SinceIndex + ") implies a range of samples (" + NumPointsSince 
					//    + ") greater than circular buffer length (" + buffer.size + ")."));
				}
                if (NumPointsSince > PointsSince.maxSize)
				{
					//too many points; throw an exception
					throw (new ArgumentOutOfRangeException("MaxCount", MaxCount, "Requesting more points ("
                        + NumPointsSince + ") greater than passed buffer max. size (" + PointsSince.maxSize + ")."));
				}

				//Inputs are OK
				MaxCount = Math.Min ( NumPointsSince, MaxCount );

				OpenMedICUtils.debugPrint ( "Returning " + MaxCount + " points starting from " + SinceIndex );

				//Let's get the points!
				PointsSince.size = MaxCount;
				int BuffIndex; //points to the index of the circular buffer that we're operating on
				for ( int i = 0; i != MaxCount; i++ )
				{
					BuffIndex = (int)( (SinceIndex + i) % buffer.size );
					PointsSince[i].copyFrom ( buffer[BuffIndex] );

					OpenMedICUtils.debugPrint ( " - Returning point at index " + BuffIndex + ": value = " + PointsSince[i].sampleValue );
				}
			}

		}

		/// <summary>
		/// Retrieves a single point that is backOffset samples in the past.
		/// </summary>
		/// <param name="backOffset">Number of samples back; 0 equals "last sample added"</param>
		/// <returns>The appropriate Sample</returns>
		public Sample getPoint ( int backOffset )
		{
			if ( backOffset < 0 || backOffset >= buffer.size )
			{
				throw new ArgumentOutOfRangeException ( "backOffset", backOffset, 
					"Parameter out of range (" + backOffset 
					+ "): allowed values are between 0 (last sample added) and buffer size -1 ("
					+ (buffer.size - 1) + ") (oldest sample available)" );
			}
			int BuffIndex = nextBufferIndex - backOffset - 1;

			if ( BuffIndex < 0 )
				BuffIndex = BuffIndex + buffer.size;

            return buffer[BuffIndex]; //[nextBufferIndex];
		}

		/// <summary>
		/// Length of circular buffer
		/// </summary>
		public int BuffLength
		{
			get
			{
				return buffer.size;
			}
		}

	}
}
