using System.Collections.Generic;
using System.IO;
using System.Linq;
using ArcLight.Core;
using Claymore.Tasks;
using Microsoft.SqlServer.Management.Smo;

namespace Claymore
{
	public interface IController
	{
		void Run(string[] args);
	}

	public class Controller : IController
	{
		private readonly ITracer tracer;
		private readonly IGetCommandLineOptionsTask getCommandLineOptions;
		private readonly IGetItemsFromServerTask getItemsFromServer;
		private readonly IScriptItemToFileTask scriptItemToFile;

		public Controller(ITracer tracer, IGetCommandLineOptionsTask getCommandLineOptions, IGetItemsFromServerTask getItemsFromServer, IScriptItemToFileTask scriptItemToFile)
		{
			this.tracer = tracer;
			this.getCommandLineOptions = getCommandLineOptions;
			this.getItemsFromServer = getItemsFromServer;
			this.scriptItemToFile = scriptItemToFile;
		}

		public void Run(string[] args)
		{
			var options = getCommandLineOptions.Run(args);
			tracer.Write("Outputing scripts to: {0}", options.OutputPath);

			var serverItems = getItemsFromServer.Run(options.Server, options.Database);

			ProcessItems("tables", serverItems.Tables, options.OutputPath);
			ProcessItems("views", serverItems.Views, options.OutputPath);
			ProcessItems("stored procedures", serverItems.StoredProcedures, options.OutputPath);
			ProcessItems("functions", serverItems.Functions, options.OutputPath);
			ProcessItems("triggers", serverItems.Triggers, options.OutputPath);

			tracer.Write("Completed");
		}

		private void ProcessItems(string itemType, IEnumerable<IScriptable> items, string outputPath)
		{
			if (items.Count() == 0) return;
			tracer.Write("Checking {0} {1}", items.Count(), itemType);
			foreach (var item in items)
			{
				var file = GetFileInfo(outputPath, itemType, ((ScriptNameObjectBase)item).Name);
				scriptItemToFile.Run(item, file);
			}
		}

		private static FileInfo GetFileInfo(string path, string type, string name)
		{
			var invalidChars = Path.GetInvalidFileNameChars();
			name = invalidChars.Aggregate(name, (current, invalidChar) => current.Replace(invalidChar, '_'));
			return new FileInfo(string.Format(@"{0}\{1}\{2}.sql", path, type, name));
		}
	}
}