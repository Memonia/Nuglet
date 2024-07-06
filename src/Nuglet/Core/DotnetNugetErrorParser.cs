using System;
using System.IO;
using System.Text;

using Nuglet.Extensions;

namespace Nuglet.Core
{
	internal sealed class DotnetNugetErrorParser : ICommandErrorParser
	{
		public static string GetError(StreamReader output, StreamReader error)
		{
			if (!error.EndOfStream)
			{
				return DotnetErrorParser.GetError(output, error);
			}

			var sb = new StringBuilder();
			while (!output.EndOfStream)
			{
				var line = output.ReadLine();
				if (line is not null && line.StartsWith("error:"))
				{
					sb.AppendLine(line);
				}
			}

			sb.RemoveTrailing(Environment.NewLine);
			return sb.ToString();
		}
	}
}
