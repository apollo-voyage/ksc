namespace kOS.Cli.Models
{
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class Configuration
    {
        public Configuration()
        {
            Volumes = new List<Volume>();
            Scripts = new List<Script>();
        }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("archive")]
        public string Archive { get; set; }

        [JsonProperty("volumes")]
        public List<Volume> Volumes { get; set; }

        [JsonProperty("scripts")]
        public List<Script> Scripts { get; set; }

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
    }

    public partial class Script
    {
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

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }


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

    public partial class Volume
    {
        public Volume() { }
        public Volume(string InputPath, string OutputPath)
        {
            this.Index = 0;
            this.Name = "Archive";
            this.InputPath = InputPath;
            this.OutputPath = OutputPath;
        }

        [JsonProperty("index")]
        public long Index { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("path")]
        public string InputPath { get; set; }

        [JsonProperty("output")]
        public string OutputPath { get; set; }

        [JsonProperty("deploy")]
        public string DeployPath { get; set; }
    }

    public partial class Configuration
    {
        public static Configuration FromJson(string json) => JsonConvert.DeserializeObject<Configuration>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Configuration self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

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
