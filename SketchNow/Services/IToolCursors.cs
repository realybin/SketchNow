using System.Windows.Input;

namespace SketchNow.Services;

public interface IToolCursors
{
    Cursor Pen { get; }
    Cursor Eraser { get; }
    Cursor Selection { get; }
}