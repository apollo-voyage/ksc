using System.Collections.Generic;
using kOS.Cli.IO;
using kOS.Safe.Compilation;

namespace kOS.Cli.Models
{
    public class Kerboscript
    { 
        public string InputPath { get; set; }
        public string OutputPath { get; set; }
        public string Content { get; set; }
        public List<CodePart> CompiledContent { get; set; }
        public CliVolume InputVolume { get; set; }
        public CliVolume OutputVolume { get; set; }
    }
}
