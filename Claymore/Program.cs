using System;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace Claymore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var container = new WindsorContainer().Install(FromAssembly.InThisApplication());
            try
            {
                container.Resolve<IController>().Run(args);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine();
                Console.Error.WriteLine("Exception: {0}", ex.Message);
                Console.Error.WriteLine(ex.StackTrace);
                Console.Error.WriteLine();
            }
#if DEBUG
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
#endif
        }
    }
}