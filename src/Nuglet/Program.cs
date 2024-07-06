using Nuglet.Logging;

using Spectre.Console.Cli;

namespace Nuglet
{
	internal sealed class Program
	{
		static void Main(string[] args)
		{
			var nuglet = new CommandApp<NugletCommand>();
			nuglet.Configure(config =>
			{
				config.SetApplicationName("nuglet");
				config.ValidateExamples();

				config.SetExceptionHandler((exception, context) =>
				{
					var logger = new SpectreAnsiConsoleLogger(LogSeverity.Error);
					logger.LogError($"{exception.Message}");
					logger.LogError($"Unhandled exception occured. See the output for details");
					return 1;
				});

				config.AddExample("./nupkgs/pkg1.nupkg");
				config.AddExample("./nupkgs/pkg1.nupkg", "./nupkgs/pkg2.nupkg");
				config.AddExample("./nupkgs/*.nupkg");
				config.AddExample("./nupkgs/pkg1.nupkg", "-d", "./proj/");
				config.AddExample("./nupkgs/pkg1.nupkg", "-d", "./proj/", "-n", "myLib");
				config.AddExample("./nupkgs/pkg1.nupkg", "-d", "./proj/", "-n", "myLib", "-o");
				config.AddExample("./nupkgs/pkg1.nupkg", "-d", "./proj/", "-n", "myLib", "-o", "-t", "1");
				config.AddExample("./nupkgs/pkg1.nupkg", "-d", "./proj/", "-n", "myLib", "-o", "-t", "1", "-l", "Debug");
			});

			nuglet.Run(args);
		}
	}
}
