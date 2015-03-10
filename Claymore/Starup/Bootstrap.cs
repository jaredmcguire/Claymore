using System;
using System.Reflection;

namespace Claymore.Starup
{
    public class Bootstrap
    {
        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
            var program = new Program();
            program.Start(args);
#if DEBUG
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
#endif
        }

        private static Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = string.Format("{0}.Resources.{1}.dll", assembly.GetName().Name, new AssemblyName(args.Name).Name);
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                var data = new Byte[stream.Length];
                stream.Read(data, 0, data.Length);
                return Assembly.Load(data);
            }
        }
    }
}