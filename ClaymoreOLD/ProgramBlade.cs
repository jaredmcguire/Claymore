using ArcLight.Core;
using Claymore.Tasks;

namespace Claymore
{
	public class ProgramBlade : IBlade
	{
		public void Registration(ILocatorProvider locator)
		{
			locator.Register<ITraceLog, ConsoleTraceLog>("ConsoleTraceLog");
			locator.Register<IGetCommandLineOptionsTask, GetCommandLineOptionsTask>();
			locator.Register<IController, Controller>();
			locator.Register<IGetItemsFromServerTask, GetItemsFromServerTask>();
			locator.Register<IScriptItemToFileTask, ScriptItemToFileTask>();
		}

		public void Start()
		{
		}
	}
}