using System;
using System.IO;
using System.Drawing;
using Pastel;
using kOS.Cli.IO;
using kOS.Cli.Models;
using kOS.Cli.Options;

namespace kOS.Cli.Actions
{
    /// <summary>
    /// Init action. Runs when the user uses "ksc init".
    /// </summary>
    class InitAction : AbstractAction
    {
        /// <summary>
        /// Options for the initializer.
        /// </summary>
        private InitOptions _options;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="options">Options for the initializer.</param>
        public InitAction(InitOptions options)
        {
            _options = options;
        }

        /// <summary>
        /// Runs the initializer.
        /// </summary>
        /// <returns>Returns the CLI return code.</returns>
        public override int Run()
        {
            int result = -1;

            // Create the configuration based on given options.
            Configuration config = _options.Yes == true ? CreateDefaultConfig() : AskConfig();

            // Write the configuration to disk based on given options.
            if (_options.ProjectPath != string.Empty && _options.ProjectPath != null)
            {
                string path = Path.Combine(_options.ProjectPath, _options.ProjectName);
                result = ConfigIO.WriteConfigFile(config, Path.Combine(_options.ProjectPath, _options.ProjectName), true);
                CreateDefaultDirectoriesAndFiles(config, path);
            }
            else
            {
                result = ConfigIO.WriteConfigFile(config, ".", true);
                CreateDefaultDirectoriesAndFiles(config, ".");
            }

            return result;
        }

        #region Private

        #region Config Creation

        /// <summary>
        /// Asks the user interactively for the config information. 
        /// </summary>
        /// <returns>Resulting configuration.</returns>
        private Configuration AskConfig()
        {
            PrintWelcomeText();

            Configuration result = new Configuration();

            // Ask the mandatory information.
            if (_options.ProjectName == string.Empty)
            {
                result.Name = Ask("Project name", GetProjectNameDefault());
            }

            result.Description = Ask("Project description");
            result.Archive = Ask("Project archive (used KSP installation)", FindKSPInstallation());
            if (result.Archive == Constants.DefaultKSPSteamInstallationWin32 ||
                result.Archive == Constants.DefaultKSPSteamInstallationWin64)
            {
                result.Archive = result.Archive + Constants.ArchivePath;
            }

            // Ask for volumes.
            Volume volume = new Volume();
            volume.Index = 2;
            volume.Name = Ask("Project volume name (volume with your code)", GetProjectNameDefault());
            volume.InputPath = Ask("Project volume source directory", Constants.DefaultVolumePath);
            volume.OutputPath = Ask("Project volume dist directory", Constants.DistDirectory);
            volume.DeployPath = Constants.CurrentDirectory;
            Console.WriteLine("You can add more volumes in the created config file!");

            result.Volumes.Add(new Volume
            {
                Index = 1,
                Name = "boot",
                InputPath = Constants.DefaultBootVolumePath,
                OutputPath = Constants.DistBootDirectory,
                DeployPath = Constants.DefaultBootVolumePath
            });
            result.Volumes.Add(volume);

            // Create two default scripts.
            AddDefaultScripts(result);

            return result;
        }

        /// <summary>
        /// Creates a default configuration.
        /// </summary>
        /// <returns>Resulting configuration.</returns>
        private Configuration CreateDefaultConfig()
        {
            Configuration result = new Configuration();

            // Config info.
            result.Name = GetProjectNameDefault();
            result.Description = "A Kerboscript project.";
            result.Archive = FindKSPInstallation();

            // Volume info.
            result.Volumes.Add(new Volume
            {
                Index = 1,
                Name = "boot",
                InputPath = Constants.DefaultBootVolumePath,
                OutputPath = Constants.DistBootDirectory,
                DeployPath = "/boot"
            });

            result.Volumes.Add(new Volume
            {
                Index = 2,
                Name = GetProjectNameDefault(),
                InputPath = Constants.DefaultVolumePath,
                OutputPath = Constants.DistDirectory,
                DeployPath = "/"
            });

            // Scripts.
            AddDefaultScripts(result);

            return result;
        }

        /// <summary>
        /// Adds default scripts to a configuration.
        /// </summary>
        /// <param name="config">Configuration to add it to.</param>
        private void AddDefaultScripts(Configuration config)
        {
            config.Scripts.Add(new Script { Name = "compile", Content = "ksc run " + Constants.DefaultScriptVolumePath + "/" + Constants.DefaultCompileScriptFilename });
            config.Scripts.Add(new Script { Name = "deploy", Content = "ksc run compile && ksc run " + Constants.DefaultScriptVolumePath + "/" + Constants.DefaultDeployScriptFilename });
        }

        /// <summary>
        /// Creates the default directories "./src".
        /// </summary>
        /// <param name="basePath">Base path where to create the default directories.</param>
        private void CreateDefaultDirectoriesAndFiles(Configuration config, string basePath)
        {
            foreach (Volume volume in config.Volumes)
            {
                string pathToCreate = Path.Combine(basePath, volume.InputPath);
                Directory.CreateDirectory(Path.GetFullPath(pathToCreate));

                if (volume.InputPath == Constants.DefaultBootVolumePath)
                {
                    File.WriteAllText(Path.Combine(Path.GetFullPath(pathToCreate), Constants.DefaultBootScriptFilename), Constants.DefaultBootScriptContent);
                }
            }

            string defaultScriptPath = Path.Combine(basePath, Constants.DefaultScriptVolumePath);
            Directory.CreateDirectory(Path.GetFullPath(defaultScriptPath));
            File.WriteAllText(Path.Combine(defaultScriptPath, Constants.DefaultCompileScriptFilename), Constants.DefaultCompileScriptContent);
            File.WriteAllText(Path.Combine(defaultScriptPath, Constants.DefaultDeployScriptFilename), Constants.DefaultDeployScriptContent);
        }

        #endregion // Config Creation

        #region User Interaction

        /// <summary>
        /// Prints the welcome text.
        /// </summary>
        private void PrintWelcomeText()
        {
            Console.WriteLine("This utility will walk you through the initialization process for a ksconfig.json");
            Console.WriteLine("");
            Console.WriteLine("Press ^C at any time to quit.");
        }

        /// <summary>
        /// Ask the user a given question.
        /// </summary>
        /// <param name="question">Question to ask.</param>
        /// <param name="default">Default for the question.</param>
        /// <returns>Answer from the user.</returns>
        private string Ask(string question, string @default = "")
        {
            string output;

            // Create the output format.
            if (@default != string.Empty)
            {
                output = string.Format(question + ": [{0}] ", @default.Pastel(Color.DarkGray));    
            }
            else
            {
                output = question + ": ";
            }

            // Ask the question.
            Console.Write(output);
            string result = Console.ReadLine();

            // Check if the question has been answered, if not set the default as the result.
            if (result == string.Empty)
            {
                result = @default;
            }

            return result;
        }

        #endregion // User Interaction

        #region Utils

        /// <summary>
        /// Looks for a KSP installation in commonly used installation paths, it if finds the directory exists it will be returned.
        /// </summary>
        /// <returns>Found KSP installation.</returns>
        private string FindKSPInstallation()
        { 
            string result = Directory.Exists(Constants.DefaultKSPSteamInstallationWin32) ? Constants.DefaultKSPSteamInstallationWin32 : string.Empty;
            result = Directory.Exists(Constants.DefaultKSPSteamInstallationWin64) ? Constants.DefaultKSPSteamInstallationWin64 : result;

            return result;
        }

        /// <summary>
        /// Returns the project name default value.
        /// </summary>
        /// <returns>Project name default value.</returns>
        private string GetProjectNameDefault()
        {
            string result = _options.ProjectName != string.Empty ? _options.ProjectName : string.Empty;
            result = result == string.Empty ? new DirectoryInfo(Directory.GetCurrentDirectory()).Name : result;

            return result;
        }

        #endregion // Utils

        #endregion // Private

    }
}
