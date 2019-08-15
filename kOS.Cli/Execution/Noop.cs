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
        public void Log(string text) { /* As this is a noop Logger, we don't need this. */ } 
        public void Log(Exception e) => Console.Error.WriteLine(e);
        public void LogError(string s) => Console.Error.WriteLine(s);
        public void LogException(Exception exception) => Console.Error.WriteLine(exception);
        public void LogWarning(string s) { /* As this is a noop Logger, we don't need this. */ }
        public void LogWarningAndScreen(string s) { /* As this is a noop Logger, we don't need this. */ }
        public void SuperVerbose(string s) { /* As this is a noop Logger, we don't need this. */ }
    }

    /// <summary>
    /// Stub execution game event dispatcher manager.
    /// </summary>
    public class NoopGameEventDispatchManager : IGameEventDispatchManager
    {
        public void Clear() { /* As this is a noop GameEventDispatchManager, we don't need this. */ }
        public void RemoveDispatcherFor(ProgramContext context) { /* As this is a noop GameEventDispatchManager, we don't need this. */ }
        public void SetDispatcherFor(ProgramContext context) { /* As this is a noop GameEventDispatchManager, we don't need this. */ }
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
        public void SetMode(ProcessorModes newProcessorMode) { /* As this is a noop Processor, we don't need this. */ }
    }

    public class NoopInterpreter : IInterpreter
    {
        private IScreenBuffer _screen;

        public NoopInterpreter(IScreenBuffer Screen)
        {
            _screen = Screen;
        }

        public int CharacterPixelWidth { get => 0; set { /* No need to set the property, but the interfaces demands the ''set'. */ } }
        public int CharacterPixelHeight { get => 0; set { /* No need to set the property, but the interfaces demands the ''set'. */ } }
        public double Brightness { get => 1; set { /* No need to set the property, but the interfaces demands the ''set'. */ } }
        public int CursorRowShow => 0;
        public int CursorColumnShow => 0;
        public int RowCount => 0;
        public int ColumnCount => 0;
        public int AbsoluteCursorRow { get => 0; set { /* No need to set the property, but the interfaces demands the ''set'. */ } }
        public int BeepsPending { get => 0; set { /* No need to set the property, but the interfaces demands the ''set'. */ } }
        public bool ReverseScreen { get => false; set { /* No need to set the property, but the interfaces demands the ''set'. */ } }
        public bool VisualBeep { get => false; set { /* No need to set the property, but the interfaces demands the ''set'. */ } }
        public Queue<char> CharInputQueue => null;
        public int TopRow => 0;

        public void AddResizeNotifier(ScreenBuffer.ResizeNotifier notifier) { /* As this is a noop Interpreter, we don't need this. */ }
        public void AddSubBuffer(SubBuffer subBuffer) { /* As this is a noop Interpreter, we don't need this. */ }
        public void ClearScreen() { /* As this is a noop Interpreter, we don't need this. */ }
        public string DebugDump() => "";
        public List<IScreenBufferLine> GetBuffer() => null;
        public string GetCommandHistoryAbsolute(int absoluteIndex) => "";
        public bool IsAtStartOfCommand() => false;
        public bool IsWaitingForCommand() => false;
        public void MoveCursor(int row, int column) { /* As this is a noop Interpreter, we don't need this. */ }
        public void MoveToNextLine() { /* As this is a noop Interpreter, we don't need this. */ }
        public void RemoveAllResizeNotifiers() { /* As this is a noop Interpreter, we don't need this. */ }
        public void RemoveResizeNotifier(ScreenBuffer.ResizeNotifier notifier) { /* As this is a noop Interpreter, we don't need this. */ }
        public void RemoveSubBuffer(SubBuffer subBuffer) { /* As this is a noop Interpreter, we don't need this. */ }
        public void Reset() { /* As this is a noop Interpreter, we don't need this. */ }
        public int ScrollVertical(int deltaRows) => 0;
        public void SetInputLock(bool isLocked) { /* As this is a noop Interpreter, we don't need this. */ }
        public void SetSize(int rowCount, int columnCount) { /* As this is a noop Interpreter, we don't need this. */ }
        public bool SpecialKey(char key) => false;
        public void Type(char ch) { /* As this is a noop Interpreter, we don't need this. */ }

        public void Print(string textToPrint) => _screen.Print(textToPrint);
        public void Print(string textToPrint, bool addNewLine) => _screen.Print(textToPrint, addNewLine);
        public void PrintAt(string textToPrint, int row, int column) => _screen.PrintAt(textToPrint, row, column);
    }
}
