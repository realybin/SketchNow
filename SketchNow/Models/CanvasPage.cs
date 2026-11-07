using System.Windows.Ink;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SketchNow.Models;

public sealed partial class CanvasPage : ObservableObject
{
    private readonly StrokeHistory _history;

    public CanvasPage()
    {
        _history = new StrokeHistory(Strokes);
        Strokes.StrokesChanged += OnStrokeCollectionChanged;
    }

    [ObservableProperty]
    public partial StrokeCollection Strokes { get; private set; } = [];

    partial void OnStrokesChanging(StrokeCollection value) => Strokes.StrokesChanged -= OnStrokeCollectionChanged;

    partial void OnStrokesChanged(StrokeCollection value)
    {
        value.StrokesChanged += OnStrokeCollectionChanged;
        NotifyCommandStates();
    }

    private void OnStrokeCollectionChanged(object? sender, StrokeCollectionChangedEventArgs e)
    {
        _history.Record(Strokes);
        NotifyCommandStates();
    }

    [RelayCommand(CanExecute = nameof(CanUndo))]
    private void Undo()
    {
        if (CanUndo())
            Strokes = _history.Undo();
    }

    private bool CanUndo() => _history.CanUndo;

    [RelayCommand(CanExecute = nameof(CanRedo))]
    private void Redo()
    {
        if (CanRedo())
            Strokes = _history.Redo();
    }

    private bool CanRedo() => _history.CanRedo;

    [RelayCommand(CanExecute = nameof(CanClear))]
    private void Clear() => Strokes.Clear();

    private bool CanClear() => Strokes.Count > 0;

    private void NotifyCommandStates()
    {
        UndoCommand.NotifyCanExecuteChanged();
        RedoCommand.NotifyCanExecuteChanged();
        ClearCommand.NotifyCanExecuteChanged();
    }
}
