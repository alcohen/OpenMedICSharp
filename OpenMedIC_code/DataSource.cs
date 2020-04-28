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
using System.Threading;

namespace OpenMedIC
{
	/// <summary>
	/// An abstract class for any source of data, such as:
	/// 
	/// -&gt; Function Generator;
	/// -&gt; File Reader;
	/// -&gt; Data Acquisition.
	/// 
	/// NOTE:  If this is used with autoOutput = TRUE, you MUST call the Terminate()
	/// method when done otherwise there is no guarantee that the class will unload 
	/// properly.
	/// </summary>
	public abstract class DataSource : Sender
	{
		/// <summary>
		/// Number of ticks in a second;  1 tick = 100 nanoseconds => 1 second = 10 million ticks
		/// </summary>
		protected const double ticksPerSecond = 10 * 1000 * 1000;	

		/// <summary>
		/// Minimum timer interval period;  if data samples have a shorter interval than this,
		/// then the timer is set to a default interval and multiple samples will be generated 
		/// for each timer tick.
		/// </summary>
		public const int minTimerIntervalMSec = 20;

		/// <summary>
		/// Store the last step for which we generated a value.
		/// This value should be reset appropriately for any cyclical function.
		/// </summary>
		protected long lastStep = 0;	// Init. to 0 (pre-first-step)
		/// <summary>
		/// Conversion of the step period in "ticks" (internal Windows time format)
		/// </summary>
		protected double ticksPerStep;	// step period in ticks (1 tick = 100 nsec)

		/// <summary>
		/// when the last sample was output, as a DateTime
		/// </summary>
		protected DateTime lastOutput;	// When the last sample was output

		/// <summary>
		/// Whether to automatically output data when it's available, 
		/// or wait for a trigger() or getNextValue[s](...)
		/// </summary>
		protected bool autoSend = false;

		/// <summary>
		/// Tracks when an autoSend DataSource is paused.
		/// </summary>
		private bool isPaused = false;

		/// <summary>
		/// A variable that can be set to interrupt data outputting, presumably because the data
		/// source ran out of actual data, possibly in the middle of a block being sent (or requested).
		/// </summary>
		protected bool dataFinished = false;

        /// <summary>
        /// Thread used to trigger sending of data at specific intervals
        /// </summary>
		protected TimerThread sendTimer;

		/// <summary>
		/// Default Constructor for DataSource.
		/// If autoOutput, then this object will output one Sample every secondsPerStep
		/// to its Followers;  if not autoOutput, then it will only generate output when
		/// requested by a call to trigger() or getNextValue() or getNextValues(int).
		/// </summary>
		/// <param name="secondsPerStep">Interval between samples, in seconds or fraction thereof,
		///					e.g. 1 KHz = 1 ms per sample would have secondsPerStep = 0.001</param>
		/// <param name="autoOutput">If true, then we push data out to the Followers;
		///					if false, we wait for whoever to pull out the data</param>
		public DataSource( double secondsPerStep, bool autoOutput )
		{
			// Set the interval:
			stepSize = secondsPerStep;

			// Set whether we output on a push or pull scheme:
			autoSend = autoOutput;
			if ( autoSend )
			{
				// Init the TimerThread:
				sendTimer = new TimerThread ( 
					new System.Threading.ThreadStart(this.next),
					true);		// true  -> wait for one call to complete before starting the next one
				//	false );	// false -> do NOT wait for one call to complete before starting the next one
								//			(this can cause overlaps and long queues of waiting threads if the
								//			 code execution should halt for any reason, such as a msg. box or
								//			 a debugger breakpoint).
			}

			// Make sure we init. this, otherwise we risk BIG problems!
			// Note:  each subclass should re-init lastOutput to what makes sense
			// for that class.
			lastOutput = DateTime.Now;
			this.lastStep = 0;
			this.lastOutput = DateTime.Now;
		}

		/// <summary>
		/// Destructor.  DO NOT COUNT ON THIS for clean-up!
		/// </summary>
		~DataSource ()
		{
			this.Terminate ();
		}

		/// <summary>
		/// Initialize the data run with the appropriate info/parameters.
		/// Note that a class should tolerate multiple init(...) calls without being destroyed
		/// then re-instantiated, and there should be nothing carried over from a previous run
		/// after init(...) has returned.
		/// </summary>
		/// <param name="iData">An object containing all necessary/relevant data for this
		/// data run.</param>
		public override void init ( ChainInfo iData )
		{
			base.init ( iData );
			// Re-Init the two progress indicators:
			lastStep = 0;
			lastOutput = DateTime.Now;

			if ( autoSend )
			{
				// Start the TimerThread now:
				// - If the step period is less than 0.02 seconds, then only execute
				//	every 0.02 seconds, to avoid overloading the system:
				if ( (stepPeriod * 1000.0) < minTimerIntervalMSec )
				{
					sendTimer.Start ( minTimerIntervalMSec );	// 20 ms. = 0.02 sec. = 50 Hz
				}
				else
				{
					sendTimer.Start ( (long)(this.stepPeriod * 1000) );
				}
			}
		}

		/// <summary>
		/// Terminates timer thread(s) as necessary for clean completion of the code.
		/// </summary>
		public override void Terminate ()
		{
			if ( autoSend )
				this.sendTimer.Release ();
		}

		/// <summary>
		/// Stops data acquisition/generation until resume() is called.
		/// Calls to pause() while the DataSource is already paused are ignored.
		/// 
		/// NOTE that this only applies to autoOutput mode;  in non-autoOutput mode this
		/// method is ignored.
		/// </summary>
		public void pause ()
		{
			if ( autoSend )
			{
				// Lock to prevent ugliness with the isPaused flag:
				lock ( sendTimer )
				{
					// Wait up to 0.1 seconds for isPaused to be false, in case
					// something else temporarily had it on:
					for ( int i = 0; i < 10; i++ )
					{
						if ( ! isPaused )
							break;
						Thread.Sleep ( 10 );
					}
					if ( ! isPaused )
					{
						isPaused = true;
						sendTimer.Pause ();
					}
				}
			}
		}

		/// <summary>
		/// Resumes data acquisition/generation after a pause ().
		/// If a pause () was not called previously then this call is ignored.
		/// Calls to resume() while not paused will be ignored.
		/// 
		/// NOTE that this only applies to autoOutput mode;  in non-autoOutput mode this
		/// method is ignored.
		/// </summary>
		public void resume ()
		{
			if ( autoSend )
			{
				// Lock to prevent ugliness with the isPaused flag:
				lock ( sendTimer )
				{
					// Wait up to 1 second for isPaused to be true, in case
					// something else temporarily had it off:
					for ( int i = 0; i < 100; i++ )
					{
						if ( isPaused )
							break;
						Thread.Sleep ( 10 );
					}
					if ( isPaused )
					{
						isPaused = false;
						sendTimer.Resume ();
					}
				}
			}
		}

		/// <summary>
		/// Gets the interval from now to the last time the trigger was called
		/// ( OR from init() ) and outputs the appropriate number of samples.
		/// 
		/// Works by outputting to each Follower the result of calling NextVal().
		/// </summary>
		public virtual void trigger ()
		{
			if (!isPaused)
			{
				Samples newVals = null;		// Gets populated, and used, only if appropriate
				int sampleCount;
				lock (this)
				{
					// Find out if we have anything to retrieve:
					sampleCount = hasNextVal();
					if (sampleCount < 1)
					{
						// Nothing to do
					}
					else
					{
						// Get new values:
						newVals = new Samples(sampleCount);
						for (int i = 0; i < sampleCount; i++)
						{
							// Sanity check:
							if (dataFinished)
							{	// No more data -- do some adjustments:
								sampleCount = i;	// Last valid value was previous one
								newVals.size = sampleCount;
								break;
							}
							else
							{
								getNextValCertified(newVals[i]);
							}
						}
						// Propagate:
						this.sendValues(newVals);
					}
				}	// END lock
			}
		}


		/// <summary>
		/// Generates the next sample(s), then outputs it/them to the followers.
		/// 
		/// If the next sample is not available yet, then this method does nothing.
		/// </summary>
		public void next ()
		{
			this.trigger();
		}

		/// <summary>
		/// Step size, in seconds.
		/// A data source that generates one sample per millisecond would have 
		/// a step size of 0.001 (1/1000).
		/// 
		/// NOTE:  stepSize will be rounded to the nearest 100 nanoseconds.
		/// </summary>
		public double stepSize
		{
			set
			{
				// Modify stepPeriod to be an integer number of ticks:
				stepPeriod = (double) ( (long)(value * ticksPerSecond) / ticksPerSecond );
				// NOTE:  this guarantees that converting steps to ticks and back will be lossless!
				ticksPerStep = stepPeriod * ticksPerSecond;
			}
			get
			{
				return stepPeriod;
			}
		}

		/// <summary>
		/// Generates and returns the next function value to be returned, IF there is one.
		/// If there is no new value and wait = true, it waits for the next value then returns
		/// it;  otherwise it leaves val unchanged and returns FALSE.
		/// </summary>
		/// <param name="val">Sample whose value to set with the value to return</param>
		/// <param name="wait">If TRUE, then the function waits for data if none is
		///						currently available;  if FALSE and no data is available,
		///						then it returns FALSE and leaves val unchanged</param>
		/// <returns>TRUE if a value was set;  FALSE otherwise</returns>
		public bool getNextValue ( Sample val, bool wait )
		{
			lock ( this )
			{
				if ( ! wait && hasNextVal() < 1 )
				{
					// We don't have a next value, and caller is not willing to wait for one:
					return false;
				}
				else
				{
					// Wait if appropriate, then get/return next value:
					this.nextValReady (wait);
					getNextValCertified ( val );
					return true;
				}
			}
		}

		/// <summary>
		/// This is the only official way of retrieving values from nextVal ();
		/// that's because it keeps stuff in sync that would otherwise be done by
		/// various different places.
		/// </summary>
		/// <param name="val">Value to be populated</param>
		private void getNextValCertified ( Sample val )
		{
			lock ( this )
			{
				val.sampleValue = nextVal ();
				// Update the last-sampled timestamp (go back to last whole sample):
				lastOutput += TimeSpan.FromTicks( (long)ticksPerStep );
			}
		}

		/// <summary>
		/// Generates and returns the next available function values up to vals.size.
		/// If there are less than that many values available (including 0), it returns 
		/// the available values and adjusts vals.size;  if there are at least that many
		/// new values available, then it returns exactly vals.size values.
		///	
		///	The returned array has Sample[0] as the OLDEST sample;  see also 
		///	IReceiver.addValues(Samples).
		/// </summary>
		/// <param name="vals">Samples array, with .size set to the max. number of
		///			samples desired.</param>
		public void getNextValues ( Samples vals )
		{
			bool hasNext;

			lock ( this )
			{
				int actAvail = hasNextVal ();

				// Sanity check:
				if ( actAvail < vals.size )
					vals.size = actAvail;

				for ( int i = 0; i < vals.size; i++ )
				{
					hasNext = this.getNextValue ( vals[i], false );
					if ( ! hasNext )
					{
						// ran out of values to return!  Exception:
						throw new ApplicationException ( "????" );
					}
				}
			}	// END lock
		}

		/// <summary>
		/// Determines whether data is output based on internal parameters or 
		/// should wait for external input.
		/// 
		/// Internal parameters include:
		/// 
		/// -&gt; data periodicity (e.g., a new sample is available);
		///	-&gt; external data source has a new sample;
		///	-&gt; local buffer (nearly?) full.
		///	
		///	External input means a call to one of:
		///	
		/// -&gt; trigger();
		///	-&gt; getNextValue();
		///	-&gt; getNextValues(int).
		/// 
		/// Note that, even if autoOutput = TRUE, calls to the "external input" methods 
		/// will still work, but if there is no data, getNextValue(true) will hang until 
		/// the next value becomes available.
		/// </summary>
		public bool autoOutput
		{
			get
			{
				return autoSend;
			}
		}

		/// <summary>
		/// This actually generates the next value in the sequence.
		/// </summary>
		/// <returns>The next available value</returns>
		protected abstract float nextVal ();

		/// <summary>
		/// Determines whether there is a next value ready for output:
		/// 
		/// -&gt; If there is a next value available, it returns TRUE;
		///	-&gt; If wait = TRUE  and there is no new value, it waits until a value is available;
		///	-&gt; If wait = FALSE and there is no new value, it returns FALSE.
		/// 
		/// Waiting is determined on how much time must elapse before the next sample is 
		/// available;  the method waits for that period then checks again for availability,
		/// and gets into a very short delay loop waiting.
		/// </summary>
		/// <param name="wait">Determines behavior if a value is not available.  If TRUE, then
		///				the method waits for the next value to be available then returns TRUE;
		///				otherwise, it returns immediately FALSE.</param>
		/// <returns>-&gt; If there is a next value available, it returns TRUE;
		///			 -&gt; If wait = TRUE  and there is no new value, it waits until a value is available;
		///			 -&gt; If wait = FALSE and there is no new value, it returns FALSE.
		///	</returns>
		protected virtual bool nextValReady ( bool wait )
		{
			if ( hasNextVal () > 0 )
				return true;
			else if ( ! wait )
				return false;
			else
			{
				// Must actually wait this one out
				// - How much time has already elapsed?
				TimeSpan timeDone = (System.TimeSpan)(DateTime.Now - this.lastOutput);
				// - How much time in a sample?
				TimeSpan sampleDuration = new TimeSpan ( (long) Math.Ceiling(this.ticksPerStep) );
				// - Get the difference:
				TimeSpan timeLeft = sampleDuration - timeDone;
				if ( timeLeft > TimeSpan.Zero )
				{
					Thread.Sleep ( timeLeft );
				}
				// Now a tight little loop that waits for hasNextVal() to return true:
				int msecPerStep = (int) Math.Ceiling( 
								( (double)ticksPerStep * 1000.0 ) / ticksPerSecond );
				for ( int i = 0; i < msecPerStep ; i++ )
				{
					if ( hasNextVal () > 0 )
					{
						// All set -- return:
						return true;
					}
					// Not available yet -- sleep:
					Thread.Sleep ( 1 );
				}
				// If we got here, something is stuck!!!!!
				throw new ApplicationException ( 
					"This Data Source seems to be stuck and is no longer "
					+ "outputting data at the specified rate!" );
			}
		}

		/// <summary>
		/// Determines how many next values we have to return.
		/// If this returns 1 or more, then a call to getNextVal(FALSE) should always 
		/// return a meaningful value;  if this returns 0, then a call to 
		/// getNextVal(TRUE) should be expected to wait for some time before returning.
		/// </summary>
		/// <returns>the number of values ready to be returned, if any.  Never returns
		///			less than 0</returns>
		protected virtual int hasNextVal ()
		{

			// Get the number of samples currently available:
			TimeSpan elapsed = (System.TimeSpan)(DateTime.Now - this.lastOutput);
			int sampleCount = (int) Math.Floor ( (double)elapsed.Ticks / this.ticksPerStep );
			// If we're way off in the future, prevent negative-number returns:
			if ( sampleCount < 0 )
				sampleCount = 0;

			return sampleCount;
		}

	}	// END OF class
}
