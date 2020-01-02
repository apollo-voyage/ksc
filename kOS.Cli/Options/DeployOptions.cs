using CommandLine;

namespace kOS.Cli.Options
{
    [Verb("deploy", HelpText = "Compile and deploy your Kerboscript source to a Kerbal Space Programm (KSP) installation. Deployment only works with a setup ksc project (use 'ksc init' to initialize a project).")]
    public class DeployOptions
    {
        [Option('v', "volume", Default = "all", Required = false, HelpText = "Volume to deploy (specified in the ksc.json)")]
        public string Volume { get; set; }

        [Option('s', "source", Default = false, Required = false, HelpText = "Deploy sources only; compilation will be skipped.")]
        public bool DeploySource { get; set; }
    }
}
