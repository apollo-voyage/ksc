using System;
using CommandLine;
using kOS.Cli.Options;

namespace kOS.Cli
{
    class Program
    {
        static int Main(string[] args)
        {
            return Parser.Default.ParseArguments<CompileOptions, WatchOptions, DeployOptions, RunOptions, InitOptions>(args)
                .MapResult(
                    (CompileOptions options) => ExecuteCompile(options),
                    (WatchOptions options)   => ExecuteWatch(options),
                    (DeployOptions options)  => ExecuteDeploy(options),
                    (RunOptions options)     => ExecuteRun(options),
                    (InitOptions options)    => ExecuteInit(options),
                    error                    => 1
                );

            //string str = Directory.GetCurrentDirectory();
            //Console.Write(str);
        }

        private static int ExecuteCompile(CompileOptions options)
        {
            return 1;
        }

        private static int ExecuteWatch(WatchOptions options)
        {
            return 1;
        }

        private static int ExecuteDeploy(DeployOptions options)
        {
            return 1;
        }

        private static int ExecuteRun(RunOptions options)
        {
            return 1;
        }

        private static int ExecuteInit(InitOptions options)
        {
            return 1;
        }
    }
}
