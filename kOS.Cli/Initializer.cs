using System;
using System.IO;
using kOS.Cli.Models;
using kOS.Cli.Options;

namespace kOS.Cli
{
    public class Initializer
    {
        /// <summary>
        /// Options for the initializer.
        /// </summary>
        private InitOptions _options;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="options">Options for the initializer.</param>
        public Initializer(InitOptions options)
        {
            _options = options;
        }

        /// <summary>
        /// Runs the initializer.
        /// </summary>
        /// <returns>Returns the CLI return code.</returns>
        public int Run()
        {
            Configuration config;
            if (_options.Yes == false)
            {
                config = AskConfig();
            }
            else
            {
                config = CreateDefaultConfig();
            }

            return WriteConfigFile(config, _options.ProjectPath);
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

            // Ask for volumes.
            Volume volume = new Volume();
            volume.Index = 0;
            volume.Name = Ask("Project volume name (volume with your code)", GetProjectNameDefault());
            volume.Path = Ask("Project volume source directory", "./src");
            Console.WriteLine("You can add more volumes later in the created config file!");
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
                Index = 0,
                Name = GetProjectNameDefault(),
                Path = "./src"
            });

            // Scripts.
            AddDefaultScripts(result);

            return result;
        }

        /// <summary>
        /// Adds default scripts to a configuration.
        /// </summary>
        /// <param name="Config">Configuration to add it to.</param>
        private void AddDefaultScripts(Configuration Config)
        {
            Config.Scripts.Add(new Script { Name = "compile", Content = "ksc run ./src/compile.ks" });
            Config.Scripts.Add(new Script { Name = "deploy", Content = "ksc run compile && ksc run ./src/deploy.ks" });
        }

        #endregion // Config Creation

        #region User Interaction

        /// <summary>
        /// Prints the welcome text.
        /// </summary>
        private void PrintWelcomeText()
        {
            Console.WriteLine("This utility will walk you through creating a ksconfig.json");
            Console.WriteLine("");
            Console.WriteLine("Press ^C at any time to quit.");
        }

        /// <summary>
        /// Ask the user a given question.
        /// </summary>
        /// <param name="Question">Question to ask.</param>
        /// <param name="Default">Default for the question.</param>
        /// <returns>Answer from the user.</returns>
        private string Ask(string Question, string Default = "")
        {
            string output;

            // Create the output format.
            if (Default != string.Empty)
            {
                output = string.Format(Question + ": [{0}] ", Default);    
            }
            else
            {
                output = Question + ": ";
            }

            // Ask the question.
            Console.Write(output);
            string result = Console.ReadLine();

            // Check if the question has been answered, if not set the default as the result.
            if (result == string.Empty)
            {
                result = Default;
            }

            return result;
        }

        #endregion // User Interaction

        #region File IO

        /// <summary>
        /// Writes the configuration to a JSON file.
        /// </summary>
        /// <param name="config"></param>
        private int WriteConfigFile(Configuration Config, string DirectoryPath = "")
        {
            int result = Config != null ? 0 : 1;
            if (result == 1)
            {
                Console.WriteLine("Something went wrong; unable to create config file.");
                return result;
            }

            string filePath;
            DirectoryPath = DirectoryPath == null ? string.Empty : DirectoryPath;
            if (DirectoryPath == string.Empty)
            {
                filePath = Path.Combine(Directory.GetCurrentDirectory(), Constants.ConfigFileName);
            }
            else
            {
                DirectoryPath = Path.GetFullPath(DirectoryPath);
                DirectoryInfo dirInfo = Directory.CreateDirectory(Path.Combine(DirectoryPath, _options.ProjectName));
                filePath = Path.Combine(dirInfo.FullName, Constants.ConfigFileName);
            }
            
            File.WriteAllText(filePath, Config.ToJson());
            Console.WriteLine("Config file created at: {0}", filePath);

            return result;
        }

        #endregion // File IO

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
