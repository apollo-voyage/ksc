using kOS.Safe;
using kOS.Safe.Persistence;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace kOS.Cli.IO
{
    public class CliVolumeDirectory : VolumeDirectory
    {
        private CliVolume volume;
        private string archivePath;

        public CliVolumeDirectory(CliVolume volume, VolumePath path) : base(volume, path)
        {
            this.volume = volume;
            this.archivePath = volume.GetArchivePath(path);
        }

        public override IDictionary<string, VolumeItem> List()
        {
            string[] files = Directory.GetFiles(archivePath);
            var filterHid = files.Where(f => (File.GetAttributes(f) & FileAttributes.Hidden) != 0);
            var filterSys = files.Where(f => (File.GetAttributes(f) & FileAttributes.System) != 0);
            var visFiles = files.Except(filterSys).Except(filterHid).ToArray();
            string[] directories = Directory.GetDirectories(archivePath);

            Array.Sort(directories);
            Array.Sort(visFiles);

            var result = new Dictionary<string, VolumeItem>();

            foreach (string directory in directories)
            {
                string directoryName = System.IO.Path.GetFileName(directory);
                result.Add(directoryName, new CliVolumeDirectory(volume, VolumePath.FromString(directoryName, Path)));
            }

            foreach (string file in visFiles)
            {
                string fileName = System.IO.Path.GetFileName(file);
                result.Add(fileName, new CliVolumeFile(volume, new FileInfo(file), VolumePath.FromString(fileName, Path)));
            }

            return result;
        }

        public override int Size {
            get {
                return List().Values.Aggregate(0, (acc, x) => acc + x.Size);
            }
        }
    }
}
