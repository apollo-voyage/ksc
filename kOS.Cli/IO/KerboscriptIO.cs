using System.IO;
using System.Collections.Generic;
using kOS.Cli.Models;

namespace kOS.Cli.IO
{
    /// <summary>
    /// Util class for basic *.ks IO.
    /// </summary>
    public static class KerboscriptIO
    {
        /// <summary>
        /// Loads Kerboscripts from disc.
        /// </summary>
        /// <param name="input">Input path for a file or directory.</param>
        /// <param name="output">Output path for the compiled file or directory.</param>
        /// <returns>List of loaded Kerboscripts.</returns>
        public static List<Kerboscript> Load(string input, string output)
        {
            List<Kerboscript> result = null;

            input = Path.GetFullPath(input);
            output = Path.GetFullPath(output);
            if (Directory.Exists(input) == true)
            {
                result = LoadFromDirectory(input, output);
            }
            else if (File.Exists(input) == true) 
            {
                Kerboscript script = LoadFromFile(input, output);
                result = script != null ? new List<Kerboscript>() : null;
                if (result != null)
                {
                    result.Add(script);
                }
            }

            return result;
        }

        /// <summary>
        /// Loads all Kerboscript from a directory.
        /// </summary>
        /// <param name="input">Directory to load from.</param>
        /// <param name="output">Directory to compile to.</param>
        /// <returns>List of loaded Kerboscripts.</returns>
        private static List<Kerboscript> LoadFromDirectory(string input, string output)
        {
            List<Kerboscript> result = new List<Kerboscript>();

            DirectoryInfo dirInfo = new DirectoryInfo(input);
            FileInfo[] filesInfos = dirInfo.GetFiles(Constants.KerboscriptSearchPattern, SearchOption.AllDirectories);
            foreach (FileInfo fileInfo in filesInfos)
            {
                string outPath = fileInfo.FullName.Replace(input, output);
                Kerboscript script = LoadFromFile(fileInfo.FullName, outPath);
                result.Add(script);
            }

            return result;
        }

        /// <summary>
        /// Loads a Kerboscript from a file.
        /// </summary>
        /// <param name="input">Filepath of the Kerboscript to load.</param>
        /// <param name="output">Filepath of the compiled Kerboscript.</param>
        /// <returns>Loaded Kerboscript.</returns>
        private static Kerboscript LoadFromFile(string input, string output)
        {
            string content = File.ReadAllText(input);
            return new Kerboscript
            {
                InputPath = input,
                OutputPath = Path.ChangeExtension(output, Constants.KerboscriptCompiledExtension),
                Content = content
            };
        }
    }
}
