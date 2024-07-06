using System;

namespace Nuglet
{
	internal sealed class NugletException : Exception
	{
		public NugletException(string message) : base(message)
		{

		}

		public NugletException(string message, Exception innerException) : base(message, innerException)
		{

		}
	}
}
