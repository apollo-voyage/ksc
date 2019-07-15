using System;
using System.Collections.Generic;
using kOS.Safe.Encapsulation;
using kOS.Safe.Encapsulation.Suffixes;

namespace kOS.Cli.Execution
{
    /// <summary>
    /// Stub execution configuration.
    /// </summary>
    public class Config : IConfig
    {
        public int InstructionsPerUpdate { get => 2000; set { } }
        public bool UseCompressedPersistence { get => false; set { } }
        public bool ShowStatistics { get => false; set { } }
        public bool StartOnArchive { get => false; set { } }
        public bool ObeyHideUI { get => true; set { } }
        public bool EnableSafeMode { get => true; set { } }
        public bool AudibleExceptions { get => false; set { } }
        public bool VerboseExceptions { get => true; set { } }
        public bool EnableTelnet { get => false; set { } }
        public int TelnetPort { get => 0; set { } }
        public string TelnetIPAddrString { get =>  ""; set { } }
        public int TerminalFontDefaultSize { get => 12; set { } }
        public string TerminalFontName { get => ""; set { } }
        public bool UseBlizzyToolbarOnly { get => false; set { } }
        public double TerminalBrightness { get => 1; set { } }
        public bool DebugEachOpcode { get => false; set { } }

        public DateTime TimeStamp => new DateTime();
        public IList<ConfigKey> GetConfigKeys() => new List<ConfigKey>();
        public ISuffixResult GetSuffix(string suffixName) => throw new NotImplementedException();
        public void SaveConfig() => throw new NotImplementedException();
        public bool SetSuffix(string suffixName, object value) => throw new NotImplementedException();
    }
}
