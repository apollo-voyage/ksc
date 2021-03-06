﻿using System.IO;
using System.Collections.Generic;
using kOS.Cli.Logging;
using kOS.Cli.Models;
using kOS.Cli.Options;

namespace kOS.Cli.Actions
{
    /// <summary>
    /// Deploy action. Runs when the user users "ksc deploy".
    /// </summary>
    class DeployAction : AbstractAction
    {
        /// <summary>
        /// Deploy CLI options.
        /// </summary>
        private readonly DeployOptions _options;

        /// <summary>
        /// Compiler. Implements compile action, will be used to compile after file changes.
        /// </summary>
        private readonly CompileAction _compiler;

        /// <summary>
        /// Logger.
        /// </summary>
        private readonly DeployerLogger _logger;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="options">Deploy CLI options.</param>
        public DeployAction(DeployOptions options)
        {
            _options = options;
            _compiler = new CompileAction(CompileOptions.FromDeployOptions(options), true);
            _logger = new DeployerLogger();
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
                SanitizedConfiguration config = LoadConfiguration();
                if (config != null)
                {
                    _logger.StartScriptDeployment();
                    result = Deploy(_compiler.CompiledScripts, config);
                    _logger.StopScriptDeployment(_compiler.CompiledScripts.Count);
                }
            }

            return result;
        }

        /// <summary>
        /// Deploys the scripts based on the options and given configuration.
        /// </summary>
        /// <param name="scripts">Scripts to deploy.</param>
        /// <param name="config">Configuration used for the deployment.</param>
        /// <returns>CLI return code.</returns>
        private int Deploy(List<Kerboscript> scripts, SanitizedConfiguration config)
        {
            int result = 0;

            List<Volume> volumes = config.GetVolumesForOption(_options.Volume);
            foreach(Volume volume in volumes)
            { 
                string deployDirectory = volume.DeployPath.Replace(Constants.CurrentDirectory, config.Archive);
                if (deployDirectory != config.Archive && Directory.Exists(deployDirectory))
                {
                    Directory.Delete(deployDirectory, true);
                }
                else
                {
                    string[] files = Directory.GetFiles(deployDirectory);
                    foreach(string file in files)
                    {
                        File.Delete(file);
                    }
                }
            }

            foreach(Kerboscript script in scripts)
            {
                _logger.DeployingScript(script);

                string fileName   = _options.DeploySource ? Path.GetFileName(script.InputPath) : Path.GetFileName(script.OutputPath);
                string deployPath = Path.Combine(script.DeployPath, fileName);

                Directory.CreateDirectory(Path.GetDirectoryName(deployPath));
                File.Copy(script.OutputPath, deployPath, true);
            }

            return result;
        }
    }
}
