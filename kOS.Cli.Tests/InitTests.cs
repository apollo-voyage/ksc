using System.IO;
using NUnit.Framework;
using kOS.Cli.Options;
using kOS.Cli.Models;

namespace kOS.Cli.Tests
{
    [TestFixture]
    public class InitTests
    {
        [Test]
        public void InitConfigWithoutOptions()
        {
            // Build up.
            InitOptions options = new InitOptions { ProjectName = "", ProjectPath = "", Yes = true };
            Initializer initializer = new Initializer(options);

            // Test.
            int result = initializer.Run();

            // General asserts.
            Assert.AreEqual(0, result);
            string configFilePath = Path.GetFullPath(Path.Combine("./", Constants.ConfigFileName));
            Assert.IsTrue(File.Exists(configFilePath));

            // Content asserts.
            string configContent = File.ReadAllText(configFilePath);
            Assert.IsNotNull(configContent);

            // Data model asserts.
            Configuration config = Configuration.FromJson(configContent);
            Assert.IsNotNull(config);

            string expectedName = new DirectoryInfo(Path.GetFullPath("./")).Name;
            Assert.IsNotNull(config.Name);
            Assert.AreEqual(expectedName, config.Name);

            // Volumes asserts.
            Assert.IsNotNull(config.Volumes);
            Assert.AreEqual(1, config.Volumes.Count);
            Assert.AreEqual(0, config.Volumes[0].Index);
            Assert.AreEqual(expectedName, config.Volumes[0].Name);
            Assert.AreEqual("./src", config.Volumes[0].Path);

            // Scripts asserts.
            Assert.IsNotNull(config.Scripts);
            Assert.AreEqual(2, config.Scripts.Count);
            Assert.AreEqual("compile", config.Scripts[0].Name);
            Assert.AreEqual("deploy", config.Scripts[1].Name);

            // Tear down.
            File.Delete(configFilePath);
        }

        [Test]
        public void InitConfigWithProjectName([Values("test", "prj-test", "no_op")] string ProjectName)
        {
            // Build up.
            InitOptions options = new InitOptions { ProjectName = ProjectName, ProjectPath = "", Yes = true };
            Initializer initializer = new Initializer(options);

            // Test.
            int result = initializer.Run();

            // General asserts.
            Assert.AreEqual(0, result);
            string configFilePath = Path.GetFullPath(Path.Combine("./", Constants.ConfigFileName));
            Assert.IsTrue(File.Exists(configFilePath));

            // Content asserts.
            string configContent = File.ReadAllText(configFilePath);
            Assert.IsNotNull(configContent);

            // Data model asserts.
            Configuration config = Configuration.FromJson(configContent);
            Assert.IsNotNull(config);

            Assert.IsNotNull(config.Name);
            Assert.AreEqual(ProjectName, config.Name);

            // Volumes asserts.
            Assert.IsNotNull(config.Volumes);
            Assert.AreEqual(1, config.Volumes.Count);
            Assert.AreEqual(0, config.Volumes[0].Index);
            Assert.AreEqual(ProjectName, config.Volumes[0].Name);
            Assert.AreEqual("./src", config.Volumes[0].Path);

            // Scripts asserts.
            Assert.IsNotNull(config.Scripts);
            Assert.AreEqual(2, config.Scripts.Count);
            Assert.AreEqual("compile", config.Scripts[0].Name);
            Assert.AreEqual("deploy", config.Scripts[1].Name);

            // Tear down.
            File.Delete(configFilePath);
        }

        [Test]
        public void InitConfigWithProjectNameAndPath(
            [Values("project-name")] string ProjectName,
            [Values("./test/noop", "./test")] string ProjectPath
        )
        {
            // Build up.
            InitOptions options = new InitOptions { ProjectName = ProjectName, ProjectPath = ProjectPath, Yes = true };
            Initializer initializer = new Initializer(options);

            // Test.
            int result = initializer.Run();

            // General asserts.
            Assert.AreEqual(0, result);
            string configFilePath = Path.GetFullPath(Path.Combine(Path.Combine(Path.GetFullPath(ProjectPath), ProjectName), Constants.ConfigFileName));
            Assert.IsTrue(File.Exists(configFilePath));

            // Content asserts.
            string configContent = File.ReadAllText(configFilePath);
            Assert.IsNotNull(configContent);

            // Data model asserts.
            Configuration config = Configuration.FromJson(configContent);
            Assert.IsNotNull(config);

            Assert.IsNotNull(config.Name);
            Assert.AreEqual(ProjectName, config.Name);

            // Volumes asserts.
            Assert.IsNotNull(config.Volumes);
            Assert.AreEqual(1, config.Volumes.Count);
            Assert.AreEqual(0, config.Volumes[0].Index);
            Assert.AreEqual(ProjectName, config.Volumes[0].Name);
            Assert.AreEqual("./src", config.Volumes[0].Path);

            // Scripts asserts.
            Assert.IsNotNull(config.Scripts);
            Assert.AreEqual(2, config.Scripts.Count);
            Assert.AreEqual("compile", config.Scripts[0].Name);
            Assert.AreEqual("deploy", config.Scripts[1].Name);

            // Tear down.
            File.Delete(configFilePath);
            Directory.Delete(Path.GetFullPath(ProjectPath), true);
        }
    }
}
