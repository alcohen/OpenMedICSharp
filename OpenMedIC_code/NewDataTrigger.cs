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
	/// This class implements an IReceiver, for the purpose of triggering a method 
	/// (delegate) call when there is new data coming through, and thus being able
	/// to do without other timers.
	/// This would normally be used to drive a display refresh and/or trigger a 
	/// DataSource created with autoOutput = false.
	/// 
	/// Usage:
	/// <code>
	///		NewDataTrigger myTrigger = new NewDataTrigger 
	///			( new NewDataTrigger.refreshDelegate ( [function to call] ),
	///			  true, true );
	///		[Any Sender].addFollower ( myTrigger );
	/// </code>
	/// </summary>
	public class NewDataTrigger:IReceiver
	{

		/// <summary>
		/// The minimum amount of time between refreshes
		/// </summary>
		public const int minRefreshMSec	= 20;

		/// <summary>
		/// Delegate to be used for new NewDataTrigger instance creation
		/// </summary>
		public delegate void refreshDelegate ();

		private static TimeSpan minRefreshSpan;
		
		private DateTime lastRefresh;

		private refreshDelegate myRefresher;

		private bool addValueTrigger;
		private bool addValuesTrigger;

		/// <summary>
		/// Creates a new instance with the appropriate parameters to run.
		/// </summary>
		/// <param name="refresher">Delegate of the method to be called for a trigger</param>
		/// <param name="triggerOnAddValue">If TRUE, then refresher is called when addValue(Sample) is called</param>
		/// <param name="triggerOnAddValues">If TRUE, then refresher is called when addValues(Samples) is called</param>
		public NewDataTrigger( refreshDelegate refresher,
			bool triggerOnAddValue, bool triggerOnAddValues )
		{
			minRefreshSpan = new TimeSpan ( 0, 0, 0, 0, minRefreshMSec );

			lastRefresh = DateTime.Now;

			myRefresher = refresher;

			addValueTrigger  = triggerOnAddValue ;
			addValuesTrigger = triggerOnAddValues;
		}

		/// <summary>
		/// Initialize myself.
		/// </summary>
		/// <param name="iData"></param>
		public void init ( ChainInfo iData )
		{
			lastRefresh = DateTime.Now;
		}

		/// <summary>
		/// A new sample is available, and it must be processed and/or propagated
		/// to the followers as appropriate.
		/// </summary>
		/// <param name="newValue"></param>
		public void addValue ( Sample newValue )
		{
			if ( this.addValueTrigger )
			{
				// Trigger a refresh
				doRefresh ();
			}
		}

		/// <summary>
		/// A number of new samples are available, and they must be processed 
		/// and/or propagated to the followers as appropriate.
		/// </summary>
		/// <param name="newValues">Zero-based array of samples.
		///                         Note that the OLDEST sample is newValues[0]</param>
		public void addValues ( Samples newValues )
		{
			if ( this.addValuesTrigger )
			{
				// Trigger a refresh
				doRefresh ();
			}
		}

		private void doRefresh ()
		{
			if (  (TimeSpan)(DateTime.Now - lastRefresh) > minRefreshSpan )
			{
				lastRefresh = DateTime.Now;

				this.myRefresher();
			}
		}

	}	// END of class
}
