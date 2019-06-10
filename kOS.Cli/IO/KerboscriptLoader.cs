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
        private CompileOptions _options;

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
        /// <param name="options">Compiler ClI options.</param>
        /// <param name="logger">Compiler specific logger.</param>
        public KerboscriptLoader(VolumeManager volumeManager, CompileOptions options, CompilerLogger logger)
        {
            _options = options;
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

            if (_options.Volume == Constants.AllVolumes)
            {
                foreach (var volume in config.Volumes)
                {
                    List<Kerboscript> scripts = LoadScriptsAndAddVolumes(volume.InputPath, volume.OutputPath, volume.Name);
                    if (scripts != null)
                    {
                        result.AddRange(scripts);
                    }
                }
            }
            else
            {
                Models.Volume volume = null;
                if (int.TryParse(_options.Volume, out int index) == true)
                {
                    volume = config.Volumes.Find(v => v.Index == index);
                }
                else
                {
                    volume = config.Volumes.Find(v => v.Name == _options.Volume);
                }

                if (volume != null)
                {
                    List<Kerboscript> scripts = LoadScriptsAndAddVolumes(volume.InputPath, volume.OutputPath, volume.Name);
                    if (scripts != null)
                    {
                        result.AddRange(scripts);
                    }
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

            if (_options.Input == Constants.CurrentDirectory &&
                _options.Output != Constants.CurrentDirectory)
            {
                _logger.NoInputSpecified();
                return result;
            }

            if (_options.Input != Constants.CurrentDirectory &&
                _options.Output != Constants.CurrentDirectory)
            {
                List<Kerboscript> scripts = LoadScriptsAndAddVolumes(_options.Input, _options.Output);
                if (scripts != null)
                {
                    result.AddRange(scripts);
                }
            }
            else if (_options.Output == Constants.CurrentDirectory)
            {
                _options.Output = Path.ChangeExtension(_options.Input, Constants.KerboscriptCompiledExtension);
                List<Kerboscript> scripts = LoadScriptsAndAddVolumes(_options.Input, _options.Output);
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

                CliVolume inputVolume = CreateVolume(inputPath, "in-" + volumeName);
                CliVolume outputVolume = CreateVolume(outputPath, "out-" + volumeName);
                scripts.ForEach(ks => ks.InputVolume = inputVolume);
                scripts.ForEach(ks => ks.OutputVolume = outputVolume);
            }

            return scripts;
        }

        /// <summary>
        /// Creates a volume.
        /// </summary>
        /// <param name="folder">Root folder of the volume.</param>
        /// <param name="name">Name of the volume.</param>
        /// <returns>Created volume.</returns>
        private CliVolume CreateVolume(string folder, string name)
        {
            CliVolume result = new CliVolume(folder, name);
            _volumeManager.Add(result);

            return result;
        }
    }
}
