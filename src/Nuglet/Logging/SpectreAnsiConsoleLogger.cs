using Spectre.Console;

namespace Nuglet.Logging
{
	internal sealed class SpectreAnsiConsoleLogger : ILogger
	{
		private readonly LogSeverity _severity;

		public SpectreAnsiConsoleLogger(LogSeverity severity)
		{
			_severity = severity;
		}

		public void LogInformation(string message)
		{
			if (_severity >= LogSeverity.Information)
			{
				AnsiConsole.MarkupLineInterpolated($"[blue][[Info]][/] {message}");
			}
		}

		public void LogWarning(string message)
		{
			if (_severity >= LogSeverity.Warning)
			{
				AnsiConsole.MarkupLineInterpolated($"[yellow][[Warning]][/] {message}");
			}
		}

		public void LogError(string message)
		{
			if (_severity >= LogSeverity.Error)
			{
				AnsiConsole.MarkupLineInterpolated($"[red][[Error]][/] {message}");
			}
		}

		public void LogDebug(string message)
		{
			if (_severity >= LogSeverity.Debug)
			{
				AnsiConsole.MarkupLineInterpolated($"[grey][[Debug]][/] {message}");
			}
		}
	}
}
