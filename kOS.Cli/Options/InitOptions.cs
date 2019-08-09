using CommandLine;

namespace kOS.Cli.Options
{
    [Verb("init", HelpText = "Create a new Kerboscript project.")]
    public class InitOptions
    {
        [Value(0, MetaName = "name", Default = "", Required = false, HelpText = "Name of the project.")]
        public string ProjectName { get; set; }

        [Value(1, MetaName = "path", Default = "", Required = false, HelpText = "Path of the project.")]
        public string ProjectPath { get; set; }

        [Option('y', Default = false, Required = false, HelpText = "Skip project initialization questions and use defaults only.")]
        public bool Yes { get; set; }
    }
}
