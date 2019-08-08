using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using kOS.Cli.Logging;
using kOS.Cli.Models;
using kOS.Cli.Options;

namespace kOS.Cli.Actions
{
    class Deployer : AbstractAction
    {
        /// <summary>
        /// Deploy CLI options.
        /// </summary>
        private DeployOptions _options;

        /// <summary>
        /// Compiler. Implements compile action, will be used to compile after file changes.
        /// </summary>
        private Compiler _compiler;

        /// <summary>
        /// Logger.
        /// </summary>
        private DeployerLogger _logger;

        /// <summary>
        /// Common logger.
        /// <summary>
        private CommonLogger _commonLogger;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="Options">Deploy CLI options.</param>
        public Deployer(DeployOptions Options)
        {
            _options = Options;
            _compiler = new Compiler(CompileOptions.FromDeployOptions(Options), true);
            _logger = new DeployerLogger();
            _commonLogger = new CommonLogger();
        }

        /// <summary>
        /// Runs the deployment action.
        /// </summary>
        /// <returns></returns>
        public override int Run()
        {
            int result = 0;

            if (_compiler.Run() == 0)
            {
                Configuration config = LoadConfiguration();
                if (config != null)
                {
                    _logger.StartScriptDeployment();
                    result = Deploy(_compiler.CompiledScripts, config);
                    _logger.StopScriptDeployment(_compiler.CompiledScripts.Count);
                }
                else
                {
                    _commonLogger.NoConfigurationFound();
                    result = 1;
                }
            }

            return result;
        }

        /// <summary>
        /// Loads the configuration from disk.
        /// </summary>
        /// <returns>Loaded configuration, if found on disk.</returns>
        protected override Configuration LoadConfiguration()
        {
            return base.LoadConfiguration();
        }

        /// <summary>
        /// Deploys the scripts based on the options and given configuration.
        /// </summary>
        /// <param name="Scripts">Scripts to deploy.</param>
        /// <param name="Config">Configuration used for the deployment.</param>
        /// <returns>CLI return code.</returns>
        private int Deploy(List<Kerboscript> Scripts, Configuration Config)
        {
            int result = 0;

            foreach(Kerboscript script in Scripts)
            {
                _logger.DeployingScript(script);

                string fileName   = _options.DeploySource ? Path.GetFileName(script.InputPath) : Path.GetFileName(script.OutputPath);
                string deployPath = Path.Combine(script.DeployPath, fileName);
                File.Copy(script.OutputPath, deployPath, true);
            }

            return result;
        }
    }
}
