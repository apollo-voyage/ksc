using CommandLine;

namespace kOS.Cli.Options
{
    [Verb("watch", HelpText = "Runs the compiler in watch mode. Watch input files and trigger recompilation on changes.")]
    public class WatchOptions : InputOuputOptions
    {
        [Option('v', "volume", Default = "all", Required = false, HelpText = "Volume to watch (specified in the ksconfig.json). Will be ignored when a input file or directory is specified which does not contain a ksc project.")]
        public string Volume { get; set; }
    }
}
