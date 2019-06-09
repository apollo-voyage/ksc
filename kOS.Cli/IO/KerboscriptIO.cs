using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using kOS.Cli.Models;

namespace kOS.Cli.IO
{
    public static class KerboscriptIO
    {
        public static List<Kerboscript> Load(string InputPath, string OutputPath)
        {
            List<Kerboscript> result = null;

            InputPath = Path.GetFullPath(InputPath);
            OutputPath = Path.GetFullPath(OutputPath);
            if (Directory.Exists(InputPath) == true)
            {
                result = LoadFromDirectory(InputPath, OutputPath);
            }
            else if (File.Exists(InputPath) == true) 
            {
                Kerboscript script = LoadFromFile(InputPath, OutputPath);
                result = script != null ? new List<Kerboscript>() : null;
                if (result != null)
                {
                    result.Add(script);
                }
            }

            return result;
        }

        private static List<Kerboscript> LoadFromDirectory(string InputPath, string OutputPath)
        {
            List<Kerboscript> result = new List<Kerboscript>();

            DirectoryInfo dirInfo = new DirectoryInfo(InputPath);
            FileInfo[] filesInfos = dirInfo.GetFiles(Constants.KerboscriptSearchPattern, SearchOption.AllDirectories);
            foreach (FileInfo fileInfo in filesInfos)
            {
                Kerboscript script = LoadFromFile(fileInfo.FullName, fileInfo.FullName.Replace(InputPath, OutputPath));
                result.Add(script);
            }

            return result;
        }

        private static Kerboscript LoadFromFile(string InputPath, string OutputPath)
        {
            string content = File.ReadAllText(InputPath);
            return new Kerboscript
            {
                InputPath = InputPath,
                OutputPath = OutputPath,
                Content = content
            };
        }
    }
}
