using System;

namespace Nuglet.TestPackage.Utility
{
	public static class UtilityFunctions
	{
		public static string GetCurrentUserName() => Environment.UserName;
		public static string GetOperatingSystem() => Environment.OSVersion.ToString();
	}
}
