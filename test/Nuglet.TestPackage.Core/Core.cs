using System;

using Nuglet.TestPackage.Utility;

namespace Nuglet.TestPackage.Core
{
	public static class CoreFunctions
	{
		public static void WriteHello() => 
			Console.WriteLine($"Hello {UtilityFunctions.GetCurrentUserName()} on {UtilityFunctions.GetOperatingSystem()}!");
	}
}
