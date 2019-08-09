using System.IO;
using System.Linq;
using NUnit.Framework;
using kOS.Cli.Models;
using kOS.Cli.Options;
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
            CompileOptions options = new CompileOptions
            {
                Input = scriptPath,
                Output = "."
            };

            //Compiler compiler = new Compiler(options);

            // Test.
            //int result = compiler.Run();
            Assert.IsTrue(File.Exists(compiledScriptPath));

            // Tear down.
            File.Delete(compiledScriptPath);
        }

        [Test]
        public void CompileProject()
        {
            // Build up.
            CompileOptions options = new CompileOptions
            {
                Input = "./scripts",
                Output = "./scripts",
                Volume = "all"
            };

            //Compiler compiler = new Compiler(options);

            // Test.
            //int result = compiler.Run();
            string fullDistPath = Path.GetFullPath("./scripts/dist");
            Assert.IsTrue(Directory.Exists(fullDistPath));

            // Tear down.
            Directory.Delete(fullDistPath, true);
        }

        [Test]
        public void CompileProjectVolumeByName()
        {
            // Build up.
            CompileOptions options = new CompileOptions
            {
                Input = "./scripts",
                Output = "./scripts",
                Volume = "boot"
            };

            //Compiler compiler = new Compiler(options);

            // Test.
            //int result = compiler.Run();
            string fullDistBootPath = Path.GetFullPath("./scripts/dist/boot");
            Assert.IsTrue(Directory.Exists(fullDistBootPath));

            // Tear down.
            string fullDistPath = Path.GetFullPath("./scripts/dist");
            Directory.Delete(fullDistPath, true);
        }

        [Test]
        public void CompileProjectVolumeByIndex()
        {
            // Build up.
            CompileOptions options = new CompileOptions
            {
                Input = "./scripts",
                Output = "./scripts",
                Volume = "1"
            };

            //Compiler compiler = new Compiler(options);

            // Test.
            //int result = compiler.Run();
            string fullDistBootPath = Path.GetFullPath("./scripts/dist/boot");
            Assert.IsTrue(Directory.Exists(fullDistBootPath));

            // Tear down.
            string fullDistPath = Path.GetFullPath("./scripts/dist");
            Directory.Delete(fullDistPath, true);
        }
    }
}
