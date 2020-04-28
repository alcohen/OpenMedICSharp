using System;
using System.Collections.Generic;
using System.Text;

namespace PodLib
{
	/// <summary>
	/// Used to raise exceptions from SerialReader classes
	/// </summary>
	class SerialReaderException : Exception
	{
		public SerialReaderException(string msg)
			: base(msg)
		{
		}
	}
}
