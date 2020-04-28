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
    /// Specifies the Data Receiving interface for OpenMedIC Building Blocks.
	/// 
	/// A Receiver is a component that receives data from a Sender.
	/// The purpose for the interface is to maintain daisy-chaining compatibility in
	/// components where it's relevant, and where it's not relevant it allows to break it
	/// deliberately by not implementing IReceiver.
	/// 
	/// The interface for a Receiver specifies an init() method to initialize the component,
	/// and two methods to pass data to the Receiver:  individual sample and sample array.
	/// </summary>
	public interface IReceiver
	{

		/// <summary>
		/// Initialize myself.
		/// </summary>
		/// <param name="iData"></param>
		void init ( ChainInfo iData );

		/// <summary>
		/// A new sample is available, and it must be processed and/or propagated
		/// to the followers as appropriate.
		/// </summary>
		/// <param name="newValue"></param>
		void addValue ( Sample newValue );

		/// <summary>
		/// A number of new samples are available, and they must be processed 
		/// and/or propagated to the followers as appropriate.
		/// </summary>
		/// <param name="newValues">Zero-based array of samples.
		///                         Note that the OLDEST sample is newValues[0]</param>
		void addValues ( Samples newValues );

	}	// End of CLASS
}
