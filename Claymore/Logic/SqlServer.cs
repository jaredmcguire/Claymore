using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace Claymore.Logic
{
    public interface ISqlServer
    {
        Database Connect(string connectionString);
    }

    public class SqlServer : ISqlServer
    {
        public Database Connect(string connectionString)
        {
            var connection = new ServerConnection { ConnectionString = connectionString };
            var server = new Server(connection);
            server.SetDefaultInitFields(typeof(Table), "IsSystemObject");
            server.SetDefaultInitFields(typeof(View), "IsSystemObject");
            server.SetDefaultInitFields(typeof(StoredProcedure), "IsSystemObject");
            server.SetDefaultInitFields(typeof(UserDefinedFunction), "IsSystemObject");
            server.SetDefaultInitFields(typeof(Trigger), "IsSystemObject");
            if (server.Version.Major < 9)
                throw new UnsupportedVersionException("SQL Server 2005 or greater is required.");
            return server.Databases[connection.SqlConnectionObject.Database];
        }
    }
}