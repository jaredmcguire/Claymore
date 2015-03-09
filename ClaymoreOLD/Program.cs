using System;
using ArcLight.Core;
using ArcLight.Unity;

namespace Claymore
{
	public class Program : ArcLightProgram
	{
		public static void Main(string[] args)
		{
			SetLocatorProvider(() => new UnityLocatorProvider());
			AttachBlade(new ProgramBlade());

			try
			{
				Locator.Resolve<IController>().Run(args);
			}
			catch (Exception ex)
			{
				Console.Error.WriteLine();
				Console.Error.WriteLine("Exception: {0}", ex.Message);
				Console.Error.WriteLine(ex.StackTrace);
				Console.Error.WriteLine();
			}
		}
	}
}