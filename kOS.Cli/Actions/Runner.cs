using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using kOS.Cli.Models;
using kOS.Cli.Options;
using kOS.Cli.Logging;
using kOS.Cli.Execution;
using kOS.Safe.Compilation;

using static kOS.Cli.Models.Script;

namespace kOS.Cli.Actions
{
    class Runner : AbstractAction
    {
        /// <summary>
        /// Run CLI options.
        /// </summary>
        private RunOptions _options;

        /// <summary>
        /// Logger.
        /// </summary>
        private RunnerLogger _logger;

        /// <summary>
        /// Common logger.
        /// </summary>
        private CommonLogger _commonLogger;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="Options">Run CLI options.</param>
        public Runner(RunOptions Options)
        {
            _options = Options;
            _logger = new RunnerLogger();
            _commonLogger = new CommonLogger();
        }

        /// <summary>
        /// Runs the run action.
        /// </summary>
        /// <returns></returns>
        public override int Run()
        {
            int result = 0;

            Configuration config = LoadConfiguration();
            if (IsKerboscript(_options.Script) == true)
            {
                string fullScriptPath = Path.GetFullPath(_options.Script);
                _logger.StartKerboscriptExecution(fullScriptPath);
                result = ExecuteKerboscript(fullScriptPath, config);
                if (result == 0)
                {
                    _logger.StopKerboscriptExecution(fullScriptPath);
                }
            }
            else
            {
                if (config != null) { 
                    Models.Script script = config.Scripts.Find(s => s.Name == _options.Script);
                    if (script != null)
                    {
                        result = ExecuteConfigScript(script);
                    }
                    else
                    {
                        _logger.ScriptNotFound(_options.Script);
                        result = 1;
                    }
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
        /// Executes a script from the configuration.
        /// </summary>
        /// <param name="Script"></param>
        /// <returns>CLI return code.</returns>
        private int ExecuteConfigScript(Models.Script Script)
        {
            int result = 0;

            List<ScriptPart> scriptParts = Script.GetScriptParts();
            foreach(ScriptPart part in scriptParts)
            {
                if (part.Program != Constants.ProgramKsc)
                {
                    _logger.StartExternalProcessExecution(part);
                    result = StartExternalProcess(part);
                    if (result != 0)
                    {
                        break;
                    }
                    _logger.StopExternalProcessExecution(part);
                }
                else
                {
                    result = ActionDispatcher.Dispatch(part.Arguments.Split(' '), false);
                }
            }

            return result;
        }

        /// <summary>
        /// Executes a Kerboscript.
        /// </summary>
        /// <param name="Filepath">Filepath to the Kerboscript to execute.</param>
        /// <returns>CLI return code.</returns>
        private int ExecuteKerboscript(string Filepath, Configuration Config)
        {
            int result = 0;

            Executer executer = new Executer(_logger, Config);
            List<string> output = executer.ExecuteScript(Filepath);
            if (executer.Error == true)
            {
                result = 1;
            }
            else
            {
                _logger.PrintScriptOutput(output);
            }

            return result;
        }

        /// <summary>
        /// Checks if a given script string is a Kerboscript on disk.
        /// </summary>
        /// <param name="Script">Script string to check.</param>
        /// <returns>True if it is a Kerboscript, false if it's not.</returns>
        private bool IsKerboscript(string Script)
        {
            bool result = false;

            string fullScriptPath = Path.GetFullPath(Script);
            result = File.Exists(fullScriptPath);

            return result;
        }

        /// <summary>
        /// Starts an external process.
        /// </summary>
        /// <param name="scriptPart">Script part which represents the external process to start.</param>
        /// <returns>Cli Return Code</returns>
        private int StartExternalProcess(ScriptPart scriptPart)
        {
            int result = 0;

            try
            {
                Process proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = scriptPart.Program,
                        Arguments = scriptPart.Arguments,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                };

                proc.Start();
                _logger.NewLine();
                while (!proc.StandardOutput.EndOfStream)
                {
                    string line = proc.StandardOutput.ReadLine();
                    _logger.PrintExternalScriptOutput(line);
                }
                _logger.NewLine();
            }
            catch (Exception e)
            {
                _logger.ExternalScriptException(e);

                result = 1;
            }

            return result;
        }
    }
}
