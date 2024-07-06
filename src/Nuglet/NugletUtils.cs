using System.IO;
using System.Text;
using System.Xml.Linq;

using Nuglet.Core;
using Nuglet.Logging;

namespace Nuglet
{
	internal static class NugletUtils
	{
		public static void CreateProjectFolder(string path, bool overwrite, ILogger logger)
		{
			if (Directory.Exists(path))
			{
				if (overwrite)
				{
					logger.LogWarning($"Project directory already exists. Clearing the directory");
					Directory.Delete(path, true);
					Directory.CreateDirectory(path);
				}

				else
				{
					logger.LogError($"Directory {path} already exists and no --overwrite option was specified");
					throw new NugletException("Project directory already exists");
				}
			}

			else
			{
				logger.LogInformation($"Creating the project directory");
				Directory.CreateDirectory(path);
			}
		}

		public static void CreateDummyPackage(string feed)
		{
			var pkg = Path.Combine(feed, Constants.DummyPackageId);
			var ver = Path.Combine(pkg, Constants.DummyPackageVersion);
			var nuspec = Path.Combine(ver, $"{Constants.DummyPackageId}.nuspec");

			Directory.CreateDirectory(pkg);
			Directory.CreateDirectory(ver);
			File.Create(nuspec).Close();
		}

		public static void RemoveDummyPackage(string feed)
		{
			var pkg = Path.Combine(feed, Constants.DummyPackageId);
			Directory.Delete(pkg, true);
		}

		public static void WriteNugetConfig(string path, string feed, string cache)
		{
			var cfg = Path.Combine(path, "NuGet.config");
			var xml =
				new XElement("configuration",
					new XElement("config",
						new XElement("add",
							new XAttribute("key", "globalPackagesFolder"),
							new XAttribute("value", cache)
						)
					),
					new XElement("packageSources",
						new XElement("add",
							new XAttribute("key", Constants.NugetFeedName),
							new XAttribute("value", feed)
						)
					)
				);

			xml.Save(cfg);
		}

		public static string GetUniqueProjectFullPath(string destination, string? name)
		{
			var projectPath = string.Empty;
			if (name is null)
			{
				var nameBuilder = new StringBuilder(Constants.DefaultProjectDirectoryName);
				var count = 256;
				var found = false;
				for (int i = 1; i <= count; ++i)
				{
					projectPath = Path.Combine(destination, nameBuilder.ToString());
					if (!Directory.Exists(projectPath))
					{
						found = true;
						break;
					}

					nameBuilder.Length = Constants.DefaultProjectDirectoryName.Length;
					nameBuilder.Append('-');
					nameBuilder.Append(i);
				}

				if (!found)
				{
					throw new NugletException("Too many default projects created");
				}
			}

			else
			{
				projectPath = Path.Combine(destination, name);
			}

			return Path.GetFullPath(projectPath);
		}
	}
}
