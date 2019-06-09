using kOS.Cli.IO;
using kOS.Cli.Models;

namespace kOS.Cli.Actions
{
    /// <summary>
    /// Abstract action.
    /// </summary>
    public abstract class AbstractAction : IAction
    {
        /// <summary>
        /// Runs the action.
        /// </summary>
        /// <returns>CLI return code.</returns>
        public abstract int Run();

        /// <summary>
        /// Tries to load the configuration from the current directory.
        /// </summary>
        /// <returns>Read configuration.</returns>
        virtual protected Configuration LoadConfiguration()
        {
            return ConfigIO.ReadConfigFileFromCurrentDirectory();
        }
    }
}
