using System;
using kOS.Safe;
using kOS.Safe.Module;
using kOS.Safe.Callback;
using kOS.Safe.Execution;
using kOS.Safe.Persistence;
using kOS.Safe.Screen;
using System.Collections.Generic;

namespace kOS.Cli.Execution
{
    /// <summary>
    /// Stub noop execution logger.
    /// </summary>
    public class NoopLogger : ILogger
    {
        public void Log(string text) { }
        public void Log(Exception e) => Console.Error.WriteLine(e);
        public void LogError(string s) => Console.Error.WriteLine(s);
        public void LogException(Exception exception) => Console.Error.WriteLine(exception);
        public void LogWarning(string s) { }
        public void LogWarningAndScreen(string s) { }
        public void SuperVerbose(string s) { }
    }

    /// <summary>
    /// Stub execution game event dispatcher manager.
    /// </summary>
    public class NoopGameEventDispatchManager : IGameEventDispatchManager
    {
        public void Clear() { }
        public void RemoveDispatcherFor(ProgramContext context) { }
        public void SetDispatcherFor(ProgramContext context) { }
    }

    /// <summary>
    /// Stub execution processor.
    /// </summary>
    public class NoopProcessor : IProcessor
    {
        public VolumePath BootFilePath => null;
        public string Tag => "ksc";
        public int KOSCoreId => 0;
        public bool CheckCanBoot() => true;
        public void SetMode(ProcessorModes newProcessorMode) { }
    }
}
