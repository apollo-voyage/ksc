using CommandLine;

namespace kOS.Cli.Options
{
    [Verb("compile", HelpText = "Compile a single input file or a complete input source directory.")]
    public class CompileOptions : InputOuputOptions { }
}
