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
        /// <param name="config">Configuration to write.</param>
        /// <param name="directoryPath">Directory path to create the file in.</param>
        /// <param name="writeToConsole">Flag, wheter the method should write to the console.</param>
        /// <returns></returns>
        public static int WriteConfigFile(Configuration config, string directoryPath = "", bool writeToConsole = false)
        {
            // Test of the config is null.
            int result = config != null ? 0 : 1;
            if (result == 1)
            {
                if (writeToConsole == true)
                {
                    Console.WriteLine("Something went wrong; unable to create config file.");
                }

                return result;
            }

            // Check for the directory path to save the config in,
            // if it doesn't exist we create it.
            string filePath;
            directoryPath = directoryPath == null ? string.Empty : directoryPath;
            directoryPath = Path.GetFullPath(directoryPath);
            if (directoryPath == string.Empty)
            {
                filePath = Path.Combine(Directory.GetCurrentDirectory(), Constants.ConfigFileName);
            }
            else
            {
                DirectoryInfo dirInfo = Directory.Exists(directoryPath) ? 
                    new DirectoryInfo(directoryPath) : 
                    Directory.CreateDirectory(directoryPath);

                filePath = Path.Combine(dirInfo.FullName, Constants.ConfigFileName);
            }

            // Finally write the config file.
            if (config != null) 
            {
                File.WriteAllText(filePath, config.ToJson());
            }

            if (writeToConsole == true)
            {
                Console.WriteLine("Config file created at: {0}", filePath);
            }

            return result;
        }

        /// <summary>
        /// Reads a configuration from a file from disk.
        /// </summary>
        /// <param name="filepath">Filepath to the configuration file.</param>
        /// <returns>Read configuration.</returns>
        public static SanitizedConfiguration ReadConfigFile(string filepath)
        {
            SanitizedConfiguration result = null;

            if (File.Exists(filepath) == true)
            {
                string configJSON = File.ReadAllText(filepath);
                result = Configuration.FromJson(configJSON).GetSanitized();
            }

            return result;
        }

        /// <summary>
        /// Reads the config file from the current directory.
        /// </summary>
        /// <returns>Read configuration.</returns>
        public static SanitizedConfiguration ReadConfigFileFromCurrentDirectory()
        {
            SanitizedConfiguration result = null;

            string configFilepath = Path.Combine(Directory.GetCurrentDirectory(), Constants.ConfigFileName);
            if (File.Exists(configFilepath) == true)
            {
                result = ConfigIO.ReadConfigFile(configFilepath);
            }

            return result;
        }

        /// <summary>
        /// Checks if a given path is a directory path.
        /// </summary>
        /// <param name="path">Path to check.</param>
        /// <returns>True if it is a directory, false if its not.</returns>
        public static bool IsDirectory(string path)
        {
            string fullPath = Path.GetFullPath(path);
            return Directory.Exists(fullPath);
        }
    }
}
