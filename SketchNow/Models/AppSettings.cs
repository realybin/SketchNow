using System.Windows.Media;

using CommunityToolkit.Mvvm.ComponentModel;

namespace SketchNow.Models;

public sealed partial class AppSettings : ObservableObject
{
    [ObservableProperty]
    public partial bool FitToCurve { get; set; } = true;

    [ObservableProperty]
    public partial bool IgnorePressure { get; set; }

    [ObservableProperty]
    public partial bool EraseByStroke { get; set; } = true;

    [ObservableProperty]
    public partial Brush CanvasBackground { get; set; } = Brushes.White;
}