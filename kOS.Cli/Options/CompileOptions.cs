using CommandLine;

namespace kOS.Cli.Options
{
    [Verb("compile", HelpText = "Compiles a single Kerboscript or a complete Kerboscript source directory.")]
    public class CompileOptions : InputOuputOptions
    {
        [Option('v', "volume", Default = "all", Required = false, HelpText = "Volume to compile (specified in the ksconfig.json). Will be ignored when a input is specified.")]
        public string Volume { get; set; }
    }
}
