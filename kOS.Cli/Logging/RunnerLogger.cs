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
        /// <param name="Script">Not found script.</param>
        public void ScriptNotFound(string Script)
        {
            Warn(Draw.PrefixAndColor, "A script named " + "'{0}'".Pastel(Color.DarkGray) + " could not be found.", Script);
            NewLine();
            Info(Draw.None, "Please check if the script " + "'{0}'".Pastel(Color.DarkGray) + " is defined in your " + "ksconfig.json.".Pastel(Color.DarkGray), Script);
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
        public void StartKerboscriptExecution(string Script)
        {
            Info("Executing Kerboscript: " + "'{0}'".Pastel(Color.DarkGray), Path.GetFileName(Script));
            ResetAndStartWatch();
        }

        /// <summary>
        /// Prints the stop script exec messages.
        /// </summary>
        /// <param name="Script"></param>
        public void StopKerboscriptExecution(string Script)
        {
            StopWatch();

            long elapsed = Elapsed > 1000 ? Elapsed / 1000 : Elapsed;
            string unit = Elapsed > 1000 ? "s" : "ms";
            Done(Draw.Prefix, "Script " + "'{0}'".Pastel(Color.DarkGray) + " executed in {1} {2}", Path.GetFileName(Script), elapsed, unit);
            NewLine();
        }

        public void PrintScriptOutput(List<string> Output)
        {
            if (Output.Count > 0)
            {
                Info("Output:");
                NewLine();
                foreach(string line in Output)
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
        /// <param name="ScriptFilepath">Filepath of the script that failed to compile.</param>
        public void CompilationError(Exception e, String ScriptFilepath)
        {
            NewLine();
            Error(Draw.PrefixAndColor, "in {0}", ScriptFilepath);
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
