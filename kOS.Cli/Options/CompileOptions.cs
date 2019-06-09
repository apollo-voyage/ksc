using CommandLine;

namespace kOS.Cli.Options
{
    [Verb("compile", HelpText = "Compile a single input file or a complete input source directory.")]
    public class CompileOptions : InputOuputOptions
    {
        [Option('v', "volume", Default = "", Required = false, HelpText = "Which volume specified in the ksconfig.json to compiler.")]
        public string Volume { get; set; }

        [Option('w', "write", Default = true, Required = false, HelpText = "Flag, wheter or not the compiled files should be written to disk.")]
        public bool Write { get; set; }
    }
}
