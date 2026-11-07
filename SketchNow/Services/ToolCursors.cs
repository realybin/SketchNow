using System.Windows;
using System.Windows.Input;

namespace SketchNow.Services;

public sealed class ToolCursors : IToolCursors, IDisposable
{
    public Cursor Pen { get; } = LoadCursor("Circle.cur");
    public Cursor Eraser { get; } = LoadCursor("Remove.cur");
    public Cursor Selection { get; } = LoadCursor("Select.cur");

    private static Cursor LoadCursor(string resourceName)
    {
        var uri = new Uri($"/SketchNow;component/Resources/{resourceName}", UriKind.Relative);
        using var stream = Application.GetResourceStream(uri)?.Stream
            ?? throw new InvalidOperationException($"Cursor resource not found: {resourceName}");
        return new Cursor(stream);
    }

    public void Dispose()
    {
        Pen.Dispose();
        Eraser.Dispose();
        Selection.Dispose();
    }
}