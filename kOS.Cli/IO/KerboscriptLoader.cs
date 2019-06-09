using System;
using System.IO;
using System.Collections.Generic;
using kOS.Cli.Models;
using kOS.Cli.Options;
using kOS.Safe.Persistence;

namespace kOS.Cli.IO
{
    public class KerboscriptLoader
    {
        private CompileOptions _options;

        private VolumeManager _volumeManager;
        public KerboscriptLoader(VolumeManager volumeManager, CompileOptions options)
        {
            _options = options;
            _volumeManager = volumeManager;
        }

        public List<Kerboscript> LoadScriptsFromConfig(Configuration config)
        {
            List<Kerboscript> result = new List<Kerboscript>();

            if (_options.Volume == Constants.AllVolumes)
            {
                foreach (var volume in config.Volumes)
                {
                    List<Kerboscript> scripts = KerboscriptIO.Load(volume.OutputPath, Constants.DistDirectory);
                    if (scripts != null)
                    {
                        Archive inputArchive = CreateArchive(volume.InputPath);
                        Archive outputArchive = CreateArchive(volume.OutputPath);
                        scripts.ForEach(ks => ks.InputArchive = inputArchive);
                        scripts.ForEach(ks => ks.OutputArchive = outputArchive);
                        result.AddRange(scripts);
                    }
                }
            }
            else
            {
                var volume = config.Volumes.Find(v => v.Name == _options.Volume);
                if (volume != null)
                {
                    List<Kerboscript> scripts = KerboscriptIO.Load(volume.OutputPath, Constants.DistDirectory);
                    if (scripts != null)
                    {
                        Archive inputArchive = CreateArchive(volume.InputPath);
                        Archive outputArchive = CreateArchive(volume.OutputPath);
                        scripts.ForEach(ks => ks.InputArchive = inputArchive);
                        scripts.ForEach(ks => ks.OutputArchive = outputArchive);
                        result.AddRange(scripts);
                    }
                }
            }

            return result;
        }

        public List<Kerboscript> LoadScriptsFromOptions()
        {
            List<Kerboscript> result = null;

            if (_options.Input == Constants.CurrentDirectory &&
                _options.Output != Constants.CurrentDirectory)
            {
                Console.WriteLine("If you specify a output you need to specify a input.");
                return result;
            }

            if (_options.Input != Constants.CurrentDirectory &&
                _options.Output != Constants.CurrentDirectory)
            {
                result = KerboscriptIO.Load(_options.Input, _options.Output);
                result = AddArchivesToScripts(result);
            }
            else if (_options.Output == Constants.CurrentDirectory)
            {
                _options.Output = Path.ChangeExtension(_options.Input, Constants.KerboscriptCompiledExtension);
                result = KerboscriptIO.Load(_options.Input, _options.Output);
                result = AddArchivesToScripts(result);
            }

            return result;
        }

        private Archive CreateArchive(string Folder)
        {
            Archive result = new Archive(Folder);
            _volumeManager.Add(result);

            return result;
        }

        private List<Kerboscript> AddArchivesToScripts(List<Kerboscript> Scripts)
        {
            if (Scripts != null)
            {
                string inputPath = Path.GetFullPath(_options.Input);
                string outputPath = Path.GetFullPath(_options.Output);

                if (Scripts.Count == 1)
                {
                    inputPath = Directory.GetParent(inputPath).FullName;
                    outputPath = Directory.GetParent(outputPath).FullName;
                }

                Archive inputArchive = CreateArchive(inputPath);
                Archive outputArchive = CreateArchive(outputPath);
                Scripts.ForEach(ks => ks.InputArchive = inputArchive);
                Scripts.ForEach(ks => ks.OutputArchive = outputArchive);
            }

            return Scripts;
        }
    }
}
