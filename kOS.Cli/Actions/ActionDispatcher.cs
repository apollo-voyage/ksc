using System;
using CommandLine;
using kOS.Cli.Options;

namespace kOS.Cli.Actions
{
    /// <summary>
    /// Action dispatcher. Dispatches actions based on a given set of arguments.
    /// </summary>
    public static class ActionDispatcher
    {
        /// <summary>
        /// Dispatches a action.
        /// </summary>
        /// <param name="args">Arguments to dispatch action to.</param>
        /// <param name="fromMain">Wheter or not this is called from the main method.</param>
        /// <returns>CLI status code.</returns>
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
        /// <param name="options">CLI options for the compile action.</param>
        /// <param name="fromMain">Wheter or not this is called from the main method.</param>
        /// <returns>CLI return code.</returns>
        private static int Compile(CompileOptions options, bool fromMain = true)
        {
            CompileAction compiler = new CompileAction(options);
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
        /// <param name="options">CLI options for the watch action.</param>
        /// <param name="fromMain">Wheter or not this is called from the main method.</param>
        /// <returns>CLI return code.</returns>
        private static int Watch(WatchOptions options, bool fromMain = true)
        {
            WatchAction watcher = new WatchAction(options);
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
        /// <param name="options">CLI options for the deploy action.</param>
        /// <param name="fromMain">Wheter or not this is called from the main method.</param>
        /// <returns>CLI return code.</returns>
        private static int Deploy(DeployOptions options, bool fromMain = true)
        {
            DeployAction deployer = new DeployAction(options);
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
        /// <param name="options">CLI options for the run action.</param>
        /// <param name="fromMain">Wheter or not this is called from the main method.</param>
        /// <returns>CLI return code.</returns>
        private static int Run(RunOptions options, bool fromMain = true)
        {
            RunAction runner = new RunAction(options);
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
        /// <param name="options">CLI options for the init action.</param>
        /// <param name="fromMain">Wheter or not this is called from the main method.</param>
        /// <returns>CLI return code.</returns>
        private static int Init(InitOptions options, bool fromMain = true)
        {
            InitAction initializer = new InitAction(options);
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
