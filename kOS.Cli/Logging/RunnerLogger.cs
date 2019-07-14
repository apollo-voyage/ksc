using System;
using System.Drawing;
using System.Collections.Generic;
using Pastel;

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

        /// <summary>
        /// Prints the start script exec messages.
        /// </summary>
        public void StartScriptExecution()
        {
            Info("Executing script...");
            ResetAndStartWatch();
        }

        /// <summary>
        /// Prints the stop script exec messages.
        /// </summary>
        /// <param name="Script"></param>
        public void StopScriptExecution(string Script)
        {
            StopWatch();

            long elapsed = Elapsed > 1000 ? Elapsed / 1000 : Elapsed;
            string unit = Elapsed > 1000 ? "s" : "ms";
            Done(Draw.Prefix, "Script {0} executed in {1} {2}", Script, elapsed, unit);
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
    }
}
