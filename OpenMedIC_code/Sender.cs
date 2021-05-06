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
    /// Specifies the Data Sender standard for OpenMedIC Building Blocks.
	/// 
	/// A Sender is a component that transmits data to a Receiver (see also IReceiver).
	/// This class, like an interface, maintains daisy-chaining compatibility in
	/// components where it's relevant, and where it's not relevant it allows to break it
	/// deliberately by not implementing Sender.
	/// 
	/// A Sender specifies an init() method to initialize the component, and two methods
	/// to add and remove Followers to the Sender.  A Follower is an IReceiver.
	/// 
	/// Note that it is not unusual for a Sender to be also an IReceiver (see BuildingBlock).
	/// </summary>
	public abstract class Sender
	{
		/// <summary>
		/// Stores the initialization data that defined this run;  exposed as a read-only property.
		/// </summary>
		protected ChainInfo initVals = null;

		/// <summary>
		/// The number of seconds duration of a step.
		/// </summary>
		protected double stepPeriod;	// in seconds;  step period

		/// <summary>
		/// The array of followers to send data to.
		/// </summary>
		protected ArrayList followers = new ArrayList ();

		/// <summary>
		/// Do what is necessary for clean completion of the code.
		/// </summary>
		public virtual void Terminate ()
		{
		}

		/// <summary>
		/// Initializes the current object with whatever params it needs, and
		/// propagates the initialization to all its followers.
		/// </summary>
		/// <param name="iData">number of seconds between data samples, e.g. 1 millisec = 0.001</param>
		public virtual void init ( ChainInfo iData )
		{
			// Throw exception if null:
			if ( iData == null )
			{
				throw new ArgumentNullException ( "iData", "iData cannot be null!" );
			}
			// Throw exception if samplingPeriod is not reasonable:
			if ( iData.samplingPeriodSec <= 0.0 )	// Must be > 0!
			{
				throw new ArgumentOutOfRangeException ( "iData", iData, 
					"iData.samplingPeriodSec (the number of seconds between data samples) "
					+ "must be greater than zero!" );
			}

			// Retrieve what we need:
			stepPeriod = iData.samplingPeriodSec;
			// Save (a handle to) the whole thing:
			initVals = iData;	// NOTE:  THE TARGET DATA IS SUBJECT TO BEING CHANGED ELSEWHERE!

			IReceiver follower;
            for (int i = 0; i < followers.Count; i++)
            {
                try
                {
                    follower = (IReceiver)followers[i];
                }
                catch
                {   // An exception means, followers got changed at just the wrong time
                    // AND it no longer has an i(th) element
                    follower = null;
                }
                if (follower != null)
                {
                    follower.init ( iData );
                }
            }
			//lock ( followers )
			//{
			//	foreach ( object cur in followers )
			//	{
			//		follower = (IReceiver) cur;
			//		follower.init ( iData );
			//	}
			//}
		}

        /// <summary>
        /// Exposes the array of initialization values for all the channels
        /// </summary>
		public ChainInfo initDataValues
		{
			get
			{
				return initVals;
			}
		}

		/// <summary>
		/// Adds the given IReceiver to the list of followers, 
		/// avoiding duplicates.
		/// </summary>
		/// <param name="follower"></param>
		public void addFollower ( IReceiver follower )
		{
			//lock ( followers )
			//{
				if ( ! followers.Contains ( follower ) )
				{
					followers.Add ( follower );
				}
			//}
		}

		/// <summary>
		/// Removes the given follower, if present, from the list of followers.
		/// </summary>
		/// <param name="follower"></param>
		public void dropFollower ( IReceiver follower )
		{
			//lock ( followers )
			//{
				if ( followers.Contains ( follower ) )
				{
					followers.Remove ( follower );
				}
			//}
		}

		/// <summary>
		/// Copies the current list of Followers to the given ArrayList.
		/// </summary>
		/// <param name="Followers">List to which we add the current Followers</param>
		public void GetFollowers(ArrayList Followers)
		{
			lock (followers)
			{
				foreach (IReceiver ir in followers)
				{
					Followers.Add(ir);
				}
			}
		}

		/// <summary>
		/// propagates the Sample to all followers 
		/// </summary>
		/// <param name="newValue"></param>
		public virtual void sendValue(Sample newValue)
		{
			IReceiver follower;
			for (int i = 0; i < followers.Count; i++)
			{
				try
				{
					follower = (IReceiver)followers[i];
				}
				catch
				{   // An exception means, followers got changed at just the wrong time
					// AND it no longer has an i(th) element
					follower = null;
				}
				if (follower != null)
				{
					follower.addValue ( newValue );
				}
			}
			//lock ( followers )
			//{
			//	foreach ( object cur in followers )
			//	{
			//		follower = (IReceiver) cur;
			//		follower.addValue ( newValue );
			//	}
			//}
		}

		/// <summary>
		/// propagates the Samples to all followers 
		/// </summary>
		/// <param name="newValues">Note that the OLDEST sample is newValues[0]</param>
		public virtual void sendValues(Samples newValues)
		{
			IReceiver follower;
            for (int i = 0; i < followers.Count; i++)
            {
                try
                {
                    follower = (IReceiver)followers[i];
                }
                catch
                {   // An exception means, followers got changed at just the wrong time
                    // AND it no longer has an i(th) element
                    follower = null;
                }
                if (follower != null)
                {
                    follower.addValues(newValues);
                }
            }
 
		}

	}	// End of CLASS
}
