using System.Reflection;
using CommandLine;
using CommandLine.Text;

namespace Claymore.Starup
{
    public class Options
    {
        [Option('c', "connectionstring", Required = false, HelpText = "The connection string to connect with.")]
        public string ConnectionString { get; set; }

        [Option('o', "outputpath", Required = true, HelpText = "The output path for sql scripts.")]
        public string OutputPath { get; set; }

        [HelpOption('h', "help", HelpText = "Dispaly this help screen.")]
        public string Help()
        {
            var help = new HelpText(
                new HeadingInfo("Claymore", Assembly.GetExecutingAssembly().GetName().Version.ToString(2)),
                new CopyrightInfo("Jared McGuire", 2011),
                this);
            return help.ToString();
        }
    }
}