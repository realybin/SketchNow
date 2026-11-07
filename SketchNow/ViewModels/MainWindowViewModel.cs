using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using SketchNow.Models;
using SketchNow.Models.Enums;
using SketchNow.Services;

namespace SketchNow.ViewModels;

public sealed partial class MainWindowViewModel : ObservableObject
{
    private readonly IToolCursors _toolCursors;

    public MainWindowViewModel(AppSettings settings, IToolCursors toolCursors)
    {
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(toolCursors);

        Settings = settings;
        _toolCursors = toolCursors;
        DrawingAttributes = new DrawingAttributes
        {
            Color = SelectedInkColor,
            Width = SelectedStrokeWidth,
            Height = SelectedStrokeWidth,
            FitToCurve = settings.FitToCurve,
            IgnorePressure = settings.IgnorePressure
        };
        settings.PropertyChanged += OnSettingsChanged;
    }

    public AppSettings Settings { get; }
    public CanvasWorkspace Workspace { get; } = new();
    public DrawingAttributes DrawingAttributes { get; }

    public IReadOnlyList<Color> InkColors { get; } =
    [
        Color.FromRgb(28, 27, 31),
        Colors.White,
        Color.FromRgb(255, 26, 0),
        Color.FromRgb(47, 47, 255),
        Color.FromRgb(0, 174, 128),
        Color.FromRgb(157, 118, 241),
        Color.FromRgb(255, 219, 29),
        Color.FromRgb(234, 43, 180)
    ];

    public IReadOnlyList<double> StrokeWidths { get; } = [5, 7, 9, 11, 13, 20];

    [ObservableProperty]
    public partial Color SelectedInkColor { get; set; } = Color.FromRgb(28, 27, 31);

    partial void OnSelectedInkColorChanged(Color value) => DrawingAttributes.Color = value;

    [ObservableProperty]
    public partial double SelectedStrokeWidth { get; set; } = 5;

    partial void OnSelectedStrokeWidthChanged(double value) =>
        DrawingAttributes.Width = DrawingAttributes.Height = value;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(EditingMode), nameof(ToolCursor), nameof(OverlayBackground))]
    public partial DrawingTool SelectedTool { get; set; }

    public InkCanvasEditingMode EditingMode => SelectedTool switch
    {
        DrawingTool.Pen => InkCanvasEditingMode.Ink,
        DrawingTool.Eraser => Settings.EraseByStroke
            ? InkCanvasEditingMode.EraseByStroke
            : InkCanvasEditingMode.EraseByPoint,
        DrawingTool.Selection => InkCanvasEditingMode.Select,
        _ => InkCanvasEditingMode.None
    };

    public Cursor ToolCursor => SelectedTool switch
    {
        DrawingTool.Pen => _toolCursors.Pen,
        DrawingTool.Eraser => _toolCursors.Eraser,
        DrawingTool.Selection => _toolCursors.Selection,
        _ => Cursors.Arrow
    };

    public Brush OverlayBackground { get => SelectedTool == DrawingTool.Pointer
        ? Brushes.Transparent
        : field; } = new SolidColorBrush(Color.FromArgb(1, 0, 0, 0));

    [RelayCommand]
    private void SelectTool(DrawingTool tool) => SelectedTool = tool;

    [RelayCommand]
    private void SelectCanvasMode(CanvasMode mode) => Workspace.Mode = mode;

    private void OnSettingsChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(AppSettings.FitToCurve):
                DrawingAttributes.FitToCurve = Settings.FitToCurve;
                break;
            case nameof(AppSettings.IgnorePressure):
                DrawingAttributes.IgnorePressure = Settings.IgnorePressure;
                break;
            case nameof(AppSettings.EraseByStroke):
                OnPropertyChanged(nameof(EditingMode));
                break;
        }
    }
}