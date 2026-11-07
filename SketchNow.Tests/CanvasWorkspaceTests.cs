using SketchNow.Models;
using SketchNow.Models.Enums;

namespace SketchNow.Tests;

public sealed class CanvasWorkspaceTests
{
    [Fact]
    public void SwitchingModesKeepsTheDesktopAndLastWhiteboardPageSeparate()
    {
        var workspace = new CanvasWorkspace();
        var desktopPage = workspace.CurrentPage;
        Assert.False(workspace.AddPageCommand.CanExecute(null));

        workspace.Mode = CanvasMode.Whiteboard;
        var firstWhiteboardPage = workspace.CurrentPage;
        workspace.AddPageCommand.Execute(null);
        var secondWhiteboardPage = workspace.CurrentPage;

        workspace.Mode = CanvasMode.Desktop;
        Assert.Same(desktopPage, workspace.CurrentPage);
        Assert.False(workspace.PreviousPageCommand.CanExecute(null));
        Assert.False(workspace.NextPageCommand.CanExecute(null));

        workspace.Mode = CanvasMode.Whiteboard;
        Assert.Same(secondWhiteboardPage, workspace.CurrentPage);
        Assert.NotSame(desktopPage, firstWhiteboardPage);
        Assert.NotSame(firstWhiteboardPage, secondWhiteboardPage);
        Assert.Equal(2, workspace.PageNumber);
        Assert.Equal(2, workspace.PageCount);
    }

    [Fact]
    public void NavigationStaysWithinWhiteboardPagesAndUpdatesCommandStates()
    {
        var workspace = new CanvasWorkspace { Mode = CanvasMode.Whiteboard };
        var nextCommandChanges = 0;
        workspace.NextPageCommand.CanExecuteChanged += (_, _) => nextCommandChanges++;
        Assert.False(workspace.PreviousPageCommand.CanExecute(null));
        Assert.False(workspace.NextPageCommand.CanExecute(null));

        workspace.AddPageCommand.Execute(null);
        workspace.PreviousPageCommand.Execute(null);

        Assert.Equal(1, workspace.PageNumber);
        Assert.False(workspace.PreviousPageCommand.CanExecute(null));
        Assert.True(workspace.NextPageCommand.CanExecute(null));
        workspace.PreviousPageCommand.Execute(null);
        Assert.Equal(1, workspace.PageNumber);

        workspace.NextPageCommand.Execute(null);
        workspace.NextPageCommand.Execute(null);
        Assert.Equal(2, workspace.PageNumber);
        Assert.False(workspace.NextPageCommand.CanExecute(null));
        Assert.True(nextCommandChanges > 0);
    }

    [Fact]
    public void AddingAPageInsertsAfterTheCurrentPage()
    {
        var workspace = new CanvasWorkspace { Mode = CanvasMode.Whiteboard };
        workspace.AddPageCommand.Execute(null);
        var lastPage = workspace.CurrentPage;
        workspace.PreviousPageCommand.Execute(null);

        workspace.AddPageCommand.Execute(null);

        Assert.Equal(3, workspace.PageCount);
        Assert.Equal(2, workspace.PageNumber);
        Assert.Same(workspace.WhiteboardPages[1], workspace.CurrentPage);
        Assert.Same(lastPage, workspace.WhiteboardPages[2]);
    }
}
