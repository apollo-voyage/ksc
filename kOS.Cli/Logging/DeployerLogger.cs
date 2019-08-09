using kOS.Cli.Models;

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
            NewLine();
            ResetAndStartWatch();
        }

        /// <summary>
        /// Prints the stop script deployment messages.
        /// </summary>
        /// <param name="amount"></param>
        public void StopScriptDeployment(int amount)
        {
            StopWatch();
            NewLine();
            Done(Draw.PrefixAndColor, "{0} Kerboscript(s) deployed in {1} ms.", amount, Elapsed);
            NewLine();
        }

        /// <summary>
        /// Print the deploying script message.
        /// </summary>
        /// <param name="Script"></param>
        public void DeployingScript(Kerboscript Script)
        {
            Info(Draw.None, "Deploying {0}...", Script.InputPath);
        }
    }
}
