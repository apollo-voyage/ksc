using System.IO;
using System.Linq;
using NUnit.Framework;
using kOS.Cli.Models; 
using kOS.Cli.Options;
using kOS.Cli.Actions;

namespace kOS.Cli.Tests
{
    [TestFixture]
    public class InitializerTests
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
            Assert.AreEqual(2, config.Volumes.Count);
            Assert.AreEqual(1, config.Volumes[0].Index);
            Assert.AreEqual(2, config.Volumes[1].Index);
            Assert.AreEqual("boot", config.Volumes[0].Name);
            Assert.AreEqual(expectedName, config.Volumes[1].Name);
            Assert.AreEqual(Constants.DefaultVolumePath + Constants.DefaultBootVolumePath, config.Volumes[0].InputPath);
            Assert.AreEqual(Constants.DefaultVolumePath, config.Volumes[1].InputPath);

            // Scripts asserts.
            Assert.IsNotNull(config.Scripts);
            Assert.AreEqual(2, config.Scripts.Count);
            Assert.AreEqual("compile", config.Scripts[0].Name);
            Assert.AreEqual("deploy", config.Scripts[1].Name);

            // Tear down.
            File.Delete(configFilePath);
            Directory.Delete(Path.GetFullPath(Constants.DefaultVolumePath), true);        }

        [Test]
        public void InitConfigWithProjectName([Values("foobar", "foo-bar", "foo_bar")] string ProjectName)
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
            Assert.AreEqual(2, config.Volumes.Count);
            Assert.AreEqual(1, config.Volumes[0].Index);
            Assert.AreEqual(2, config.Volumes[1].Index);
            Assert.AreEqual("boot", config.Volumes[0].Name);
            Assert.AreEqual(ProjectName, config.Volumes[1].Name);
            Assert.AreEqual(Constants.DefaultVolumePath + Constants.DefaultBootVolumePath, config.Volumes[0].InputPath);
            Assert.AreEqual(Constants.DefaultVolumePath, config.Volumes[1].InputPath);

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
            [Values("./foo/bar", "./foobar", "./foo/bar/test/dasfasdf")] string ProjectPath
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
            Assert.AreEqual(2, config.Volumes.Count);
            Assert.AreEqual(1, config.Volumes[0].Index);
            Assert.AreEqual(2, config.Volumes[1].Index);
            Assert.AreEqual("boot", config.Volumes[0].Name);
            Assert.AreEqual(ProjectName, config.Volumes[1].Name);
            Assert.AreEqual(Constants.DefaultVolumePath + Constants.DefaultBootVolumePath, config.Volumes[0].InputPath);
            Assert.AreEqual(Constants.DefaultVolumePath, config.Volumes[1].InputPath);

            // Scripts asserts.
            Assert.IsNotNull(config.Scripts);
            Assert.AreEqual(2, config.Scripts.Count);
            Assert.AreEqual("compile", config.Scripts[0].Name);
            Assert.AreEqual("deploy", config.Scripts[1].Name);

            // Tear down.
            File.Delete(configFilePath);

            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetFullPath(ProjectPath));
            int count = ProjectPath.Count(s => s == '/');
            if (count > 1)
            {
                for(int i = count - 1; i > 0; i--)
                {
                    dirInfo = Directory.GetParent(dirInfo.FullName);
                }
            }

            Directory.Delete(dirInfo.FullName, true);
        }
    }
}
