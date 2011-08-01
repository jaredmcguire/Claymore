using System;
using System.IO;
using CommandLine;

namespace Claymore.Tasks
{
	public interface IGetCommandLineOptionsTask
	{
		Options Run(string[] args);
	}

	public class GetCommandLineOptionsTask : IGetCommandLineOptionsTask
	{
		public Options Run(string[] args)
		{
			var options = GetOptionsFromCommandLineArgs(args);
			options.OutputPath = Path.GetFullPath(options.OutputPath);
			return options;
		}

		private static Options GetOptionsFromCommandLineArgs(string[] args)
		{
			var options = new Options();
			var parser = new CommandLineParser(new CommandLineParserSettings(false, Console.Error));
			if (parser.ParseArguments(args, options) == false)
				Environment.Exit(1);
			return options;
		}
	}
}