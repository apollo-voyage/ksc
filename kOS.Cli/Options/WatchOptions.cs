using CommandLine;

namespace kOS.Cli.Options
{
    [Verb("watch", HelpText = "Run compiler in watch mode. Watch input files and trigger recompilation on changes.")]
    public class WatchOptions : InputOuputOptions { }
}
