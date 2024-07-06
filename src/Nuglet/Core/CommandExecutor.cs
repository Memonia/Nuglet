using System;
using System.Diagnostics;

using Nuglet.Logging;

namespace Nuglet.Core
{
	internal sealed class CommandExecutor
	{
		public string WorkingDirectory { get; set; }
		public TimeSpan DefaultTimeout { get; set; }

		private readonly ILogger _logger;
		private readonly ProcessStartInfo _info;

		public CommandExecutor(string workingDirectory, TimeSpan defaultTimeout, ILogger logger)
		{
			WorkingDirectory = workingDirectory;
			DefaultTimeout = defaultTimeout;

			_logger = logger;
			_info = new ProcessStartInfo
			{
				WorkingDirectory = workingDirectory,
				CreateNoWindow = true,
				UseShellExecute = false,
				RedirectStandardInput = true,
				RedirectStandardError = true,
				RedirectStandardOutput = true
			};
		}

		public void Execute<TErrorParser>(string command, string arguments) 
			where TErrorParser : ICommandErrorParser
		{
			Execute<TErrorParser>(command, arguments, WorkingDirectory, DefaultTimeout);
		}

		public void Execute<TErrorParser>(string command, string arguments, string workingDirectory, TimeSpan timeout) 
			where TErrorParser : ICommandErrorParser
		{
			_info.FileName = command;
			_info.Arguments = arguments;
			_info.WorkingDirectory = workingDirectory;

			using var ps = new Process()
			{
				StartInfo = _info
			};

			_logger.LogInformation($"Executing: {command} {arguments}");

			ps.Start();
			if (!ps.WaitForExit(timeout))
			{
				_logger.LogError($"Executing: {command} {arguments}");
				_logger.LogError($"Command timed out after {timeout.TotalSeconds} seconds");

				ps.Kill(true);
				throw new TimeoutException("Command timed out");
			}

			else
			if (ps.ExitCode != 0)
			{
				_logger.LogError($"Executing: {command} {arguments}");
				_logger.LogError($"Reason: {TErrorParser.GetError(ps.StandardOutput, ps.StandardError)}");
				throw new CommandException($"Command failed with an exit code {ps.ExitCode}");
			}
		}
	}
}
