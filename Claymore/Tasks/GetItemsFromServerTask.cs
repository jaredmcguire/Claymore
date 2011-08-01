using System.Linq;
using ArcLight.Core;
using Claymore.Models;
using Microsoft.SqlServer.Management.Smo;

namespace Claymore.Tasks
{
	public interface IGetItemsFromServerTask
	{
		ServerItems Run(string serverName, string databaseName);
	}

	public class GetItemsFromServerTask : IGetItemsFromServerTask
	{
		private readonly ITracer tracer;

		public GetItemsFromServerTask(ITracer tracer)
		{
			this.tracer = tracer;
		}

		public ServerItems Run(string serverName, string databaseName)
		{
			tracer.Write("Connecting to {0} on {1}", databaseName, serverName);
			var server = new Server(serverName);
			server.SetDefaultInitFields(typeof(Table), "IsSystemObject");
			server.SetDefaultInitFields(typeof(View), "IsSystemObject");
			server.SetDefaultInitFields(typeof(StoredProcedure), "IsSystemObject");
			server.SetDefaultInitFields(typeof(UserDefinedFunction), "IsSystemObject");
			server.SetDefaultInitFields(typeof(Trigger), "IsSystemObject");
			var database = server.Databases[databaseName];
			tracer.Write("Connected");

			return new ServerItems
			{
				Tables = database.Tables.Cast<Table>()
					.Where(x => x.IsSystemObject == false),
				Views = database.Views.Cast<View>()
					.Where(x => x.IsSystemObject == false),
				StoredProcedures = database.StoredProcedures.Cast<StoredProcedure>()
					.Where(x => x.IsSystemObject == false),
				Functions = database.UserDefinedFunctions.Cast<UserDefinedFunction>()
					.Where(x => x.IsSystemObject == false),
				Triggers = database.Triggers.Cast<Trigger>()
					.Where(x => x.IsSystemObject == false),
			};
		}
	}
}