using System.Collections.Generic;
using Microsoft.SqlServer.Management.Smo;

namespace Claymore.Models
{
	public class ServerItems
	{
		public IEnumerable<Table> Tables { get; set; }
		public IEnumerable<View> Views { get; set; }
		public IEnumerable<StoredProcedure> StoredProcedures { get; set; }
		public IEnumerable<UserDefinedFunction> Functions { get; set; }
		public IEnumerable<Trigger> Triggers { get; set; }
	}
}