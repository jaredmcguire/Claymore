using System;
using System.IO;
using System.Linq;
using Claymore.Models;
using Microsoft.SqlServer.Management.Smo;

namespace Claymore.Logic
{
    public interface IScripter
    {
        void ScriptToDisk(IScriptable item, string type);
    }

    public class Scripter : IScripter
    {
        private readonly ISettings settings;

        public Scripter(ISettings settings)
        {
            this.settings = settings;
        }

        public void ScriptToDisk(IScriptable item, string type)
        {
            var script = GetScript(item);
            var file = GetFile(item, type);
            WriteScriptToFile(script, file);
        }

        private static void WriteScriptToFile(string script, FileInfo file)
        {
            if (file.Exists)
                using (var reader = file.OpenText())
                    if (string.Equals(script, reader.ReadToEnd(), StringComparison.Ordinal))
                        return;
            Console.WriteLine($"    {file.Name}");
            if (file.Directory.Exists == false)
                file.Directory.Create();
            using (var writer = file.CreateText())
                writer.Write(script);
        }

        private FileInfo GetFile(IScriptable item, string type)
        {
            var name = Path.GetInvalidFileNameChars().Aggregate(((ScriptNameObjectBase)item).Name, (current, next) => current.Replace(next, '_'));
            return new FileInfo($"{settings.OutputPath}\\{type}\\{name}.sql");
        }

        private static string GetScript(IScriptable item)
        {
            return item.Script().Cast<string>()
                       .Aggregate(string.Empty, (current, next) => current + Environment.NewLine + next);
        }
    }
}