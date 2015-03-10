using System;
using System.IO;
using Claymore.Starup;
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
            if (!Parser.Default.ParseArguments(args, options))
				Environment.Exit(1);
			return options;
		}
	}
}