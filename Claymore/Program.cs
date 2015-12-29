using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Claymore.Logic;
using Claymore.Models;
using Microsoft.SqlServer.Management.Smo;

namespace Claymore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new WindsorContainer()
                .Install(FromAssembly.InThisApplication())
                .Resolve<Program>()
                .Run();
#if DEBUG
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
#endif
        }

        private readonly ISettings settings;
        private readonly ISqlServer sqlServer;
        private readonly IScripter scripter;

        public Program(ISettings settings, ISqlServer sqlServer, IScripter scripter)
        {
            this.settings = settings;
            this.sqlServer = sqlServer;
            this.scripter = scripter;
        }

        private void Run()
        {
            var database = sqlServer.Connect(settings.ConnectionString);
            Console.WriteLine($"Outputing scripts to {settings.OutputPath}");
            if (settings.ScriptTables)
                Process(database.Tables.Cast<Table>().Where(x => !x.IsSystemObject).ToList(), "tables");
            if (settings.ScriptViews)
                Process(database.Views.Cast<View>().Where(x => !x.IsSystemObject).ToList(), "views");
            if (settings.ScriptSprocs)
                Process(database.StoredProcedures.Cast<StoredProcedure>().Where(x => !x.IsSystemObject).ToList(), "sprocs");
            if (settings.ScriptFunctions)
                Process(database.UserDefinedFunctions.Cast<UserDefinedFunction>().Where(x => !x.IsSystemObject).ToList(), "functions");
            if (settings.ScriptTriggers)
                Process(database.Tables.Cast<Table>().SelectMany(x => x.Triggers.Cast<Trigger>()).ToList(), "triggers");
            if (settings.ScriptSchemas)
                Process(database.Schemas.Cast<Schema>().Where(x => !x.IsSystemObject).ToList(), "schemas");
            if (settings.ScriptUsers)
                Process(database.Users.Cast<User>().Where(x => !x.IsSystemObject).ToList(), "users");
        }

        private void Process(IReadOnlyCollection<IScriptable> items, string type)
        {
            if (!items.Any())
                return;
            Console.WriteLine($"{items.Count} {type}");
            items.ForEach(x => scripter.ScriptToDisk(x, type));
        }
    }
}