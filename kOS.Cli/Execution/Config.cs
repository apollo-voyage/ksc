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
        public int InstructionsPerUpdate { get => 2000; set { /* No need to set the property, but the interfaces demands the ''set'. */ } }
        public bool UseCompressedPersistence { get => false; set { /* No need to set the property, but the interfaces demands the ''set'. */ } }
        public bool ShowStatistics { get => false; set { /* No need to set the property, but the interfaces demands the ''set'. */ } }
        public bool StartOnArchive { get => false; set { /* No need to set the property, but the interfaces demands the ''set'. */ } }
        public bool ObeyHideUI { get => true; set { /* No need to set the property, but the interfaces demands the ''set'. */ } }
        public bool EnableSafeMode { get => true; set { /* No need to set the property, but the interfaces demands the ''set'. */ } }
        public bool AudibleExceptions { get => false; set { /* No need to set the property, but the interfaces demands the ''set'. */ } }
        public bool VerboseExceptions { get => true; set { /* No need to set the property, but the interfaces demands the ''set'. */ } }
        public bool EnableTelnet { get => false; set { /* No need to set the property, but the interfaces demands the ''set'. */ } }
        public int TelnetPort { get => 0; set { /* No need to set the property, but the interfaces demands the ''set'. */ } }
        public string TelnetIPAddrString { get =>  ""; set { /* No need to set the property, but the interfaces demands the ''set'. */ } }
        public int TerminalFontDefaultSize { get => 12; set { /* No need to set the property, but the interfaces demands the ''set'. */ } }
        public string TerminalFontName { get => ""; set { /* No need to set the property, but the interfaces demands the ''set'. */ } }
        public bool UseBlizzyToolbarOnly { get => false; set { /* No need to set the property, but the interfaces demands the ''set'. */ } }
        public double TerminalBrightness { get => 1; set { /* No need to set the property, but the interfaces demands the ''set'. */ } }
        public bool DebugEachOpcode { get => false; set { /* No need to set the property, but the interfaces demands the ''set'. */ } }

        public DateTime TimeStamp => new DateTime();

        public int TerminalDefaultWidth { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int TerminalDefaultHeight { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IList<ConfigKey> GetConfigKeys() => new List<ConfigKey>();
        public ISuffixResult GetSuffix(string suffixName) => throw new NotImplementedException();
        public ISuffixResult GetSuffix(string suffixName, bool failOkay = false) => throw new NotImplementedException();

        public void SaveConfig() => throw new NotImplementedException();
        public bool SetSuffix(string suffixName, object value) => throw new NotImplementedException();
        public bool SetSuffix(string suffixName, object value, bool failOkay = false) => throw new NotImplementedException();
    }
}
