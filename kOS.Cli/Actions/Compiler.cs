using System;
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
            _scriptLoader = new KerboscriptLoader(_volumeManager, _options);
            _logger = new CompilerLogger();
        }

        /// <summary>
        /// Runs the compiler.
        /// </summary>
        /// <returns></returns>
        public override int Run()
        {
            int result = 0;

            List<Kerboscript> scripts = LoadScripts();
            if (scripts != null)
            {
                bool compiled = Compile(scripts);
                if (_options.Write == true && compiled == true)
                {
                    result = WriteCompiledContent(scripts);
                } else if (compiled == false)
                {
                    result = 1;
                }
            }
            else
            {
                _logger.NoFilesFound(_options);
            }

            return result;
        }

        protected override Configuration LoadConfiguration()
        {
            Configuration result = null;

            if (_options.Input == Constants.CurrentDirectory)
            {
                result = base.LoadConfiguration();
            }

            return result;
        }

        private List<Kerboscript> LoadScripts()
        {
            List<Kerboscript> result;

            _logger.StartScriptLoading();
            {
                Configuration config = LoadConfiguration();
                if (config != null)
                {
                    result = _scriptLoader.LoadScriptsFromConfig(config);
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

        private bool Compile(List<Kerboscript> scripts, Configuration config = null)
        {
            bool result = true;

            _logger.StartCompilation();
            {
                foreach (Kerboscript script in scripts)
                {
                    GlobalPath inputPath = GetGlobalPath(script.InputArchive, script.InputPath);

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

        private int WriteCompiledContent(List<Kerboscript> Scripts)
        {
            foreach (Kerboscript script in Scripts)
            {
                GlobalPath outputPath = GetGlobalPath(script.OutputArchive, script.OutputPath);
                script.OutputArchive.SaveFile(outputPath, new FileContent(script.CompiledContent)); 
            }

            return 1;
        }

        private GlobalPath GetGlobalPath(Archive Archive, string Filepath)
        {
            int archiveId = _volumeManager.GetVolumeId(Archive);
            string path = string.Format("{0}:/" + Path.GetFileName(Filepath), archiveId);
            return _volumeManager.GlobalPathFromObject(path);
        }
    }
}
