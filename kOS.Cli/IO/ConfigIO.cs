using System;
using System.IO;
using kOS.Cli.Models;

namespace kOS.Cli.IO
{
    /// <summary>
    /// Utils class for configuration IO.
    /// </summary>
    public static class ConfigIO
    {
        /// <summary>
        /// Writes the configuration to a file to disk, in form of a JSON representation.
        /// </summary>
        /// <param name="Config">Configuration to write.</param>
        /// <param name="DirectoryPath">Directory path to create the file in.</param>
        /// <param name="WriteToConsole">Flag, wheter the method should write to the console.</param>
        /// <returns></returns>
        public static int WriteConfigFile(Configuration Config, string DirectoryPath = "", bool WriteToConsole = false)
        {
            // Test of the config is null.
            int result = Config != null ? 0 : 1;
            if (result == 1)
            {
                if (WriteToConsole == true)
                {
                    Console.WriteLine("Something went wrong; unable to create config file.");
                }

                return result;
            }

            // Check for the directory path to save the config in,
            // if it doesn't exist we create it.
            string filePath;
            DirectoryPath = DirectoryPath == null ? string.Empty : DirectoryPath;
            DirectoryPath = Path.GetFullPath(DirectoryPath);
            if (DirectoryPath == string.Empty)
            {
                filePath = Path.Combine(Directory.GetCurrentDirectory(), Constants.ConfigFileName);
            }
            else
            {
                DirectoryInfo dirInfo = Directory.Exists(DirectoryPath) ? 
                    new DirectoryInfo(DirectoryPath) : 
                    Directory.CreateDirectory(DirectoryPath);

                filePath = Path.Combine(dirInfo.FullName, Constants.ConfigFileName);
            }

            // Finally write the config file.
            File.WriteAllText(filePath, Config.ToJson());


            if (WriteToConsole == true)
            {
                Console.WriteLine("Config file created at: {0}", filePath);
            }

            return result;
        }

        /// <summary>
        /// Reads a configuration from a file from disk.
        /// </summary>
        /// <param name="Filepath">Filepath to the configuration file.</param>
        /// <returns>Read configuration.</returns>
        public static Configuration ReadConfigFile(string Filepath)
        {
            Configuration result = null;

            if (File.Exists(Filepath) == true)
            {
                string configJSON = File.ReadAllText(Filepath);
                result = Configuration.FromJson(configJSON);
            }

            return result;
        }
    }
}
