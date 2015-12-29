using System.IO;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Claymore.Models;
using Newtonsoft.Json;

namespace Claymore
{
    public class WindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // settings
            var settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText("Claymore.json"));
            container.Register(Component.For<ISettings>().Instance(settings));

            // default interfaces
            container.Register(Classes.FromThisAssembly().Pick()
                                      .WithServiceDefaultInterfaces()
                                      .LifestyleTransient());
        }
    }
}