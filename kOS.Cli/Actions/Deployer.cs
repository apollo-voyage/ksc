using kOS.Cli.Logging;
using kOS.Cli.Models;
using kOS.Cli.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kOS.Cli.Actions
{
    public class Deployer : AbstractAction
    {
        private DeployOptions _options;

        private Compiler _compiler;

        private CompilerLogger _compilerLogger;


        public Deployer(DeployOptions options)
        {
            _options = options;
            _compiler = new Compiler(CompileOptions.FromDeployOptions(options), true);
            _compilerLogger = new CompilerLogger();
        }

        public override int Run()
        {
            int result = 0;

            if (_compiler.Run() == 0)
            {
                Configuration config = LoadConfiguration();
                result = config == null ? DeployFromOptions(_compiler.CompiledScripts) : 
                                          DeployFromConfig(_compiler.CompiledScripts, config);
            }

            return result;
        }

        /// <summary>
        /// Loads the configuration from disk.
        /// </summary>
        /// <returns>Loaded configuration, if found on disk.</returns>
        protected override Configuration LoadConfiguration()
        {
            Configuration result = null;
            //if (_options.Input == Constants.CurrentDirectory)
            //{
            //    result = base.LoadConfiguration();
            //}

            return result;
        }

        private int DeployFromConfig(List<Kerboscript> scripts, Configuration config)
        {
            int result = 0;

            return result;
        }

        private int DeployFromOptions(List<Kerboscript> scripts)
        {
            int result = 0;

            return result;
        }
    }
}
