using System;
using System.Drawing;
using System.Collections.Generic;
using Pastel;
using static kOS.Cli.Models.Script;
using System.IO;

namespace kOS.Cli.Logging
{
    /// <summary>
    /// Logger implementation for the run action.
    /// </summary>
    public class RunnerLogger : Logger
    {
        /// <summary>
        /// Prints the script not found messages.
        /// </summary>
        /// <param name="script">Not found script.</param>
        public void ScriptNotFound(string script)
        {
            Warn(Draw.PrefixAndColor, "A script named " + "'{0}'".Pastel(Color.DarkGray) + " could not be found.", script);
            NewLine();
            Info(Draw.None, "Please check if the script " + "'{0}'".Pastel(Color.DarkGray) + " is defined in your " + Constants.ConfigFileName.Pastel(Color.DarkGray), script);
        }

        public void StartExternalProcessExecution(ScriptPart scriptPart)
        {
            Info("Executing external process: " + "'{0} {1}'".Pastel(Color.DarkGray), scriptPart.Program, scriptPart.Arguments);
            ResetAndStartWatch();
        }

        public void StopExternalProcessExecution(ScriptPart scriptPart)
        {
            StopWatch();

            long elapsed = Elapsed > 1000 ? Elapsed / 1000 : Elapsed;
            string unit = Elapsed > 1000 ? "s" : "ms";
            Done(Draw.Prefix, "External process " + "'{0}'".Pastel(Color.DarkGray) + " executed in {1} {2}", scriptPart.Program, elapsed, unit);
            NewLine();
        }

        /// <summary>
        /// Prints the start script exec messages.
        /// </summary>
        public void StartKerboscriptExecution(string script)
        {
            Info("Executing Kerboscript: " + "'{0}'".Pastel(Color.DarkGray), Path.GetFileName(script));
            ResetAndStartWatch();
        }

        /// <summary>
        /// Prints the stop script exec messages.
        /// </summary>
        /// <param name="script"></param>
        public void StopKerboscriptExecution(string script)
        {
            StopWatch();

            long elapsed = Elapsed > 1000 ? Elapsed / 1000 : Elapsed;
            string unit = Elapsed > 1000 ? "s" : "ms";
            Done(Draw.Prefix, "Script " + "'{0}'".Pastel(Color.DarkGray) + " executed in {1} {2}", Path.GetFileName(script), elapsed, unit);
            NewLine();
        }

        public void PrintScriptOutput(List<string> output)
        {
            if (output.Count > 0)
            {
                NewLine();
                foreach(string line in output)
                {
                    Info(Draw.None, "   " + line);
                }
                NewLine();
            }
        }

        /// <summary>
        /// Prints a compilation error.
        /// </summary>
        /// <param name="e">Compilation exception.</param>
        /// <param name="scriptFilepath">Filepath of the script that failed to compile.</param>
        public void CompilationError(Exception e, String scriptFilepath)
        {
            NewLine();
            Error(Draw.PrefixAndColor, "in {0}", scriptFilepath);
            Error(Draw.Color, e.Message);
        }

        /// <summary>
        /// Prints the output for a external script.
        /// </summary>
        /// <param name="line"></param>
        public void PrintExternalScriptOutput(string line)
        {
            Info(Draw.None, "   " + line);
        }

        /// <summary>
        /// Prints a exception for a external script.
        /// </summary>
        /// <param name="e"></param>
        public void ExternalScriptException(Exception e)
        {
            StopWatch();
            NewLine();
            Error(Draw.Color, e.Message);
        }
    }
}
