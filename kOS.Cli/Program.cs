using System;
using CommandLine;
using kOS.Cli.Actions;
using kOS.Cli.Logging;
using kOS.Cli.Options;

namespace kOS.Cli
{
    class Program
    {
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

        private static int Compile(CompileOptions options)
        {
            Compiler compiler = new Compiler(options);
            int result = compiler.Run();

            #if DEBUG
                Console.Read();
            #endif

            return result;
        }

        private static int Watch(WatchOptions options)
        {
            Watcher watcher = new Watcher(options);
            int result = watcher.Run();

            #if DEBUG
                Console.Read();
            #endif

            return result;
        }

        private static int Deploy(DeployOptions options)
        {
            int result = 1;

            #if DEBUG
                Console.Read();
            #endif

            return result;
        }

        private static int Run(RunOptions options)
        {
            int result = 1;

            #if DEBUG
                Console.Read();
            #endif

            return result;
        }

        private static int Init(InitOptions options)
        {
            Initializer initializer = new Initializer(options);
            int result = initializer.Run();

            #if DEBUG
                Console.Read();
            #endif

            return result;
        }
    }
}
