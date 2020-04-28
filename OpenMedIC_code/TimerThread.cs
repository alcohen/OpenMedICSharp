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
using System.Threading;

namespace OpenMedIC
{
	/// <summary>
	/// This class is used to start a Timer in a separate thread.  The thread waits
	/// for a specified amount of time (interval), then executes a specific method.
	/// 
	/// Unless the TimerThread is created with blocking = true, the executed 
	/// method runs in a separate thread from the Timer;  this means that, if the 
	/// method takes longer than [interval] to execute, it will be called again
	/// before the first call has completed.
	/// 
	/// The method called by the Timer at the end of each interval is set
	/// when the Timer is created.
	/// Note that you cannot change the target method; to do so, you must create
	/// a new Timer.  This is to avoid serious confusion, since the target method
	/// really defines the Timer.
	/// 
	/// The Timer's interval (in microseconds) is set when started, and can be
	/// changed by stopping then restarting the timer.
	/// </summary>
	public class TimerThread
	{
		private long intervalMsec;
		private long intvlSecs;
		private int intvlRemain;

		private ThreadStart callbackMethod;
		private Thread mainThread;

		private bool block;

		/// <summary>
		/// True if the timer is running, regardless of whether it's paused or not
		/// </summary>
		private bool isRunning = false;
		/// <summary>
		/// True if the timer is paused;  it usually implies isRunning = true
		/// </summary>
		private bool isPaused = false;
		/// <summary>
		/// True if the timer must stop as soon as possible.  This is used to let
		/// currently-spawned threads know that they must go away ASAP.
		/// </summary>
		private bool mustStop = false;

		/// <summary>
		/// Sets the callback function for this thread.  a ThreadStart is obtained by calling:
		/// "new System.Threading.ThreadStart ( myObject.myMethod );", as in:
		/// <code>
		/// ...
		/// myObj = new thisObj ();
		/// myTimer = new System.Threading.ThreadStart ( new ThreadStart(myObj.myMethod) );
		/// ...
		/// </code>
		/// </summary>
		/// <param name="callbackFn">A valid ThreadStart (see comment for the Constructor)</param>
		/// <param name="blocking">if true, then the timer pauses during execution of the method,
		///					then starts the next interval only after the method has completed.
		///					If false, the timer starts the next interval during execution, which 
		///					means that the delay between the end of one execution and the start 
		///					of the next will be less than the specified interval, and can also
		///					be negative (i.e. overlap of method executions).</param>
		public TimerThread( ThreadStart callbackFn, bool blocking )
		{
			callbackMethod = callbackFn;
			mainThread = new Thread ( new ThreadStart(this.Loop) );

			block = blocking;
		}

		/// <summary>
		/// Releases any resources that need releasing for proper shut-down
		/// </summary>
		~TimerThread ()
		{
			Release();
		}

		/// <summary>
		/// Releases any resources that need releasing for proper shut-down
		/// </summary>
		public void Release ()
		{
			// Stop and release stuff:
			mustStop = true;
			if ( isPaused )
			{
				// Current thread is paused -- must restart for it to terminate cleanly:
				this.Resume ();
			}
			mainThread = null;
			callbackMethod = null;
		}


		private void SetInterval(long intervalMillisecs)
		{
			// Sanity checks:
			if (this.block && (intervalMillisecs < 0))
			{
				throw new ArgumentOutOfRangeException("intervalMillisecs",
					intervalMillisecs,
					"The interval between executions for a blocking Timer "
						+ "cannot be less than 0.");
			}
			else if (!this.block && (intervalMillisecs < 1))
			{
				throw new ArgumentOutOfRangeException("intervalMillisecs",
					intervalMillisecs,
					"The interval between executions for a non-blocking Timer "
					+ "cannot be less than 1 millisecond.");
			}

			intervalMsec = intervalMillisecs;
			// Decrement the interval count to account for the millisec spent between checks:
			if (block && intervalMsec > 0)
				intervalMsec--;
			// Calculate sub-second and whole-second components for faster response:
			intvlRemain = (int)(intervalMsec % 1000);
			intvlSecs = (intervalMsec - intvlRemain) / 1000;
		}

		/// <summary>
		/// Starts the timer.  Note the the first execution of the callback Function occurs
		/// only after one interval.
		/// 
		/// If the timer is already running, this command is ignored.
		/// </summary>
		/// <param name="intervalMillisecs">Interval between executions</param>
		public void Start ( long intervalMillisecs )
		{
			if ( ! this.isRunning )
			{
				SetInterval(intervalMillisecs);

				mustStop = false;
				isRunning = true;
				isPaused = false;
				mainThread.Start ();
			}
		}

		/// <summary>
		/// Changes the interval between triggers for a RUNNING timer.  If the timer is not
		/// running already, nothing happens.
		/// Note that the currently-running interval may not be affected.
		/// </summary>
		/// <param name="intervalMillisecs">New desired interval between executions</param>
		public void UpdatePeriod(long intervalMillisecs)
		{
			if (this.isRunning)
			{
				SetInterval(intervalMillisecs);
				// ...
			}
		}

		/// <summary>
		/// Stops the timer, if it is running.  The same timer can be restarted 
		/// by calling Start().
		/// </summary>
		public void Stop ()
		{
			if ( isRunning )
			{
				mustStop = true;
				isRunning = false;
			}
		}

		/// <summary>
		/// Pauses execution of the timer.  When resuming, the timer 
		/// will start again from where it left off.
		/// </summary>
		public void Pause ()
		{
			if (mainThread == null)
			{
				return;
			}
			try
			{
				lock ( mainThread )
				{
					if ( ! isPaused && ! mustStop )
					{
						isPaused = true;
                        mainThread.Suspend ();
					}
				}
			}
			catch (Exception suspendException)
			{
				while (suspendException.InnerException != null)
				{	// Get to the core exception:
					suspendException = suspendException.InnerException;
				}
				System.Windows.Forms.MessageBox.Show("Warning:  " + suspendException.Message);
			}
		}

		/// <summary>
		/// Resumes execution of the timer.  The timer will start again from where it
		/// left off, EXCEPT that the period between ticks may or may not be honored
		/// for the interval during which the timer is paused.
		/// </summary>
		public void Resume ()
		{
			if (mainThread == null)
			{
				return;
			}
			try
			{
				lock ( mainThread )
				{
					if ( isPaused )
					{
						isPaused = false;
						mainThread.Resume ();
					}
				}
			}
			catch (Exception resumeException)
			{
				while (resumeException.InnerException != null)
				{	// Get to the core exception:
					resumeException = resumeException.InnerException;
				}
				System.Windows.Forms.MessageBox.Show("Warning:  " + resumeException.Message);
			}
		}

		/// <summary>
		/// Sleeps for the specified sleep time, EXCEPT that it breaks up intervals
		/// greater than one second into the appropriate number of 1-second intervals
		/// followed by the difference, to contain the max. latency when stopping the
		/// thread.
		/// </summary>
		private void SleepTime ()
		{
			for (int i = 0; i < intvlSecs; i++)
			{
				if ( mustStop )
					break;
				Thread.Sleep ( 1000 );
			}
			if ( ! mustStop )
				Thread.Sleep ( this.intvlRemain );

		}

        /// <summary>
        /// This is the main loop executed by mainThread.
        /// Its job is to start a new worker thread then wait for it to be done,
        /// then sleep for the specified amount of time before starting another new 
        /// worker thread.
        /// </summary>
		protected void Loop ()
		{

			while ( ! mustStop )
			{
				this.SleepTime ();
				// Make sure the timer wasn't stopped while we slept:
				if ( ! mustStop )
				{
					Thread executor = new Thread ( callbackMethod );
					executor.Start ();
					if ( this.block )
					{
						// Wait for thread to finish before continuing:
						do
						{
							Thread.Sleep ( 1 );
						}	while ( ! mustStop && executor.IsAlive );
						// Note:  a very fast execution will return within 1 millisec,
						// which is why we decremented the wait for blocking Timers.
					}
				}

			}
		}

	}
}
