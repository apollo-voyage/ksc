using System;
using System.Drawing;
using System.IO;
using kOS.Cli.Models;
using kOS.Cli.Options;
using Pastel;

namespace kOS.Cli.Logging
{
    /// <summary>
    /// Logger implementation for the compile action.
    /// </summary>
    public class CompilerLogger : Logger
    {
        /// <summary>
        /// Flag, wheter a input was specified or not.
        /// </summary>
        private bool _noFilesFound = false;

        /// <summary>
        /// Flag, wheter the configuration was found.
        /// </summary>
        private bool _noConfigFound = false;

        /// <summary>
        /// Prints the start script loading messages.
        /// </summary>
        public void StartScriptLoading()
        {
            Info("Loading Kerboscripts...");
            ResetAndStartWatch();
        }

        /// <summary>
        /// Prints the stop script loading messages.
        /// </summary>
        /// <param name="amount"></param>
        public void StopScriptLoading(int amount)
        {
            StopWatch();

            if (_noFilesFound == false)
            {
                Done(Draw.Prefix, "{0} Kerboscripts loaded in {1} ms.", amount, Elapsed);
                NewLine();
            }
        }

        /// <summary>
        /// Prints the start compilation messages.
        /// </summary>
        public void StartCompilation()
        {
            Info("Compiling Kerboscripts...");
            ResetAndStartWatch();
        }

        /// <summary>
        /// Prints a compilation error.
        /// </summary>
        /// <param name="e">Compilation exception</param>
        /// <param name="script">Script which failed to compile.</param>
        public void CompilationError(Exception e, Kerboscript script)
        {
            NewLine();
            Error(Draw.PrefixAndColor, "in {0}", script.InputPath);
            Error(Draw.Color, e.Message);
        }

        /// <summary>
        /// Prints the stop compilation messages for success.
        /// </summary>
        /// <param name="amount"></param>
        public void StopCompilationSuccess(int amount)
        {
            StopWatch();
            Done(Draw.PrefixAndColor, "{0} Kerboscripts compiled in {1} ms.", amount, Elapsed);
        }

        /// <summary>
        /// Prints the stop compilation messages for failure.
        /// </summary>
        /// <param name="amount"></param>
        public void StopCompilationFailure()
        {
            StopWatch();
            Error(Draw.PrefixAndColor, "Compilation unsuccessfull. See error messages above.");
        }

        /// <summary>
        /// Prints no files found messages.
        /// </summary>
        /// <param name="options"></param>
        public void NoFilesFound(CompileOptions options) 
        {
            if (_noConfigFound == true)
            {
                return;
            }

            Warn(Draw.PrefixAndColor, "No Kerboscript files found.");
            if (options.Input != Constants.CurrentDirectory ||
                options.Output != Constants.CurrentDirectory)
            {
                NewLine();
                Warn(Draw.Color, "Options:");
                if (_noFilesFound == false) { 
                    Warn(Draw.Color, "    Input: {0}", Path.GetFullPath(options.Input));
                }

                Warn(Draw.Color, "    Ouput: {0}", Path.GetFullPath(options.Output));

                NewLine();
                Warn(Draw.Color, "Please check if the given input is valid.");
            }
        }

        /// <summary>
        /// Prints no input specified message.
        /// </summary>
        public void NoInputSpecified()
        {
            _noFilesFound = true;
            Error(Draw.PrefixAndColor, "If you specify a output you need to specify a input.");
        }

        /// <summary>
        /// Prints no configuration found message.
        /// </summary>
        public void NoConfigurationFound()
        {
            _noConfigFound = true;
            Error(Draw.PrefixAndColor, "No configuration found.");
            NewLine();
            Info(Draw.None, "Please execute the command " + "'ksc init'".Pastel(Color.DarkGray) + " to create a " + "ksconfig.json".Pastel(Color.DarkGray));
            Info(Draw.None, "or point to a file or directory containing " + "*.ks".Pastel(Color.DarkGray) + " files via the compile CLI options (" + "'ksc compile --help'".Pastel(Color.DarkGray) + ").");
            NewLine();
        }

        public void DrawSeperator()
        {
            NewLine();
        }
    }
}
