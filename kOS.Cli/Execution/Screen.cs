using System.Collections.Generic;
using System;
using kOS.Safe.Screen;


namespace kOS.Cli.Execution
{
    public class Screen : IScreenBuffer
    {
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
        public Queue<char> CharInputQueue => new Queue<char>();
        public int TopRow => 0;

        public List<string> Output { get; } = new List<string>();
        public void ClearScreen() => Output.Clear();
        public string DebugDump() => "";
        public void Print(string textToPrint) => Output.Add(textToPrint);
        public void Print(string textToPrint, bool addNewLine) => Output.Add(textToPrint);
        public void PrintAt(string textToPrint, int row, int column) => Output.Add(textToPrint);

        public void AddResizeNotifier(ScreenBuffer.ResizeNotifier notifier) => throw new NotSupportedException();
        public void AddSubBuffer(SubBuffer subBuffer) => throw new NotSupportedException();
        public List<IScreenBufferLine> GetBuffer() => throw new NotSupportedException();
        public void MoveCursor(int row, int column) => throw new NotSupportedException();
        public void MoveToNextLine() => throw new NotSupportedException();
        public void RemoveAllResizeNotifiers() => throw new NotSupportedException();
        public void RemoveResizeNotifier(ScreenBuffer.ResizeNotifier notifier) => throw new NotSupportedException();
        public void RemoveSubBuffer(SubBuffer subBuffer) => throw new NotSupportedException();
        public int ScrollVertical(int deltaRows) => throw new NotSupportedException();
        public void SetSize(int rowCount, int columnCount) => throw new NotSupportedException();
    }
}
