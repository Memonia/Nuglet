using System;
using System.IO;

using Nuglet.Core;
using Nuglet.Logging;

using Spectre.Console.Cli;

namespace Nuglet
{
	internal sealed class NugletCommand : Command<NugletCommandSettings>
	{
		public override int Execute(CommandContext context, NugletCommandSettings settings)
		{
			var logger = new SpectreAnsiConsoleLogger(settings.Severity);
			try
			{
				var project = NugletUtils.GetUniqueProjectFullPath(settings.Destination, settings.ProjectName);
				var nuglet = Path.GetFullPath(Path.Combine(project, Constants.NugletHiddenFolderName));
				var feed = Path.Combine(nuglet, Constants.NugetFeedDirectoryName);
				var cache = Path.Combine(nuglet, Constants.NugetCacheDirectoryName);
				var csproj = Path.Combine(Constants.ProjectName, $"{Constants.ProjectName}.csproj");
				var executor = new CommandExecutor(project, TimeSpan.FromSeconds(settings.Timeout), logger);

				logger.LogInformation($"Creating the project in {project}");
				NugletUtils.CreateProjectFolder(project, settings.Overwrite, logger);
				Directory.CreateDirectory(nuglet);
				File.SetAttributes(nuglet, FileAttributes.Hidden);

				if (settings.VisualStudioSolution)
				{
					logger.LogInformation($"Creating the Visual Studio solution");
					executor.Execute<DotnetErrorParser>("dotnet", $"new console -n {Constants.ProjectName} --use-program-main");
					executor.Execute<DotnetErrorParser>("dotnet", $"new sln -n {Constants.SolutionName}");
					executor.Execute<DotnetErrorParser>("dotnet", $"sln add {csproj}");

				}

				else
				{
					logger.LogInformation($"Creating the project");
					executor.Execute<DotnetErrorParser>("dotnet", $"new console -n {Constants.ProjectName} --use-program-main");
				}
				logger.LogInformation($"Creating the NuGet feed");
				Directory.CreateDirectory(feed);

				logger.LogInformation($"Creating the NuGet cache");
				Directory.CreateDirectory(cache);

				logger.LogInformation($"Creating the NuGet.config file");
				NugletUtils.WriteNugetConfig(
					project,
					Path.GetRelativePath(project, feed),
					Path.GetRelativePath(project, cache)
				);

				logger.LogInformation($"Creating a dummy package");
				NugletUtils.CreateDummyPackage(feed);

				logger.LogInformation($"Pushing the targets to the feed");
				foreach (var target in settings.Targets)
				{
					var ft = Path.GetFullPath(target);
					executor.Execute<DotnetNugetErrorParser>("dotnet", $"nuget push -s \"{feed}\" \"{ft}\"");
				}

				logger.LogInformation($"Removing a dummy package");
				NugletUtils.RemoveDummyPackage(feed);

				logger.LogInformation($"Adding package references to the project");
				foreach (var target in Directory.EnumerateDirectories(feed))
				{
					var fn = Path.GetFileName(target);
					executor.Execute<DotnetNugetErrorParser>("dotnet", $"add \"{csproj}\" package \"{fn}\"");
				}

				logger.LogInformation($"Successfully created the project");
			}

			catch (Exception ex)
			{
				logger.LogDebug(ex.ToString());
				throw new NugletException($"Failed to create the project. {ex.Message}", ex);
			}

			return 0;
		}
	}
}
