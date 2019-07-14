using System.Drawing;
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
    }
}
