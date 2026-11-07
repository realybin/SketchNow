using System.Windows.Ink;

namespace SketchNow.Models;

internal sealed class StrokeHistory(StrokeCollection initialStrokes)
{
    private const int MaxUndoSteps = 100;
    private readonly List<StrokeCollection> _snapshots = [initialStrokes.Clone()];
    private int _position;

    public bool CanUndo => _position > 0;
    public bool CanRedo => _position < _snapshots.Count - 1;

    public void Record(StrokeCollection strokes)
    {
        _snapshots.RemoveRange(_position + 1, _snapshots.Count - _position - 1);
        _snapshots.Add(strokes.Clone());

        if (_snapshots.Count > MaxUndoSteps + 1)
            _snapshots.RemoveAt(0);

        _position = _snapshots.Count - 1;
    }

    public StrokeCollection Undo() => _snapshots[--_position].Clone();

    public StrokeCollection Redo() => _snapshots[++_position].Clone();
}