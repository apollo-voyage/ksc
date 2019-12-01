using System.IO;
using System.Linq;
using NUnit.Framework;
using kOS.Cli.Models; 
using kOS.Cli.Actions;

namespace kOS.Cli.Tests
{
    [TestFixture]
    public class InitializerTests
    {
        [Test]
        public void InitConfigWithoutOptions([Values("foobar", "foo-bar", "foo_bar")] string ProjectName)
        {
            // Build up.
            string arguments = $"init {ProjectName} -y";

            // Test.
            int result = ActionDispatcher.Dispatch(arguments.Split(' '), false);

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
            Assert.AreEqual(Constants.DefaultBootVolumePath, config.Volumes[0].InputPath);
            Assert.AreEqual(Constants.DefaultVolumePath, config.Volumes[1].InputPath);

            // Scripts asserts.
            Assert.IsNotNull(config.Scripts);
            Assert.AreEqual(2, config.Scripts.Count);
            Assert.AreEqual("compile", config.Scripts[0].Name);
            Assert.AreEqual("deploy", config.Scripts[1].Name);

            // Default files and directory asserts.
            string bootDirectory = Path.GetFullPath(Path.Combine("./", Constants.DefaultBootVolumePath));
            string bootFilepath = Path.Combine(bootDirectory, Constants.DefaultBootScriptFilename);
            Assert.IsTrue(Directory.Exists(bootDirectory));
            Assert.IsTrue(File.Exists(bootFilepath));
            Assert.AreEqual(Constants.DefaultBootScriptContent, File.ReadAllText(bootFilepath));

            string scriptDirectory = Path.GetFullPath(Path.Combine("./", Constants.DefaultScriptVolumePath));
            Assert.IsTrue(Directory.Exists(scriptDirectory));

            string compileScriptFilepath = Path.Combine(scriptDirectory, Constants.DefaultCompileScriptFilename);
            Assert.IsTrue(File.Exists(compileScriptFilepath));
            Assert.AreEqual(Constants.DefaultCompileScriptContent, File.ReadAllText(compileScriptFilepath));

            string deployScriptFilepath = Path.Combine(scriptDirectory, Constants.DefaultDeployScriptFilename);
            Assert.IsTrue(File.Exists(deployScriptFilepath));
            Assert.AreEqual(Constants.DefaultDeployScriptContent, File.ReadAllText(deployScriptFilepath));

            // Tear down.
            File.Delete(configFilePath);
            Directory.Delete(Path.GetFullPath(Constants.DefaultVolumePath), true);
            Directory.Delete(Path.GetFullPath(Constants.DefaultBootVolumePath), true);
            Directory.Delete(Path.GetFullPath(Constants.DefaultScriptVolumePath), true);
        }

        [Test]
        public void InitConfigWithProjectName([Values("foobar", "foo-bar", "foo_bar")] string ProjectName)
        {
            // Build up.
            string arguments = "init " + ProjectName + " -y";

            // Test.
            int result = ActionDispatcher.Dispatch(arguments.Split(' '), false);

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
            Assert.AreEqual(Constants.DefaultBootVolumePath, config.Volumes[0].InputPath);
            Assert.AreEqual(Constants.DefaultVolumePath, config.Volumes[1].InputPath);

            // Scripts asserts.
            Assert.IsNotNull(config.Scripts);
            Assert.AreEqual(2, config.Scripts.Count);
            Assert.AreEqual("compile", config.Scripts[0].Name);
            Assert.AreEqual("deploy", config.Scripts[1].Name);

            // Default files and directory asserts.

            // Tear down.
            File.Delete(configFilePath);
            Directory.Delete(Path.GetFullPath(Constants.DefaultVolumePath), true);
            Directory.Delete(Path.GetFullPath(Constants.DefaultBootVolumePath), true);
            Directory.Delete(Path.GetFullPath(Constants.DefaultScriptVolumePath), true);
        }

        [Test]
        public void InitConfigWithProjectNameAndPath(
            [Values("project-name")] string ProjectName,
            [Values("./foo/bar", "./foobar", "./foo/bar/test/dasfasdf")] string ProjectPath
        )
        {
            // Build up.
            string arguments = "init " + ProjectName + " " + ProjectPath + " -y";

            // Test.
            int result = ActionDispatcher.Dispatch(arguments.Split(' '), false);

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
            Assert.AreEqual(Constants.DefaultBootVolumePath, config.Volumes[0].InputPath);
            Assert.AreEqual(Constants.DefaultVolumePath, config.Volumes[1].InputPath);

            // Scripts asserts.
            Assert.IsNotNull(config.Scripts);
            Assert.AreEqual(2, config.Scripts.Count);
            Assert.AreEqual("compile", config.Scripts[0].Name);
            Assert.AreEqual("deploy", config.Scripts[1].Name);

            // Default files and directory asserts.

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
