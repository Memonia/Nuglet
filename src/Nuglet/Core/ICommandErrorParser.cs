using System.IO;

namespace Nuglet.Core
{
	internal interface ICommandErrorParser
	{
		abstract static string GetError(StreamReader output, StreamReader error);
	}
}
