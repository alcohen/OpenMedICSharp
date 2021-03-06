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
	/// This class creates an exception for general exception throwing by Wfdb-related code.
	/// </summary>
	class WfdbException : Exception
	{
		public WfdbException(string msg)
			: base(msg)
		{
		}
	}
}
