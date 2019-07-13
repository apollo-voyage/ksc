using CommandLine;

namespace kOS.Cli.Options
{
    [Verb("compile", HelpText = "Compiles a single Kerboscript or a complete Kerboscript source directory.")]
    public class CompileOptions : InputOuputOptions
    {
        [Option('v', "volume", Default = "all", Required = false, HelpText = "Volume to compile (specified in the ksconfig.json). Will be ignored when a input is specified.")]
        public string Volume { get; set; }

        /// <summary>
        /// Creates compile options from watch options.
        /// </summary>
        /// <param name="options">Watch options to convert.</param>
        /// <returns>Created compile options.</returns>
        public static CompileOptions FromWatchOptions(WatchOptions options)
        {
            return new CompileOptions
            {
                Input = options.Input,
                Output = options.Output,
                Volume = options.Volume
            };
        }

        /// <summary>
        /// Creates compile options from deploy options.
        /// </summary>
        /// <param name="options">Deploy options to convert.</param>
        /// <returns>Created compile options.</returns>
        public static CompileOptions FromDeployOptions(DeployOptions options)
        {
            return new CompileOptions
            {
                Input = ".",
                Output = ".",
                Volume = options.Volume
            };
        }
    }
}
