using System.Collections.Generic;
using System.IO;
using kOS.Cli.IO;
using kOS.Cli.Models;
using kOS.Cli.Options;
using kOS.Safe.Serialization;
using kOS.Safe.Compilation;
using kOS.Safe.Compilation.KS;
using kOS.Safe.Persistence;
using kOS.Safe.Exceptions;
using kOS.Cli.Logging;

namespace kOS.Cli.Actions
{
    /// <summary>
    /// Compiler action.
    /// </summary>
    public class Compiler : AbstractAction
    {
        /// <summary>
        /// Options for the compiler.
        /// </summary>
        private CompileOptions _options;

        /// <summary>
        /// Compiler options for the KSScript compiler.
        /// </summary>
        private CompilerOptions _compilerOptions;

        /// <summary>
        /// Volume manager.
        /// </summary>
        private VolumeManager _volumeManager;

        /// <summary>
        /// Script handler, which handles compiling.
        /// </summary>
        private KSScript _scriptHandler;

        /// <summary>
        /// Script loader, which handles loading Kerboscripts from disc.
        /// </summary>
        private KerboscriptLoader _scriptLoader;

        /// <summary>
        /// Logger.
        /// </summary>
        private CompilerLogger _logger;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="Options">Options for the compiler.</param>
        public Compiler(CompileOptions Options)
        {
            Opcode.InitMachineCodeData();
            CompiledObject.InitTypeData();
            SafeSerializationMgr.CheckIDumperStatics();

            _options = Options;
            _volumeManager = new VolumeManager();
            _scriptHandler = new KSScript();
            _compilerOptions = new CompilerOptions{ LoadProgramsInSameAddressSpace = true };
            _logger = new CompilerLogger();
            _scriptLoader = new KerboscriptLoader(_volumeManager, _options, _logger);
        }

        /// <summary>
        /// Runs the compiler.
        /// </summary>
        /// <returns></returns>
        public override int Run()
        {
            int result = 0;

            List<Kerboscript> scripts = LoadScripts();
            if (scripts != null && scripts.Count > 0)
            {
                result = Compile(scripts) ? WriteCompiledContent(scripts) : 1;
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
            Configuration result = null;

            if (_options.Input == Constants.CurrentDirectory)
            {
                result = base.LoadConfiguration();
            }

            return result;
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
                Configuration config = LoadConfiguration();
                if (_options.Input == Constants.CurrentDirectory && 
                    _options.Output == Constants.CurrentDirectory)
                {
                    if (config != null)
                    {
                        result = _scriptLoader.LoadScriptsFromConfig(config);
                    }
                    else
                    {
                        _logger.NoConfigurationFound();
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
            {
                foreach (Kerboscript script in scripts)
                {
                    GlobalPath inputPath = CreateGlobalPath(script.InputVolume, script.InputPath);

                    try
                    {
                        script.CompiledContent = _scriptHandler.Compile(inputPath, 1, script.Content, "ksc", _compilerOptions);
                    }
                    catch (KOSParseException e)
                    {
                        _logger.CompilationError(e, script);
                        result = false;
                    }
                }
            }
            _logger.StopCompilation(scripts.Count);

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
                GlobalPath outputPath = CreateGlobalPath(script.OutputVolume, script.OutputPath);
                script.OutputVolume.SaveFile(outputPath, new FileContent(script.CompiledContent)); 
            }

            return 1;
        }

        /// <summary>
        /// Creates a global volume path.
        /// </summary>
        /// <param name="volume">Volume for which to create the path.</param>
        /// <param name="filepath">Filepath on disk.</param>
        /// <returns>Created global path.</returns>
        private GlobalPath CreateGlobalPath(CliVolume volume, string filepath)
        {
            int archiveId = _volumeManager.GetVolumeId(volume);
            string path = string.Format("{0}:/" + Path.GetFileName(filepath), archiveId);
            return _volumeManager.GlobalPathFromObject(path);
        }
    }
}
