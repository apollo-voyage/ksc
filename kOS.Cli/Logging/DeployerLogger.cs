using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kOS.Cli.Logging
{
    /// <summary>
    /// Logger implementation for the deploy action.
    /// </summary>
    public class DeployerLogger : Logger
    {
        /// <summary>
        /// Prints the start script deployment messages.
        /// </summary>
        public void StartScriptDeployment()
        {
            NewLine();
            Info("Deploying Kerboscript(s)...");
            ResetAndStartWatch();
        }

        /// <summary>
        /// Prints the stop script deployment messages.
        /// </summary>
        /// <param name="amount"></param>
        public void StopScriptLoading(int amount)
        {
            StopWatch();
            Done(Draw.PrefixAndColor, "{0} Kerboscript(s) deployed in {1} ms.", amount, Elapsed);
            NewLine();
        }
    }
}
