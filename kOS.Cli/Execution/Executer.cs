using System;
using System.IO;
using System.Collections.Generic;
using kOS.Cli.Models;
using kOS.Cli.Logging;
using kOS.Safe;
using kOS.Safe.Function;
using kOS.Safe.Execution;
using kOS.Safe.Utilities;
using kOS.Safe.Exceptions;
using kOS.Safe.Compilation;
using kOS.Safe.Persistence;
using kOS.Safe.Encapsulation;
using kOS.Safe.Compilation.KS;
using kOS.Cli.IO;

namespace kOS.Cli.Execution
{
    /// <summary>
    /// Script executer, this class encupsulates the creation of a kOS VM and the execution of a script on it.
    /// </summary>
    public class ScriptExecuter
    {
        /// <summary>
        /// Current action configuration.
        /// </summary>
        private Configuration _config;

        /// <summary>
        /// Shared safe objects.
        /// </summary>
        private SafeSharedObjects _shared;

        /// <summary>
        /// CPU that implements the kOS VM.
        /// </summary>
        private CPU _cpu;

        /// <summary>
        /// Logger.
        /// </summary>
        private RunnerLogger _logger;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="Logger">Logger.</param>
        /// <param name="Config">Current configuration.</param>
        public ScriptExecuter(RunnerLogger Logger, Configuration Config)
        {
            _config = Config;
            _logger = Logger;

            StaticSetup();
            Setup();
            AddVolumes();
            CreateCPU();
        }

        /// <summary>
        /// Executes a single script.
        /// </summary>
        /// <param name="Filepath">Filepath to the script to execute.</param>
        /// <returns>Output of the script.</returns>
        public List<string> ExecuteScript(string Filepath)
        {
            _shared.Screen.ClearScreen();
            List<CodePart> CompiledScript = CompileScript(Filepath);
            if (CompiledScript.Count > 0)
            {
                RunCompiledScript(CompiledScript);
            }

            return GetCastedScreen().Ouput;
        }

        /// <summary>
        /// Sets up all static variables needed to boot a CPU.
        /// </summary>
        private void StaticSetup()
        {
            SafeHouse.Init(new Config(), new VersionInfo(0, 0, 0, 0), "", true, _config.Archive);
            SafeHouse.Logger = new NoopLogger();

            try
            {
                AssemblyWalkAttribute.Walk();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine(e.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Sets up the shared safe object context.
        /// </summary>
        private void Setup()
        {
            _shared = new SafeSharedObjects();
            _shared.FunctionManager = new FunctionManager(_shared);
            _shared.GameEventDispatchManager = new NoopGameEventDispatchManager();
            _shared.Processor = new NoopProcessor();
            _shared.Interpreter = new NoopInterpreter();
            _shared.ScriptHandler = new KSScript();
            _shared.Screen = new Screen();
            _shared.UpdateHandler = new UpdateHandler();
            _shared.VolumeMgr = new VolumeManager();
            _shared.FunctionManager.Load();
        }

        /// <summary>
        /// Adds all volumes from the configuration, if present.
        /// </summary>
        private void AddVolumes()
        {
            if (_config != null)
            {
                Archive archive = new Archive(_config.Archive);
                _shared.VolumeMgr.Add(archive);

                foreach(Models.Volume volume in _config.Volumes)
                {
                    string fullVolumePath = Path.GetFullPath(volume.InputPath);
                    CliVolume cliVolume = new CliVolume(fullVolumePath, volume.Name);
                    _shared.VolumeMgr.Add(cliVolume);
                }
            }
        }

        /// <summary>
        /// Creates a CPU.
        /// </summary>
        private void CreateCPU()
        {
            _cpu = new CPU(_shared);
            _cpu.Boot();
        }

        /// <summary>
        /// Compiles a script.
        /// </summary>
        /// <param name="Filepath">Filepath to the script to compile.</param>
        /// <returns>Compiled script parts.</returns>
        private List<CodePart> CompileScript(string Filepath)
        {
            List<CodePart> result = new List<CodePart>();

            string content = File.ReadAllText(Filepath);
            GlobalPath path = _shared.VolumeMgr.GlobalPathFromObject("0:/" + Path.GetFileName(Filepath));

            try
            {
                result = _shared.ScriptHandler.Compile(path, 1, content, "ksc", new CompilerOptions() {
                    LoadProgramsInSameAddressSpace = true,
                    IsCalledFromRun = false,
                    FuncManager = _shared.FunctionManager
                });
            }
            catch (KOSParseException e)
            {
                _logger.CompilationError(e, Filepath);
            }

            return result;
        }

        /// <summary>
        /// Runs a compiled script.
        /// </summary>
        /// <param name="CompiledScript">Compiled script parts.</param>
        private void RunCompiledScript(List<CodePart> CompiledScript)
        {
            _cpu.GetCurrentContext().AddParts(CompiledScript);

            _shared.UpdateHandler.UpdateFixedObservers(0.01f);
            while (_cpu.InstructionsThisUpdate >= SafeHouse.Config.InstructionsPerUpdate)
            {
                _shared.UpdateHandler.UpdateFixedObservers(0.01f);
            }
        }

        /// <summary>
        /// Returns the custom screen already casted.
        /// </summary>
        /// <returns>Already casted custom screen.</returns>
        private Screen GetCastedScreen()
        {
            return _shared.Screen as Screen;
        }
    }
}
