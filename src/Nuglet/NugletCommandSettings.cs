using System.ComponentModel;
using System.IO;

using Nuglet.Logging;

using Spectre.Console;
using Spectre.Console.Cli;

namespace Nuglet
{
	internal sealed class NugletCommandSettings : CommandSettings
	{
		[Description("NuGet packages to be included")]
		[CommandArgument(0, "<targets>")]
		public string[] Targets { get; init; } = [];

		[Description("The name of the nuglet project")]
		[CommandOption("-n | --name")]
		public string? ProjectName { get; init; }

		[Description("If the --name option is specified, clears the directory in destination with this name")]
		[CommandOption("-o | --overwrite")]
		[DefaultValue(false)]
		public bool Overwrite { get; init; }

		[Description("Directory where the new nuglet project will be created")]
		[CommandOption("-d | --destination")]
		[DefaultValue(".")]
		public string Destination { get; init; } = "";

		[Description("Create a Visual Studio solution")]
		[CommandOption("--vs-sln")]
		[DefaultValue(false)]
		public bool VisualStudioSolution { get; init; }

		[Description("Time in seconds for each individual command to be executed before it is aborted")]
		[CommandOption("-t | --timeout")]
		[DefaultValue(5)]
		public int Timeout { get; init; }

		[Description("Log Severity level to output. The values are: Error, Warning, Information, Debug")]
		[CommandOption("-l | --log-severity")]
		[DefaultValue(LogSeverity.Information)]
		public LogSeverity Severity { get; init; }

		public override ValidationResult Validate()
		{
			if (!Directory.Exists(Destination))
			{
				return ValidationResult.Error("Destination directory does not exist");
			}

			if (Timeout < 0)
			{
				return ValidationResult.Error("Timeout must be a non-negative integer");
			}

			if (ProjectName is not null)
			{
				if (ProjectName.Length == 0)
				{
					return ValidationResult.Error("Project name cannot be empty");
				}

				if (string.IsNullOrWhiteSpace(ProjectName))
				{
					return ValidationResult.Error("Project name cannot contain spaces");
				}
			}

			return base.Validate();
		}
	}
}
