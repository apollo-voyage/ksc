using kOS.Cli.Models;
using kOS.Cli.Options;
using System.IO;

namespace kOS.Cli.IO
{
    /// <summary>
    /// Delets compiled Kerboscripts from disk.
    /// </summary>
    public class KerboscriptDeleter
    {
        /// <summary>
        /// Compiler CLI options.
        /// </summary>
        private readonly CompileOptions _options;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="options"></param>
        public KerboscriptDeleter(CompileOptions options)
        {
            _options = options;
        }

        /// <summary>
        /// Removes compiled scripts based on either given options or configuration.
        /// </summary>
        /// <param name="config">Loaded project configuration.</param>
        public void RemoveCompiledScripts(SanitizedConfiguration config)
        {
            if (config != null)
            {
                foreach (var volume in config.GetVolumesForOption(_options.Volume))
                {
                    if(Directory.Exists(volume.OutputPath) == true)
                    {
                        Directory.Delete(volume.OutputPath, true);
                    }
                }
            }
            else
            {
                if (_options.Output != Constants.CurrentDirectory)
                {
                    if (Directory.Exists(_options.Output) == true)
                    {
                        Directory.Delete(_options.Output, true);
                    }
                    else if (File.Exists(_options.Output) == true)
                    {
                        File.Delete(_options.Output);
                    }
                }
            }
        }
    }
}
