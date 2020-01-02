namespace kOS.Cli.Models
{
    using System.Collections.Generic;
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// Sanitized configuration.
    /// </summary>
    public class SanitizedConfiguration
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="configuration">Parsed configuration.</param>
        public SanitizedConfiguration(Configuration configuration = null)
        {
            if (configuration != null)
            {
                Name = configuration.Name;
                Description = configuration.Description;
                Archive = configuration.Archive;
                Volumes = configuration.GetVolumes();
                Scripts = configuration.GetScripts();
            }
            else
            {
                Volumes = new List<Volume>();
                Scripts = new List<Script>();
            }
        }

        /// <summary>
        /// Name of the configuration.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of the configuration.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Archive: Local KSP installation.
        /// </summary>
        public string Archive { get; set; }

        /// <summary>
        /// Configuration volumes.
        /// </summary>
        public List<Volume> Volumes { get; set; }

        /// <summary>
        /// Configuration scripts.
        /// </summary>
        public List<Script> Scripts { get; set; }

        /// <summary>
        /// Returns volumes based on a given name or index.
        /// </summary>
        /// <param name="nameOrIndex">Name or index of the volume to return.</param>
        /// <returns>Either all volumes or a volume based on the given option</returns>
        public List<Volume> GetVolumesForOption(string nameOrIndex = "all")
        {
            List<Volume> result = new List<Volume>();

            if (nameOrIndex == Constants.AllVolumes)
            {
                result = Volumes;
            }
            else
            {
                Volume volume = null;
                if (int.TryParse(nameOrIndex, out int index) == true)
                {
                    volume = Volumes.Find(v => v.Index == index);
                    if (volume != null)
                    {
                        result.Add(volume);
                    }
                }
                else
                {
                    volume = Volumes.Find(v => v.Name == nameOrIndex);
                    if (volume != null)
                    {
                        result.Add(volume);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Converts the sanitized configuration back to the original configuration.
        /// </summary>
        /// <returns>Return converted configuration</returns>
        public Configuration ToConfiguration()
        {
            Configuration result = new Configuration
            {
                Name = Name,
                Description = Description,
                Archive = Archive
            };

            foreach(Volume volume in Volumes)
            {
                result.Volumes.Add(volume.Name, volume);
            }

            foreach (Script script in Scripts)
            {
                result.Scripts.Add(script.Name, script.Content);
            }

            return result;
        }
    }

    /// <summary>
    /// ksc configuration.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public partial class Configuration
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Configuration()
        {
            Volumes = new Dictionary<string, Volume>();
            Scripts = new Dictionary<string, string>();
        }

        /// <summary>
        /// Name of the configuration.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Description of the configuration.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Archive: Local KSP installation.
        /// </summary>
        [JsonProperty("archive")]
        public string Archive { get; set; }

        /// <summary>
        /// Configuration volumes.
        /// </summary>
        [JsonProperty("volumes")]
        public Dictionary<string, Volume> Volumes { get; set; }

        /// <summary>
        /// Configuration scripts.
        /// </summary>
        [JsonProperty("scripts")]
        public Dictionary<string, string> Scripts { get; set; }

        /// <summary>
        /// Returns all scripts.
        /// </summary>
        /// <returns>All scripts</returns>
        public List<Script> GetScripts() 
        {
            List<Script> result = new List<Script>();

            foreach(KeyValuePair<string, string> entry in Scripts)
            {
                Script script = new Script(entry.Key, entry.Value);
                result.Add(script);
            }

            return result;
        }

        /// <summary>
        /// Returns all volumes.
        /// </summary>
        /// <returns>All volumes</returns>
        public List<Volume> GetVolumes() 
        {
            List<Volume> result = new List<Volume>();

            int i = 1;
            foreach(KeyValuePair<string, Volume> entry in Volumes)
            {
                entry.Value.Name = entry.Key;
                entry.Value.Index = i; 
                
                result.Add(entry.Value);
                
                i++;         
            }

            return result;
        }

        /// <summary>
        /// Returns the sanitized configuration.
        /// </summary>
        /// <returns>Sanitized configuration</returns>
        public SanitizedConfiguration GetSanitized()
        {
            return new SanitizedConfiguration(this);
        }
    }

    /// <summary>
    /// Configuration script.
    /// </summary>
    public partial class Script
    {
        /// <summary>
        /// Part of a script.
        /// </summary>
        public class ScriptPart
        {
            /// <summary>
            /// Program to execute.
            /// </summary>
            public string Program { get; private set; }

            /// <summary>
            /// Arguments of the program to execute.
            /// </summary>
            public string Arguments { get; private set; }

            /// <summary>
            /// Default constructor.
            /// </summary>
            /// <param name="program">Program to execute</param>
            /// <param name="arguments">Arguments for the give program</param>
            public ScriptPart(string program, string arguments)
            {
                Program = program;
                Arguments = arguments;
            }
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="name">Name of the script</param>
        /// <param name="content">Content of the script</param>
        public Script(string name, string content) 
        {
            Name = name;
            Content = content;
        }

        /// <summary>
        /// Name of the script.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Content of the script.
        /// </summary>
        public string Content { get; set; }
        
        /// <summary>
        /// Returns parts of the script.
        /// </summary>
        /// <returns>Script parts of the script</returns>
        public List<ScriptPart> GetScriptParts()
        {
            List<ScriptPart> result = new List<ScriptPart>();

            string[] parts = Content.Split(new string[] { "&&" }, System.StringSplitOptions.RemoveEmptyEntries);
            foreach(string part in parts)
            {
                string[] scriptParts = part.Trim(' ').Split(new char[] { ' ' }, 2);
                result.Add(new ScriptPart(scriptParts[0], scriptParts[1]));
            }

            return result;
        }
    }

    /// <summary>
    /// Configuration volume.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public partial class Volume
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Volume() { }

        /// <summary>
        /// 2nd constructor.
        /// </summary>
        /// <param name="inputPath">Input path.</param>
        /// <param name="outputPath">Output path.</param>
        public Volume(string inputPath, string outputPath)
        {
            Name = "Archive";
            InputPath = inputPath;
            OutputPath = outputPath;
        }
        
        /// <summary>
        /// Index of the volume.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Name of the volume.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Input path of the volume.
        /// </summary>
        [JsonProperty("path")]
        public string InputPath { get; set; }

        /// <summary>
        /// Output path of the volume.
        /// </summary>
        [JsonProperty("output")]
        public string OutputPath { get; set; }

        /// <summary>
        /// Deployment path of the volume.
        /// </summary>
        [JsonProperty("deploy")]
        public string DeployPath { get; set; }
    }

    /// <summary>
    /// Configuration serialization partial.
    /// </summary>
    public partial class Configuration
    {
        public static Configuration FromJson(string json) => JsonConvert.DeserializeObject<Configuration>(json, Converter.Settings);
    }

    /// <summary>
    /// Serialize.
    /// </summary>
    public static class Serialize
    {
        public static string ToJson(this Configuration self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    /// <summary>
    /// Configuration converter.
    /// </summary>
    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
            Formatting = Formatting.Indented
        };
    }
}
