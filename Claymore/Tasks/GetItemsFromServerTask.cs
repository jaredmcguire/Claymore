using System;
using System.Linq;
using Claymore.Models;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace Claymore.Tasks
{
    public interface IGetItemsFromServerTask
    {
        ServerItems Run(string connectionString);
    }

    public class GetItemsFromServerTask : IGetItemsFromServerTask
    {

        public ServerItems Run(string connectionString)
        {
            var connection = new ServerConnection { ConnectionString = connectionString };
            var server = new Server(connection);
            server.SetDefaultInitFields(typeof(Table), "IsSystemObject");
            server.SetDefaultInitFields(typeof(View), "IsSystemObject");
            server.SetDefaultInitFields(typeof(StoredProcedure), "IsSystemObject");
            server.SetDefaultInitFields(typeof(UserDefinedFunction), "IsSystemObject");
            server.SetDefaultInitFields(typeof(Trigger), "IsSystemObject");

            Console.WriteLine("Connecting...");
            if (server.Version.Major < 9)
                throw new UnsupportedVersionException("SQL Server 2005 or greater is required.");
            Console.WriteLine("Connected");

            var database = server.Databases[connection.SqlConnectionObject.Database];
            return new ServerItems
                   {
                       Tables = database.Tables.Cast<Table>().Where(x => x.IsSystemObject == false),
                       Views = database.Views.Cast<View>().Where(x => x.IsSystemObject == false),
                       StoredProcedures = database.StoredProcedures.Cast<StoredProcedure>().Where(x => x.IsSystemObject == false),
                       Functions = database.UserDefinedFunctions.Cast<UserDefinedFunction>().Where(x => x.IsSystemObject == false),
                       Triggers = database.Triggers.Cast<Trigger>().Where(x => x.IsSystemObject == false),
                   };
        }
    }
}