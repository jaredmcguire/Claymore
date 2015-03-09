using System;
using ArcLight.Core;

namespace Claymore
{
	public class ConsoleTraceLog : ITraceLog
	{
		public void Write(string message)
		{
			Console.WriteLine(message);
		}
	}
}