using System;
using CommandLine;
using kOS.Cli.Actions;
using kOS.Cli.Options;

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
            return Parser.Default.ParseArguments<CompileOptions, WatchOptions, DeployOptions, RunOptions, InitOptions>(args)
                .MapResult(
                    (CompileOptions options) => Compile(options),
                    (WatchOptions options)   => Watch(options),
                    (DeployOptions options)  => Deploy(options),
                    (RunOptions options)     => Run(options),
                    (InitOptions options)    => Init(options),
                    error                    => 1
                );
        }

        /// <summary>
        /// Runs the compile action.
        /// </summary>
        /// <param name="Options">CLI options for the compile action.</param>
        /// <returns>CLI return code.</returns>
        private static int Compile(CompileOptions Options)
        {
            Compiler compiler = new Compiler(Options);
            int result = compiler.Run();

            #if DEBUG
                Console.Read();
            #endif

            return result;
        }

        /// <summary>
        /// Runs the watch action.
        /// </summary>
        /// <param name="Options">CLI options for the watch action.</param>
        /// <returns>CLI return code.</returns>
        private static int Watch(WatchOptions Options)
        {
            Watcher watcher = new Watcher(Options);
            int result = watcher.Run();

            #if DEBUG
                Console.Read();
            #endif

            return result;
        }

        /// <summary>
        /// Runs the deploy action.
        /// </summary>
        /// <param name="Options">CLI options for the deploy action.</param>
        /// <returns>CLI return code.</returns>
        private static int Deploy(DeployOptions Options)
        {
            Deployer deployer = new Deployer(Options);
            int result = deployer.Run();

            #if DEBUG
                Console.Read();
            #endif

            return result;
        }

        /// <summary>
        /// Runs the run action.
        /// </summary>
        /// <param name="Options">CLI options for the run action.</param>
        /// <returns>CLI return code.</returns>
        private static int Run(RunOptions Options)
        {
            Runner runner = new Runner(Options);
            int result = runner.Run();

            #if DEBUG
                Console.Read();
            #endif

            return result;
        }

        /// <summary>
        /// Runs the init action.
        /// </summary>
        /// <param name="Options">CLI options for the init action.</param>
        /// <returns>CLI return code.</returns>
        private static int Init(InitOptions Options)
        {
            Initializer initializer = new Initializer(Options);
            int result = initializer.Run();

            #if DEBUG
                Console.Read();
            #endif

            return result;
        }
    }
}
