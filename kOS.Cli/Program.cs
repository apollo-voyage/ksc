using System;
using CommandLine;
using kOS.Cli.Actions;
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
            return 1;
        }

        private static int Watch(WatchOptions options)
        {
            return 1;
        }

        private static int Deploy(DeployOptions options)
        {
            return 1;
        }

        private static int Run(RunOptions options)
        {
            return 1;
        }

        private static int Init(InitOptions options)
        {
            Initializer initializer = new Initializer(options);
            int result = initializer.Run();

            Console.Read();

            return result;
        }
    }
}
