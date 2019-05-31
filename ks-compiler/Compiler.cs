using System.IO;
using kOS.Safe.Serialization;
using kOS.Safe.Compilation;
using kOS.Safe.Compilation.KS;
using kOS.Safe.Persistence;

namespace KSCompiler
{
    public class Compiler
    {
        private string baseDir;
        private VolumeManager volumeManager;
        private KSScript scriptHandler;

        private string FindKerboscriptTests()
        {
            var currentDir = Directory.GetCurrentDirectory();
            while (!Directory.Exists(Path.Combine(currentDir, "scripts/src")))
            {
                currentDir = Directory.GetParent(currentDir).FullName;
            }

            return Path.Combine(currentDir, "scripts/src");
        }

        public Compiler()
        {
            Opcode.InitMachineCodeData();
            CompiledObject.InitTypeData();
            SafeSerializationMgr.CheckIDumperStatics();

            baseDir = FindKerboscriptTests();
            scriptHandler = new KSScript();

            volumeManager = new VolumeManager();
            Archive archive = new Archive(baseDir);
            volumeManager.Add(archive);
            volumeManager.SwitchTo(archive);
        }

        public void Compile(string fileName)
        {
            string contents = File.ReadAllText(Path.Combine(baseDir, fileName));
            GlobalPath path = volumeManager.GlobalPathFromObject("0:/" + fileName);
            GlobalPath outPath = volumeManager.GlobalPathFromObject("0:/" + Path.ChangeExtension(fileName, ".ksm"));

            var options = new CompilerOptions() { LoadProgramsInSameAddressSpace = true };

            var codeParts = scriptHandler.Compile(path, 1, contents, "ks-compiler", options);
            VolumeFile written = volumeManager.CurrentVolume.SaveFile(outPath, new FileContent(codeParts));
        }
    }
}
