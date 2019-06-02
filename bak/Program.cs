using System;
using CommandLine;

namespace KSCompiler
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args).WithParsed<Options>(o => Run(o));
        }

        static void Run(Options options)
        {
            Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);
            Console.WriteLine("Input: " + options.Input);
            Console.WriteLine("Output: " + options.Output);
        }
    }
}