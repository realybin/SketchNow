using CommunityToolkit.Mvvm.ComponentModel;

namespace SketchNow.Models;

public sealed partial class UpdateProgress : ObservableObject
{
    [ObservableProperty]
    public partial int Percentage { get; set; }

    [ObservableProperty]
    public partial bool IsIndeterminate { get; set; }

    [ObservableProperty]
    public partial bool IsVisible { get; set; }
}