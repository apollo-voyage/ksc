using System;
using System.IO;
using kOS.Safe;
using kOS.Safe.Utilities;
using kOS.Safe.Exceptions;
using kOS.Safe.Persistence;

namespace kOS.Cli.IO
{
    public enum CliVolumeMode
    {
        Root,
        Output
    }

    [KOSNomenclature("CliVolume")]
    public class CliVolume : Volume
    {
        public const string ArchiveName = "Archive";
        public CliVolumeDirectory RootVolumeDirectory { get; private set; }
        public CliVolumeDirectory OutputVolumeDirectory { get; private set; }

        public CliVolumeMode Mode = CliVolumeMode.Root;

        private string VolumeFolder {
            get {
                return Mode == CliVolumeMode.Root ? _volumeFolder : _outputFolder;
            }

            set {
                switch (Mode)
                {
                    case CliVolumeMode.Root: _volumeFolder = value;
                        break;
                    case CliVolumeMode.Output: _outputFolder = value;
                        break;
                }
            }
        }

        private string _volumeFolder;
        private string _outputFolder;

        public override VolumeDirectory Root
        {
            get
            {
                return RootVolumeDirectory;
            }
        }

        public CliVolume(string folder, string outputFolder, string name = ArchiveName)
        {
            _volumeFolder = Path.GetFullPath(folder).TrimEnd(VolumePath.PathSeparator);
            _outputFolder = Path.GetFullPath(outputFolder).TrimEnd(VolumePath.PathSeparator);

            CreateVolumeDirectories();
            Renameable = false;
            InitializeName(name);

            Mode = CliVolumeMode.Root;
            RootVolumeDirectory = new CliVolumeDirectory(this, VolumePath.EMPTY);
            Mode = CliVolumeMode.Output;
            OutputVolumeDirectory = new CliVolumeDirectory(this, VolumePath.EMPTY);
            Mode = CliVolumeMode.Root;
        }

        public CliVolume(string folder, string name = ArchiveName)
        {
            _volumeFolder = Path.GetFullPath(folder).TrimEnd(VolumePath.PathSeparator);

            CreateVolumeDirectories();
            Renameable = false;
            InitializeName(name);

            RootVolumeDirectory = new CliVolumeDirectory(this, VolumePath.EMPTY);
        }

        private void CreateVolumeDirectories()
        {
            Mode = CliVolumeMode.Root;
            Directory.CreateDirectory(VolumeFolder);

            Mode = CliVolumeMode.Output;
            if (VolumeFolder != null && VolumeFolder != "")
            {
                Directory.CreateDirectory(VolumeFolder);
            }

            Mode = CliVolumeMode.Root;
        }

        public string GetArchivePath(VolumePath path)
        {
            if (path.PointsOutside)
            {
                throw new KOSInvalidPathException("Path refers to parent directory", path.ToString());
            }

            string mergedPath = VolumeFolder;
            foreach (string segment in path.Segments)
            {
                mergedPath = Path.Combine(mergedPath, segment);
            }

            string fullPath = Path.GetFullPath(mergedPath);
            if (!fullPath.StartsWith(VolumeFolder, StringComparison.Ordinal))
            {
                throw new KOSInvalidPathException("Path refers to parent directory", path.ToString());
            }

            return fullPath;
        }

        public override void Clear()
        {
            if (Directory.Exists(VolumeFolder))
            {
                Directory.Delete(VolumeFolder, true);
            }

            Directory.CreateDirectory(VolumeFolder);
        }

        public override VolumeItem Open(VolumePath path, bool ksmDefault = false)
        {
            try
            {
                var fileSystemInfo = Search(path, ksmDefault);

                if (fileSystemInfo == null)
                {
                    return null;
                }
                else if (fileSystemInfo is FileInfo)
                {
                    VolumePath filePath = VolumePath.FromString(fileSystemInfo.FullName.Substring(VolumeFolder.Length).Replace(Path.DirectorySeparatorChar, VolumePath.PathSeparator));
                    return new CliVolumeFile(this, fileSystemInfo as FileInfo, filePath);
                }
                else
                {
                    // we can use 'path' here, default extensions are not added to directories
                    return new CliVolumeDirectory(this, path);
                }
            }
            catch (Exception e)
            {
                throw new KOSPersistenceException("Could not open path: " + path, e);
            }
        }

        public override VolumeDirectory CreateDirectory(VolumePath path)
        {
            string archivePath = GetArchivePath(path);

            if (Directory.Exists(archivePath))
            {
                throw new KOSPersistenceException("Already exists: " + path);
            }

            try
            {
                Directory.CreateDirectory(archivePath);
            }
            catch (IOException)
            {
                throw new KOSPersistenceException("Could not create directory: " + path);
            }

            return new CliVolumeDirectory(this, path);
        }

        public override VolumeFile CreateFile(VolumePath path)
        {
            if (path.Depth == 0)
            {
                throw new KOSPersistenceException("Can't create a file over root directory");
            }

            string archivePath = GetArchivePath(path);

            if (File.Exists(archivePath))
            {
                throw new KOSPersistenceException("Already exists: " + path);
            }

            try
            {
                Directory.CreateDirectory(GetArchivePath(path.GetParent()));
            }
            catch (IOException)
            {
                throw new KOSPersistenceException("Parent directory for path does not exist: " + path.ToString());
            }

            try
            {
                File.Create(archivePath).Dispose();
            }
            catch (UnauthorizedAccessException)
            {
                throw new KOSPersistenceException("Could not create file: " + path);
            }

            return Open(path) as VolumeFile;
        }

        public override bool Exists(VolumePath path, bool ksmDefault = false)
        {
            return Search(path, ksmDefault) != null;
        }

        public override bool Delete(VolumePath path, bool ksmDefault = false)
        {
            if (path.Depth == 0)
            {
                throw new KOSPersistenceException("Can't delete root directory");
            }

            var fileSystemInfo = Search(path, ksmDefault);

            if (fileSystemInfo == null)
            {
                return false;
            }
            else if (fileSystemInfo is FileInfo)
            {
                File.Delete(fileSystemInfo.FullName);
            }
            else
            {
                Directory.Delete(fileSystemInfo.FullName, true);
            }

            return true;
        }

        public override VolumeFile SaveFile(VolumePath path, FileContent content, bool verifyFreeSpace = true)
        {
            Directory.CreateDirectory(VolumeFolder);

            string archivePath = GetArchivePath(path);
            if (Directory.Exists(archivePath))
            {
                throw new KOSPersistenceException("Can't save file over a directory: " + path);
            }

            string parentPath = Directory.GetParent(archivePath).FullName;

            if (!Directory.Exists(parentPath))
            {
                Directory.CreateDirectory(parentPath);
            }

            byte[] fileBody = ConvertToWindowsNewlines(content.Bytes);

            using (var outfile = new BinaryWriter(File.Open(archivePath, FileMode.Create)))
            {
                outfile.Write(fileBody);
            }

            return Open(path) as VolumeFile;
        }

        public static byte[] ConvertToWindowsNewlines(byte[] bytes)
        {
            FileCategory category = PersistenceUtilities.IdentifyCategory(bytes);

            if (SafeHouse.IsWindows && !PersistenceUtilities.IsBinary(category))
            {
                string asString = FileContent.DecodeString(bytes);
                // Only evil windows gets evil windows line breaks, and only if this is some sort of ASCII:
                asString = asString.Replace("\n", "\r\n");
                return FileContent.EncodeString(asString);
            }

            return bytes;
        }

        public static byte[] ConvertFromWindowsNewlines(byte[] bytes)
        {
            FileCategory category = PersistenceUtilities.IdentifyCategory(bytes);

            if (!PersistenceUtilities.IsBinary(category))
            {
                string asString = FileContent.DecodeString(bytes);
                // Only evil windows gets evil windows line breaks, and only if this is some sort of ASCII:
                asString = asString.Replace("\r\n", "\n");
                return FileContent.EncodeString(asString);
            }

            return bytes;
        }

        public override float RequiredPower()
        {
            const int MULTIPLIER = 5;
            const float POWER_REQUIRED = BASE_POWER * MULTIPLIER;

            return POWER_REQUIRED;
        }

        /// <summary>
        /// Get the file from the OS.
        /// </summary>
        /// <param name="name">filename to look for</param>
        /// <param name="ksmDefault">if true, it prefers to use the KSM filename over the KS.  The default is to prefer KS.</param>
        /// <returns>the full fileinfo of the filename if found</returns>
        private FileSystemInfo Search(VolumePath volumePath, bool ksmDefault)
        {
            var path = GetArchivePath(volumePath);

            if (Directory.Exists(path))
            {
                return new DirectoryInfo(path);
            }

            if (File.Exists(path))
            {
                return new FileInfo(path);
            }

            var kerboscriptFile = new FileInfo(PersistenceUtilities.CookedFilename(path, KERBOSCRIPT_EXTENSION, true));
            var kosMlFile = new FileInfo(PersistenceUtilities.CookedFilename(path, KOS_MACHINELANGUAGE_EXTENSION, true));

            if (kerboscriptFile.Exists && kosMlFile.Exists)
            {
                return ksmDefault ? kosMlFile : kerboscriptFile;
            }
            if (kerboscriptFile.Exists)
            {
                return kerboscriptFile;
            }
            if (kosMlFile.Exists)
            {
                return kosMlFile;
            }
            return null;
        }
    }
}
