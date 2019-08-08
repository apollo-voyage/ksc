using kOS.Cli.Actions;

namespace kOS.Cli
{
    /// <summary>
    /// Main program.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Main method; entry point.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        /// <returns>CLI return code.</returns>
        static int Main(string[] args)
        {
            return ActionDispatcher.Dispatch(args);
        }
    }
}
