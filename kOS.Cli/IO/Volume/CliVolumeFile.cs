using System.IO;
using kOS.Safe.Utilities;
using kOS.Safe.Persistence;

namespace kOS.Cli.IO
{
    /// <summary>
    /// CLI volume file to be used for a custom Kerboscript compilation and deployment.
    /// </summary>
    [KOSNomenclature("CliVolumeFile")]
    public class CliVolumeFile : VolumeFile
    {
        private readonly FileInfo fileInfo;

        public override int Size { get { fileInfo.Refresh(); return (int)fileInfo.Length; } }

        public CliVolumeFile(CliVolume volume, FileInfo fileInfo, VolumePath path)
            : base(volume, path)
        {
            this.fileInfo = fileInfo;
        }

        public override FileContent ReadAll()
        {
            byte[] bytes = File.ReadAllBytes(fileInfo.FullName);

            bytes = CliVolume.ConvertFromWindowsNewlines(bytes);

            return new FileContent(bytes);
        }

        public override bool Write(byte[] content)
        {
            if (!fileInfo.Exists)
            {
                throw new KOSFileException("File does not exist: " + fileInfo.Name);
            }

            byte[] bytes = CliVolume.ConvertToWindowsNewlines(content);
            using (FileStream stream = fileInfo.Open(FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                stream.Write(bytes, 0, bytes.Length);
                stream.Flush();
            }

            return true;
        }

        public override void Clear()
        {
            File.WriteAllText(fileInfo.FullName, string.Empty);
        }
    }
}
