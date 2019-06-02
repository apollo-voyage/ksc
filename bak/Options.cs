using CommandLine;

namespace KSCompiler
{ 
    /// <summary>
    /// Commandline interface options.
    /// </summary>
    public class Options
    {
        /// <summary>
        /// Input filename or directory.
        /// </summary>
        [Option('i', "input", Required = true, HelpText = "Kerboscript input filename or path to a directory containing the Kerboscript source code.")]
        public string Input { get; set; }

        /// <summary>
        /// Output filename or directory.
        /// </summary>
        [Option('o', "output", Required = false, HelpText = "Kerboscript output filename or path to a directory where the compiled Kerboscript should be written to; defaults to './objs'")]
        public string Output { get; set; } = "obj";

        /// <summary>
        /// Indication wheter the compiler should watch the input file or directory for changes and recompile when changes are found.
        /// </summary>
        [Option('w', "watch", Required = false, HelpText = "Wheter the compiler should watch the input file or directory for changes and recompile when changes are found.")]
        public bool Watch { get; set; }
    }
}
