using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using SketchNow.Models.Enums;

namespace SketchNow.Models;

public sealed partial class CanvasWorkspace : ObservableObject
{
    private readonly ObservableCollection<CanvasPage> _whiteboardPages = [new()];

    public CanvasWorkspace() => WhiteboardPages = new ReadOnlyObservableCollection<CanvasPage>(_whiteboardPages);

    public ReadOnlyObservableCollection<CanvasPage> WhiteboardPages { get; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CurrentPage), nameof(IsWhiteboard))]
    [NotifyCanExecuteChangedFor(nameof(PreviousPageCommand), nameof(NextPageCommand), nameof(AddPageCommand))]
    public partial CanvasMode Mode { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CurrentPage), nameof(PageNumber))]
    [NotifyCanExecuteChangedFor(nameof(PreviousPageCommand), nameof(NextPageCommand))]
    public partial int SelectedPageIndex { get; private set; }

    public bool IsWhiteboard => Mode == CanvasMode.Whiteboard;
    public CanvasPage CurrentPage { get => IsWhiteboard ? _whiteboardPages[SelectedPageIndex] : field; } = new();
    public int PageNumber => SelectedPageIndex + 1;
    public int PageCount => _whiteboardPages.Count;

    [RelayCommand(CanExecute = nameof(CanGoToPreviousPage))]
    private void PreviousPage()
    {
        if (CanGoToPreviousPage())
            SelectedPageIndex--;
    }

    private bool CanGoToPreviousPage() => IsWhiteboard && SelectedPageIndex > 0;

    [RelayCommand(CanExecute = nameof(CanGoToNextPage))]
    private void NextPage()
    {
        if (CanGoToNextPage())
            SelectedPageIndex++;
    }

    private bool CanGoToNextPage() => IsWhiteboard && SelectedPageIndex < PageCount - 1;

    [RelayCommand(CanExecute = nameof(IsWhiteboard))]
    private void AddPage()
    {
        if (!IsWhiteboard)
            return;

        _whiteboardPages.Insert(SelectedPageIndex + 1, new CanvasPage());
        OnPropertyChanged(nameof(PageCount));
        SelectedPageIndex++;
    }
}
