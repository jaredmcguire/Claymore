using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private readonly IGetCommandLineOptionsTask getCommandLineOptions;
        private readonly IGetItemsFromServerTask getItemsFromServer;
        private readonly IScriptItemToFileTask scriptItemToFile;

        public Controller(IGetCommandLineOptionsTask getCommandLineOptions, IGetItemsFromServerTask getItemsFromServer, IScriptItemToFileTask scriptItemToFile)
        {
            this.getCommandLineOptions = getCommandLineOptions;
            this.getItemsFromServer = getItemsFromServer;
            this.scriptItemToFile = scriptItemToFile;
        }

        public void Run(string[] args)
        {
            var options = getCommandLineOptions.Run(args);

            var serverItems = getItemsFromServer.Run(options.ConnectionString);

            Console.WriteLine("Outputing scripts to: {0}", options.OutputPath);

            ProcessItems("tables", serverItems.Tables, options.OutputPath);
            ProcessItems("views", serverItems.Views, options.OutputPath);
            ProcessItems("stored procedures", serverItems.StoredProcedures, options.OutputPath);
            ProcessItems("functions", serverItems.Functions, options.OutputPath);
            ProcessItems("triggers", serverItems.Triggers, options.OutputPath);

            Console.WriteLine("Completed");
        }

        private void ProcessItems(string itemType, IEnumerable<IScriptable> items, string outputPath)
        {
            var objects = items.ToList();
            if (!objects.Any())
                return;
            Console.WriteLine("Checking {0} {1}", objects.Count, itemType);
            foreach (var item in objects)
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