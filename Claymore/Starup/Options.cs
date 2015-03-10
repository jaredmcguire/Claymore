using System.Reflection;
using CommandLine;
using CommandLine.Text;

namespace Claymore.Starup
{
    public class Options
    {
        [Option('c', "conn", Required = true, HelpText = "The connection string to connect with.")]
        public string ConnectionString { get; set; }

        [Option('o', "out", Required = false, HelpText = "The output path for sql scripts.", DefaultValue = ".\\sql")]
        public string OutputPath { get; set; }

        [HelpOption('?', "help")]
        public string Help()
        {
            var help = new HelpText
                       {
                           Heading = new HeadingInfo("Claymore", Assembly.GetExecutingAssembly().GetName().Version.ToString(2)),
                           Copyright = new CopyrightInfo("Jared McGuire", 2015),
                           AdditionalNewLineAfterOption = true,
                           AddDashesToOption = true
                       };
            help.AddOptions(this);
            return help;
        }
    }
}