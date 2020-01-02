using kOS.Cli.IO;
using kOS.Cli.Logging;
using kOS.Cli.Models;
using System.Collections.Generic;

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
        /// Common logger.
        /// </summary>
        protected readonly CommonLogger _commonLogger = new CommonLogger();

        /// <summary>
        /// Tries to load the configuration from the current directory.
        /// </summary>
        /// <returns>Read configuration.</returns>
        virtual protected SanitizedConfiguration LoadConfiguration()
        {
            SanitizedConfiguration result;

            result = ConfigIO.ReadConfigFileFromCurrentDirectory();
            if (result == null)
            {
                _commonLogger.NoConfigurationFound();
            }

            return result;
        }
    }
}
