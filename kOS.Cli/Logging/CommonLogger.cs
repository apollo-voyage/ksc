using System.Drawing;
using Pastel;

namespace kOS.Cli.Logging
{
    /// <summary>
    /// Common logger implementation.
    /// </summary>
    public class CommonLogger : Logger
    {
        /// <summary>
        /// Prints no configuration found message.
        /// </summary>
        public void NoConfigurationFound()
        {
            Error(Draw.PrefixAndColor, "No configuration found.");
            NewLine();
            Info(Draw.None, "Please execute the command " + "'ksc init'".Pastel(Color.DarkGray) + " to create a " + "ksconfig.json".Pastel(Color.DarkGray));
            Info(Draw.None, "or point to a file or directory containing " + "*.ks".Pastel(Color.DarkGray) + " files via the compile CLI options (" + "'ksc compile --help'".Pastel(Color.DarkGray) + ").");
            NewLine();
        }
    }
}
