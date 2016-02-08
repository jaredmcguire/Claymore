using System;
using System.Linq;
using Autofac;

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
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var builder = new ContainerBuilder();

            // all classes with IName convention
            builder.RegisterAssemblyTypes(assemblies)
                   .InNamespaceOf<Program>()
                   .Where(t => t.IsClass && t.GetInterfaces().Any(i => i.Name == $"I{t.Name}"))
                   .As(t => t.GetInterfaces().First(i => i.Name == $"I{t.Name}"));

            return builder.Build();
        }
    }
}