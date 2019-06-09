using System.Collections.Generic;
using kOS.Safe.Compilation;
using kOS.Safe.Persistence;

namespace kOS.Cli.Models
{
    public class Kerboscript
    { 
        public string InputPath { get; set; }
        public string OutputPath { get; set; }
        public string Content { get; set; }
        public List<CodePart> CompiledContent { get; set; }
        public Archive InputArchive { get; set; }
        public Archive OutputArchive { get; set; }
    }
}
