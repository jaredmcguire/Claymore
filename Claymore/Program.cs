using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using Claymore.Models;
using Newtonsoft.Json;

namespace Claymore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateContainer().Resolve<IApp>().Run();
#if DEBUG
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
#endif
        }

        private static IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();

            // IName convention
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                   .InNamespaceOf<Program>()
                   .Where(t => t.IsClass && t.GetInterfaces().Any(i => i.Name == $"I{t.Name}"))
                   .As(t => t.GetInterfaces().First(i => i.Name == $"I{t.Name}"));

            // settings
            builder.RegisterInstance(JsonConvert.DeserializeObject<Settings>(File.ReadAllText("Claymore.json"))).As<ISettings>();

            return builder.Build();
        }
    }
}