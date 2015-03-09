using System;
using System.IO;
using System.Text;
using Microsoft.SqlServer.Management.Smo;

namespace Claymore.Tasks
{
    public interface IScriptItemToFileTask
    {
        void Run(IScriptable table, FileInfo file);
    }

    public class ScriptItemToFileTask : IScriptItemToFileTask
    {
        public void Run(IScriptable item, FileInfo file)
        {
            var script = CreateScript(item);
            if (file.Exists)
                using (var reader = file.OpenText())
                    if (string.Equals(script, reader.ReadToEnd(), StringComparison.Ordinal))
                        return;
            Console.WriteLine("Writing {0}", file.Name);
            if (file.Directory.Exists == false)
                file.Directory.Create();
            using (var writer = file.CreateText())
                writer.Write(script);
        }

        public string CreateScript(IScriptable item)
        {
            var stringCollection = item.Script();
            var stringBuilder = new StringBuilder();
            foreach (var line in stringCollection)
                stringBuilder.AppendLine(line);
            return stringBuilder.ToString();
        }

    }
}