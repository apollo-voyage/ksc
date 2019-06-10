using CommandLine;

namespace kOS.Cli.Options
{
    public class InputOuputOptions
    {
        [Option('i', "input", Default = ".", Required = false, HelpText = "Input Kerboscript file or Kerboscript source directory.")]
        public string Input { get; set; }

        [Option('o', "output", Default = ".", Required = false, HelpText = "Output name for the compiled Kerboscript file or directory.")]
        public string Output { get; set; }
    }
}
