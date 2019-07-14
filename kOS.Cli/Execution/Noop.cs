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

    public class NoopInterpreter : IInterpreter
    {
        public int CharacterPixelWidth { get => 0; set { } }
        public int CharacterPixelHeight { get => 0; set { } }
        public double Brightness { get => 1; set { } }
        public int CursorRowShow => 0;
        public int CursorColumnShow => 0;
        public int RowCount => 0;
        public int ColumnCount => 0;
        public int AbsoluteCursorRow { get => 0; set { } }
        public int BeepsPending { get => 0; set { } }
        public bool ReverseScreen { get => false; set { } }
        public bool VisualBeep { get => false; set { } }
        public Queue<char> CharInputQueue => new Queue<char>();
        public int TopRow => 0;

        public string DebugDump() => "";
        public void Print(string textToPrint) { }
        public void Print(string textToPrint, bool addNewLine) { }
        public void PrintAt(string textToPrint, int row, int column) { }

        public void AddResizeNotifier(ScreenBuffer.ResizeNotifier notifier) => throw new NotImplementedException();
        public void AddSubBuffer(SubBuffer subBuffer) => throw new NotImplementedException();
        public List<IScreenBufferLine> GetBuffer() => throw new NotImplementedException();
        public void MoveCursor(int row, int column) => throw new NotImplementedException();
        public void MoveToNextLine() => throw new NotImplementedException();
        public void RemoveAllResizeNotifiers() => throw new NotImplementedException();
        public void RemoveResizeNotifier(ScreenBuffer.ResizeNotifier notifier) => throw new NotImplementedException();
        public void RemoveSubBuffer(SubBuffer subBuffer) => throw new NotImplementedException();
        public int ScrollVertical(int deltaRows) => throw new NotImplementedException();
        public void SetSize(int rowCount, int columnCount) => throw new NotImplementedException();

        public void Type(char ch) { }
        public bool SpecialKey(char key) => false;
        public string GetCommandHistoryAbsolute(int absoluteIndex) => "";
        public void SetInputLock(bool isLocked) { }
        public bool IsAtStartOfCommand() => false;
        public bool IsWaitingForCommand() => false;
        public void Reset() { }
        public void ClearScreen() { }
    }
}
