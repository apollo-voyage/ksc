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
    }

    public partial class Script
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }
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
