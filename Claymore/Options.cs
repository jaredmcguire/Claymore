using System.Reflection;
using CommandLine;
using CommandLine.Text;

namespace Claymore
{
	public class Options
	{
		[Option("s", "server", Required = true, HelpText = "The name of the server.")]
		public string Server;

		[Option("d", "database", Required = true, HelpText = "The name of the database.")]
		public string Database;

		[Option("o", "outputpath", Required = true, HelpText = "The output path for sql scripts.")]
		public string OutputPath;

		[HelpOption("h", "help", HelpText = "Dispaly this help screen.")]
		public string Help()
		{
			var help = new HelpText(
				new HeadingInfo("Claymore", Assembly.GetExecutingAssembly().GetName().Version.ToString(2)),
				new CopyrightInfo("Jared McGuire", 2011),
				this);
			return help.ToString();
		}
	}
}
