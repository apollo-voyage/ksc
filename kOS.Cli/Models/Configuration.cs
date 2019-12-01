namespace kOS.Cli.Models
{
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// ksc configuration.
    /// </summary>
    public partial class Configuration
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Configuration()
        {
            Volumes = new List<Volume>();
            Scripts = new List<Script>();
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
        public List<Volume> Volumes { get; set; }

        /// <summary>
        /// Configuration scripts.
        /// </summary>
        [JsonProperty("scripts")]
        public List<Script> Scripts { get; set; }

        /// <summary>
        /// Libs that can be installed.
        /// </summary>
        // public List<Library> Libs;

        /// <summary>
        /// Returns volumes based on a given name or index.
        /// </summary>
        /// <param name="nameOrIndex"></param>
        /// <returns></returns>
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
        /// Checks wheter the configuration is semantically valid.
        /// </summary>
        /// <returns>A list of messages of invalidities.</returns>
        public List<string> IsValid()
        {
            List<string> result = new List<string>();

            Volume volume = Volumes.Find(v => v.Index == 0);
            if (volume != null)
            {
                result.Add("Volume " + volume.Name + " has a 0 index; this is permitted as the 0 index is reserved for the archive volume");
            }

            return result;
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
            public string Program { get; private set; }

            public string Arguments { get; private set; }

            public ScriptPart(string program, string arguments)
            {
                Program = program;
                Arguments = arguments;
            }
        }

        /// <summary>
        /// Name of the script.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Content of the script.
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; set; }

        /// <summary>
        /// Returns all parts of a given script.
        /// </summary>
        /// <returns></returns>
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
            Index = 0;
            Name = "Archive";
            InputPath = inputPath;
            OutputPath = outputPath;
        }

        /// <summary>
        /// Index of the volume.
        /// </summary>
        [JsonProperty("index")]
        public long Index { get; set; }

        /// <summary>
        /// Name of the volume.
        /// </summary>
        [JsonProperty("name")]
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
