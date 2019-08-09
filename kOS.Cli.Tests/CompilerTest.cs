using System.IO;
using NUnit.Framework;
using kOS.Cli.Actions;

namespace kOS.Cli.Tests
{
    [TestFixture]
    public class CompilerTests
    {
        [Test]
        public void CompileSingleFile()
        {
            // Build up.
            string scriptPath = Path.GetFullPath("./scripts/src/test.ks");
            string compiledScriptPath = Path.ChangeExtension(scriptPath, "ksm");
            string arguments = "compile -i" + scriptPath;


            // Test.
            int result = ActionDispatcher.Dispatch(arguments.Split(' '), false);
            Assert.AreEqual(0, result);
            Assert.IsTrue(File.Exists(compiledScriptPath));

            // Tear down.
            File.Delete(compiledScriptPath);
        }

        [Test]
        public void CompileProject()
        {
            string arguments = "compile -i ./scripts";

            // Test.
            int result = ActionDispatcher.Dispatch(arguments.Split(' '), false);
            Assert.AreEqual(0, result);

            string fullDistPath = Path.GetFullPath("./scripts/dist");
            Assert.IsTrue(Directory.Exists(fullDistPath));

            // Tear down.
            Directory.Delete(fullDistPath, true);
        }

        [Test]
        public void CompileProjectVolumeByName()
        {
            string arguments = "compile -i ./scripts -v boot";

            // Test.
            int result = ActionDispatcher.Dispatch(arguments.Split(' '), false);
            Assert.AreEqual(0, result);

            string fullDistBootPath = Path.GetFullPath("./scripts/dist/boot");
            Assert.IsTrue(Directory.Exists(fullDistBootPath));

            // Tear down.
            string fullDistPath = Path.GetFullPath("./scripts/dist");
            Directory.Delete(fullDistPath, true);
        }

        [Test]
        public void CompileProjectVolumeByIndex()
        {
            string arguments = "compile -i ./scripts -v 1";

            // Test.
            int result = ActionDispatcher.Dispatch(arguments.Split(' '), false);
            Assert.AreEqual(0, result);

            string fullDistBootPath = Path.GetFullPath("./scripts/dist/boot");
            Assert.IsTrue(Directory.Exists(fullDistBootPath));

            // Tear down.
            string fullDistPath = Path.GetFullPath("./scripts/dist");
            Directory.Delete(fullDistPath, true);
        }
    }
}
