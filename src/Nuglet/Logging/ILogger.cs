namespace Nuglet.Logging
{
	internal interface ILogger
	{
		void LogError(string message);
		void LogWarning(string message);
		void LogInformation(string message);
		void LogDebug(string message);
	}
}
