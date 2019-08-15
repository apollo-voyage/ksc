using System.IO;
using System.Collections.Generic;
using kOS.Cli.IO;
using kOS.Cli.Models;
using kOS.Cli.Options;
using kOS.Cli.Logging;
using kOS.Cli.Execution;
using kOS.Safe;
using kOS.Safe.Function;
using kOS.Safe.Exceptions;
using kOS.Safe.Persistence;
using kOS.Safe.Serialization;
using kOS.Safe.Compilation;
using kOS.Safe.Compilation.KS;

namespace kOS.Cli.Actions
{
    /// <summary>
    /// Compiler action. Runs when the user uses "ksc compile".
    /// </summary>
    class CompileAction : AbstractAction
    {
        /// <summary>
        /// Options for the compiler.
        /// </summary>
        private readonly CompileOptions _options;

        /// <summary>
        /// Compiler options for the KSScript compiler.
        /// </summary>
        private readonly CompilerOptions _compilerOptions;

        /// <summary>
        /// Shared safe objects.
        /// </summary>
        private readonly SafeSharedObjects _shared;

        /// <summary>
        /// Script loader, which handles loading Kerboscripts from disk.
        /// </summary>
        private readonly KerboscriptLoader _scriptLoader;

        /// <summary>
        /// Script delete, which handles the deletion of compiled Kerboscripts from disk.
        /// </summary>
        private readonly KerboscriptDeleter _scriptDeleter;

        /// <summary>
        /// Logger.
        /// </summary>
        private readonly CompilerLogger _logger;

        /// <summary>
        /// Common logger.
        /// </summary>
        private readonly CommonLogger _commonLogger;

        /// <summary>
        /// Loaded project congfiguration.
        /// </summary>
        private Configuration _config;

        /// <summary>
        /// Previous successfully compiled scripts.
        /// </summary>
        public List<Kerboscript> CompiledScripts { get; private set; }

        /// <summary>
        /// Flag, wheter the compiler is being called from the watcher.
        /// </summary>
        private readonly bool _usedExternally;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="options">Options for the compiler.</param>
        public CompileAction(CompileOptions options, bool usedExternally = false)
        {
            Opcode.InitMachineCodeData();
            CompiledObject.InitTypeData();
            SafeSerializationMgr.CheckIDumperStatics();

            _usedExternally = usedExternally;
            _options = options;

            _shared = new SafeSharedObjects();
            _shared.FunctionManager = new FunctionManager(_shared);
            _shared.GameEventDispatchManager = new NoopGameEventDispatchManager();
            _shared.Processor = new NoopProcessor();
            _shared.ScriptHandler = new KSScript();
            _shared.Screen = new Screen();
            _shared.UpdateHandler = new UpdateHandler();
            _shared.VolumeMgr = new VolumeManager();
            _shared.FunctionManager.Load();

            _compilerOptions = new CompilerOptions()
            {
                LoadProgramsInSameAddressSpace = false,
                IsCalledFromRun = false,
                FuncManager = _shared.FunctionManager
            };

            _logger = new CompilerLogger();
            _commonLogger = new CommonLogger();
            _scriptLoader = new KerboscriptLoader(_shared.VolumeMgr as VolumeManager, _logger, _options);
            _scriptDeleter = new KerboscriptDeleter(_options);
        }

        /// <summary>
        /// Runs the compiler.
        /// </summary>
        /// <returns></returns>
        public override int Run()
        {
            int result = 0;

            if (_usedExternally == true)
            {
                _logger.NewLine();
            }

            List<Kerboscript> scripts = LoadScripts();
            if (scripts != null && scripts.Count > 0)
            {
                _scriptDeleter.RemoveCompiledScripts(_config);
                result = Compile(scripts) ? WriteCompiledContent(scripts) : 1;
                CompiledScripts = result == 1 ? null : scripts;
            }
            else
            {
                _logger.NoFilesFound(_options);
            }

            return result;
        }

        /// <summary>
        /// Loads the configuration from disk.
        /// </summary>
        /// <returns>Loaded configuration, if found on disk.</returns>
        protected override Configuration LoadConfiguration()
        {
            if (_config == null)
            {
                if (_options.Input == Constants.CurrentDirectory)
                {
                    _config = base.LoadConfiguration();
                }
                else
                {
                    string fullPath = Path.GetFullPath(_options.Input);
                    if (Directory.Exists(fullPath) == true)
                    {
                        string configPath = Path.Combine(fullPath, Constants.ConfigFileName);
                        _config = ConfigIO.ReadConfigFile(configPath);

                        foreach(Models.Volume volume in _config.Volumes)
                        {
                            volume.InputPath = volume.InputPath.Replace(".", _options.Input);
                            volume.OutputPath = volume.OutputPath.Replace(".", _options.Output);
                        }
                    }
                }
            }

            return _config;
        }

        /// <summary>
        /// Loads all scripts to compiler based on either the configuration or the CLI options.
        /// </summary>
        /// <returns>All found scripts.</returns>
        private List<Kerboscript> LoadScripts()
        {
            List<Kerboscript> result = null;

            _logger.StartScriptLoading();
            {
                if ((_options.Input == Constants.CurrentDirectory && 
                    _options.Output == Constants.CurrentDirectory) || 
                    ConfigIO.IsDirectory(_options.Input) == true)
                {
                    if (_options.Output == Constants.CurrentDirectory)
                    {
                        _options.Output = _options.Input;
                    }

                    Configuration config = LoadConfiguration();
                    if (config != null)
                    {
                        result = _scriptLoader.LoadScriptsFromConfig(config);
                    }
                    else
                    {
                        _commonLogger.NoConfigurationFound();
                    }
                }
                else
                {
                    result = _scriptLoader.LoadScriptsFromOptions();
                }
            }

            if (result != null)
            {
                _logger.StopScriptLoading(result.Count);
            }

            return result;
        }

        /// <summary>
        /// Compiles scripts.
        /// </summary>
        /// <param name="scripts">Scripts to compile.</param>
        /// <returns>Flag, wheter the compilation has encountered errors or not.</returns>
        private bool Compile(List<Kerboscript> scripts)
        {
            bool result = true;

            _logger.StartCompilation();
            foreach (Kerboscript script in scripts)
            {
                GlobalPath inputPath = CreateGlobalPath(script.Volume, script.InputPath);

                try
                {
                    script.CompiledContent = _shared.ScriptHandler.Compile(inputPath, 1, script.Content, "ksc", _compilerOptions);
                }
                catch (KOSParseException e)
                {
                    _logger.CompilationError(e, script);
                    result = false;
                }
            }

            if (result != false)
            {
                _logger.StopCompilationSuccess(scripts.Count);
            }
            else
            {
                _logger.StopCompilationFailure();
            }

            return result;
        }

        /// <summary>
        /// Writes the compile content to disk.
        /// </summary>
        /// <param name="scripts">Scripts for which the compiled content should be written.</param>
        /// <returns>CLI return code.</returns>
        private int WriteCompiledContent(List<Kerboscript> scripts)
        {
            foreach (Kerboscript script in scripts)
            {
                GlobalPath outputPath = CreateGlobalPath(script.Volume, script.OutputPath);

                script.Volume.Mode = CliVolumeMode.Output;
                script.Volume.SaveFile(outputPath, new FileContent(script.CompiledContent));
                script.Volume.Mode = CliVolumeMode.Root;
            }

            return 0;
        }

        /// <summary>
        /// Creates a global volume path.
        /// </summary>
        /// <param name="volume">Volume for which to create the path.</param>
        /// <param name="filepath">Filepath on disk.</param>
        /// <returns>Created global path.</returns>
        private GlobalPath CreateGlobalPath(CliVolume volume, string filepath)
        {
            int archiveId = _shared.VolumeMgr.GetVolumeId(volume);
            string path = string.Format("{0}:/" + Path.GetFileName(filepath), archiveId);
            return _shared.VolumeMgr.GlobalPathFromObject(path);
        }
    }
}
