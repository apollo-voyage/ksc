using CommandLine;

namespace kOS.Cli.Options
{
    [Verb("deploy", HelpText = "Compile and deploy your Kerboscript source to a Kerbal Space Programm (KSP) installation.")]
    class DeployOptions
    {
        [Option('i', "input", Default = ".", Required = true, HelpText = "Input Kerboscript file or source directory.")]
        public string Input { get; set; }

        [Option('t', "target", Default = ".", Required = false, HelpText = "Target Kerbal Space Programm (KSP) installation.")]
        public string Target { get; set; }

        [Option('s', "source", Default = false, Required = false, HelpText = "Deploy sources only; compilation will be skipped.")]
        public bool DeploySource { get; set; }
    }
}
