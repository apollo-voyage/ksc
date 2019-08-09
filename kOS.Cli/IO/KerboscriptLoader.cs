using System.IO;
using System.Collections.Generic;
using kOS.Cli.Models;
using kOS.Cli.Options;
using kOS.Safe.Persistence;
using kOS.Cli.Logging;

namespace kOS.Cli.IO
{
    /// <summary>
    /// Loads Kerboscripts from disk.
    /// </summary>
    public class KerboscriptLoader
    {
        /// <summary>
        /// Compiler CLI options.
        /// </summary>
        private CompileOptions _compilerOptions;

        /// <summary>
        /// Compiler specific logger.
        /// </summary>
        private CompilerLogger _logger;

        /// <summary>
        /// Volume manager.
        /// </summary>
        private VolumeManager _volumeManager;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="volumeManager">Volume manager.</param>
        /// <param name="compilerOptions">Compiler ClI options.</param>
        /// <param name="deployOptions">Deployer ClI options.</param>
        /// <param name="logger">Compiler specific logger.</param>
        public KerboscriptLoader(VolumeManager volumeManager, CompilerLogger logger, CompileOptions compilerOptions)
        {
            _compilerOptions = compilerOptions;
            _volumeManager = volumeManager;
            _logger = logger;
        }

        /// <summary>
        /// Loads scripts based on a given configuration.
        /// </summary>
        /// <param name="config">Configuration to load from.</param>
        /// <returns>List of loaded Kerboscripts.</returns>
        public List<Kerboscript> LoadScriptsFromConfig(Configuration config)
        {
            List<Kerboscript> result = new List<Kerboscript>();
            foreach (var volume in config.GetVolumesForOption(_compilerOptions.Volume))
            {
                List<Kerboscript> scripts = LoadScriptsAndAddVolumes(volume.InputPath, volume.OutputPath, volume.Name);
                if (scripts != null)
                {
                    scripts.ForEach(ks => ks.DeployPath = volume.DeployPath.Replace(".", config.Archive));
                    result.AddRange(scripts);
                }
            }

            return result;
        }

        /// <summary>
        /// Loads scripts based on CLI options.
        /// </summary>
        /// <returns>List of loaded Kerboscripts.</returns>
        public List<Kerboscript> LoadScriptsFromOptions()
        {
            List<Kerboscript> result = new List<Kerboscript>();

            if (_compilerOptions.Input == Constants.CurrentDirectory &&
                _compilerOptions.Output != Constants.CurrentDirectory)
            {
                _logger.NoInputSpecified();
                return result;
            }

            if (_compilerOptions.Input != Constants.CurrentDirectory &&
                _compilerOptions.Output != Constants.CurrentDirectory)
            {
                List<Kerboscript> scripts = LoadScriptsAndAddVolumes(_compilerOptions.Input, _compilerOptions.Output);
                if (scripts != null)
                {
                    result.AddRange(scripts);
                }
            }
            else if (_compilerOptions.Output == Constants.CurrentDirectory)
            {
                if (Directory.Exists(_compilerOptions.Input) == false)
                {
                    _compilerOptions.Output = Path.ChangeExtension(_compilerOptions.Input, Constants.KerboscriptCompiledExtension);
                }

                List<Kerboscript> scripts = LoadScriptsAndAddVolumes(_compilerOptions.Input, _compilerOptions.Output);
                if (scripts != null)
                {
                    result.AddRange(scripts);
                }
            }

            return result;
        }

        /// <summary>
        /// Loads scripts and adds input and output volumes.
        /// </summary>
        /// <param name="input">Input path of the file or directory.</param>
        /// <param name="output">Output path of file or directory.</param>
        /// <param name="volumeName">Name of the volume to load the scripts into.</param>
        /// <returns>List of loaded Kerboscripts.</returns>
        private List<Kerboscript> LoadScriptsAndAddVolumes(string input, string output, string volumeName = "")
        {
            List<Kerboscript> result = null;
            List<Kerboscript> scripts = KerboscriptIO.Load(input, output);
            result = scripts != null ? AddVolumesToScripts(scripts, input, output, volumeName) : result;

            return result;
        }

        /// <summary>
        /// Adds input and output volumes to given scripts.
        /// </summary>
        /// <param name="scripts">Scripts to add volumes to.</param>
        /// <param name="input">Input path of the file or directory.</param>
        /// <param name="output">Output path of the file or directory.</param>
        /// <param name="volumeName">Name of the volume to load the scripts into.</param>
        /// <returns>List of loaded Kerboscripts.</returns>
        private List<Kerboscript> AddVolumesToScripts(List<Kerboscript> scripts, string input, string output, string volumeName = "")
        {
            if (scripts != null)
            {
                string inputPath = Path.GetFullPath(input);
                string outputPath = Path.GetFullPath(output);

                if (File.Exists(inputPath) == true)
                {
                    inputPath = Directory.GetParent(inputPath).FullName;
                    outputPath = Directory.GetParent(outputPath).FullName;
                }

                CliVolume volume = CreateVolume(inputPath, outputPath, volumeName);
                scripts.ForEach(ks => ks.Volume = volume);
            }

            return scripts;
        }

        /// <summary>
        /// Creates a volume.
        /// </summary>
        /// <param name="directory">Root directory of the volume.</param>
        /// <param name="name">Name of the volume.</param>
        /// <returns>Created volume.</returns>
        private CliVolume CreateVolume(string directory, string outputDirectory, string name)
        {
            CliVolume result = _volumeManager.GetVolume(name) as CliVolume;
            if (result == null) { 
                result = new CliVolume(directory, outputDirectory, name);
                _volumeManager.Add(result);
            }

            return result;
        }
    }
}
