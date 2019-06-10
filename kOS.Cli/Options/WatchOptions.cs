using CommandLine;

namespace kOS.Cli.Options
{
    [Verb("watch", HelpText = "Runs the compiler in watch mode. Watch input files and trigger recompilation on changes.")]
    public class WatchOptions : InputOuputOptions { }
}
