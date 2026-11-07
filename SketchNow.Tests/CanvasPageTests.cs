using System.Windows.Ink;
using System.Windows.Input;

using SketchNow.Models;

namespace SketchNow.Tests;

public sealed class CanvasPageTests
{
    [Fact]
    public void UndoAfterRedoAndNewStrokeRestoresTheRedoneState()
    {
        var page = new CanvasPage();
        page.Strokes.Add(CreateStroke(10));
        page.Strokes.Add(CreateStroke(20));

        page.UndoCommand.Execute(null);
        page.RedoCommand.Execute(null);
        page.Strokes.Add(CreateStroke(30));
        page.UndoCommand.Execute(null);

        Assert.Equal([10d, 20d], page.Strokes.Select(stroke => stroke.StylusPoints[0].X));

        page.UndoCommand.Execute(null);
        Assert.Single(page.Strokes);
        page.UndoCommand.Execute(null);
        Assert.Empty(page.Strokes);
        Assert.False(page.UndoCommand.CanExecute(null));
    }

    [Fact]
    public void NewStrokeAfterUndoDiscardsTheRedoBranch()
    {
        var page = new CanvasPage();
        page.Strokes.Add(CreateStroke(10));
        page.Strokes.Add(CreateStroke(20));
        page.UndoCommand.Execute(null);

        page.Strokes.Add(CreateStroke(30));

        Assert.False(page.RedoCommand.CanExecute(null));
        page.UndoCommand.Execute(null);
        page.RedoCommand.Execute(null);
        Assert.Equal([10d, 30d], page.Strokes.Select(stroke => stroke.StylusPoints[0].X));
    }

    [Fact]
    public void ClearCanBeUndoneAndRedoneAndNotifiesCommandStates()
    {
        var page = new CanvasPage();
        var commandStateChanges = 0;
        page.ClearCommand.CanExecuteChanged += (_, _) => commandStateChanges++;
        Assert.False(page.ClearCommand.CanExecute(null));

        page.Strokes.Add(CreateStroke(10));
        page.Strokes.Add(CreateStroke(20));
        page.ClearCommand.Execute(null);

        Assert.Empty(page.Strokes);
        Assert.False(page.ClearCommand.CanExecute(null));
        page.UndoCommand.Execute(null);
        Assert.Equal(2, page.Strokes.Count);
        Assert.True(page.ClearCommand.CanExecute(null));
        page.RedoCommand.Execute(null);
        Assert.Empty(page.Strokes);
        Assert.True(commandStateChanges > 0);
    }

    [Fact]
    public void ReplacedStrokeCollectionNoLongerRecordsChanges()
    {
        var page = new CanvasPage();
        page.Strokes.Add(CreateStroke(10));
        var previousStrokes = page.Strokes;
        page.UndoCommand.Execute(null);

        previousStrokes.Add(CreateStroke(20));

        Assert.Empty(page.Strokes);
        Assert.False(page.UndoCommand.CanExecute(null));
        Assert.True(page.RedoCommand.CanExecute(null));
        page.RedoCommand.Execute(null);
        Assert.Single(page.Strokes);
    }

    [Fact]
    public void HistoryRetainsAtMostOneHundredUndoSteps()
    {
        var page = new CanvasPage();
        for (var i = 0; i < 105; i++)
            page.Strokes.Add(CreateStroke(i));

        for (var i = 0; i < 100; i++)
            page.UndoCommand.Execute(null);

        Assert.Equal(5, page.Strokes.Count);
        Assert.False(page.UndoCommand.CanExecute(null));
        Assert.True(page.RedoCommand.CanExecute(null));
    }

    private static Stroke CreateStroke(double x) =>
        new(new StylusPointCollection([new StylusPoint(x, 0), new StylusPoint(x + 1, 1)]));
}