using System;
using System.IO;
using System.Text;

using Nuglet.Extensions;

namespace Nuglet.Core
{
	internal sealed class DotnetErrorParser : ICommandErrorParser
	{
		public static string GetError(StreamReader output, StreamReader error)
		{
			var sb = new StringBuilder(error.ReadToEnd());
			sb.RemoveTrailing(Environment.NewLine);
			return sb.ToString();
		}
	}
}
