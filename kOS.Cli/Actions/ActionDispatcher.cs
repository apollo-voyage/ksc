using System;
using CommandLine;
using kOS.Cli.Options;

namespace kOS.Cli.Actions
{
    public static class ActionDispatcher
    {
        public static int Dispatch(string[] args, bool fromMain = true)
        {
            return Parser.Default.ParseArguments<CompileOptions, WatchOptions, DeployOptions, RunOptions, InitOptions>(args)
                .MapResult(
                    (CompileOptions options) => Compile(options, fromMain),
                    (WatchOptions options) => Watch(options, fromMain),
                    (DeployOptions options) => Deploy(options, fromMain),
                    (RunOptions options) => Run(options, fromMain),
                    (InitOptions options) => Init(options, fromMain),
                    error => 1
                );
        }

        /// <summary>
        /// Runs the compile action.
        /// </summary>
        /// <param name="Options">CLI options for the compile action.</param>
        /// <returns>CLI return code.</returns>
        private static int Compile(CompileOptions Options, bool fromMain = true)
        {
            Compiler compiler = new Compiler(Options);
            int result = compiler.Run();

#if DEBUG
            if (fromMain == true)
            {
                Console.Read();
            }
#endif

            return result;
        }

        /// <summary>
        /// Runs the watch action.
        /// </summary>
        /// <param name="Options">CLI options for the watch action.</param>
        /// <returns>CLI return code.</returns>
        private static int Watch(WatchOptions Options, bool fromMain = true)
        {
            Watcher watcher = new Watcher(Options);
            int result = watcher.Run();

#if DEBUG
            if (fromMain == true)
            {
                Console.Read();
            }
#endif

            return result;
        }

        /// <summary>
        /// Runs the deploy action.
        /// </summary>
        /// <param name="Options">CLI options for the deploy action.</param>
        /// <returns>CLI return code.</returns>
        private static int Deploy(DeployOptions Options, bool fromMain = true)
        {
            Deployer deployer = new Deployer(Options);
            int result = deployer.Run();

#if DEBUG
            if (fromMain == true)
            {
                Console.Read();
            }
#endif

            return result;
        }

        /// <summary>
        /// Runs the run action.
        /// </summary>
        /// <param name="Options">CLI options for the run action.</param>
        /// <returns>CLI return code.</returns>
        private static int Run(RunOptions Options, bool fromMain = true)
        {
            Runner runner = new Runner(Options);
            int result = runner.Run();

#if DEBUG
            if (fromMain == true)
            {
                Console.Read();
            }
#endif

            return result;
        }

        /// <summary>
        /// Runs the init action.
        /// </summary>
        /// <param name="Options">CLI options for the init action.</param>
        /// <returns>CLI return code.</returns>
        private static int Init(InitOptions Options, bool fromMain = true)
        {
            Initializer initializer = new Initializer(Options);
            int result = initializer.Run();

#if DEBUG
            if (fromMain == true)
            {
                Console.Read();
            }
#endif

            return result;
        }
    }
}
