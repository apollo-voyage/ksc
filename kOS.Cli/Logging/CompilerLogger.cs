using System;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using kOS.Cli.Models;
using kOS.Cli.Options;
using Pastel;

namespace kOS.Cli.Logging
{
    public class CompilerLogger : Logger
    {
        public void StartScriptLoading()
        {
            Info("Loading Kerboscripts...");
            ResetAndStartWatch();
        }

        public void StopScriptLoading(int amount)
        {
            StopWatch();
            Done(Draw.PrefixAndColor, "{0} Kerboscripts loaded in {1} ms.", amount, Elapsed);
            NewLine();
        }

        public void StartCompilation()
        {
            Info("Compiling Kerboscripts...");
            ResetAndStartWatch();
        }

        public void CompilationError(Exception e, Kerboscript script)
        {
            NewLine();
            Error(Draw.PrefixAndColor, "in {0}", script.InputPath);
            Error(Draw.Color, e.Message);
        }

        public void StopCompilation(int amount)
        {
            StopWatch();
            Done(Draw.PrefixAndColor, "{0} Kerboscripts compiled in {1} ms.", amount, Elapsed);
        }

        public void NoFilesFound(CompileOptions options) 
        {
            NewLine();
            Warn(Draw.PrefixAndColor, "No Kerboscript files found.");
            NewLine();
            Warn(Draw.Color, "Options:");
            if (options.Input != Constants.CurrentDirectory)
            {
                Warn(Draw.Color, "    Input: {0}", Path.GetFullPath(options.Input));
                Warn(Draw.Color, "    Ouput: {0}", Path.GetFullPath(options.Output));
            }
            NewLine();
            Warn(Draw.Color, "Please check if the given input is valid.");
        }
    }
}
