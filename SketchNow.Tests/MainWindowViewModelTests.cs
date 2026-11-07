using System.Windows.Controls;
using System.Windows.Media;

using SketchNow.Models;
using SketchNow.Models.Enums;
using SketchNow.Services;
using SketchNow.ViewModels;

namespace SketchNow.Tests;

public sealed class MainWindowViewModelTests
{
    [Theory]
    [InlineData(DrawingTool.Pointer, InkCanvasEditingMode.None)]
    [InlineData(DrawingTool.Pen, InkCanvasEditingMode.Ink)]
    [InlineData(DrawingTool.Eraser, InkCanvasEditingMode.EraseByStroke)]
    [InlineData(DrawingTool.Selection, InkCanvasEditingMode.Select)]
    public void SelectingAToolSetsItsEditingMode(DrawingTool tool, InkCanvasEditingMode expected)
    {
        var viewModel = new MainWindowViewModel(new AppSettings(), Mock.Of<IToolCursors>());

        viewModel.SelectToolCommand.Execute(tool);

        Assert.Equal(expected, viewModel.EditingMode);
    }

    [Fact]
    public void SharedSettingsApplyInitiallyAndUpdateTheActiveEraser()
    {
        var settings = new AppSettings { FitToCurve = false, IgnorePressure = true };
        var viewModel = new MainWindowViewModel(settings, Mock.Of<IToolCursors>())
        {
            SelectedTool = DrawingTool.Eraser
        };
        Assert.False(viewModel.DrawingAttributes.FitToCurve);
        Assert.True(viewModel.DrawingAttributes.IgnorePressure);
        var changedProperties = new List<string?>();
        viewModel.PropertyChanged += (_, e) => changedProperties.Add(e.PropertyName);

        settings.FitToCurve = true;
        settings.IgnorePressure = false;
        settings.EraseByStroke = false;

        Assert.True(viewModel.DrawingAttributes.FitToCurve);
        Assert.False(viewModel.DrawingAttributes.IgnorePressure);
        Assert.Equal(InkCanvasEditingMode.EraseByPoint, viewModel.EditingMode);
        Assert.Contains(nameof(MainWindowViewModel.EditingMode), changedProperties);
    }

    [Fact]
    public void DrawingAttributesMatchTheSelectedColorAndWidth()
    {
        var viewModel = new MainWindowViewModel(new AppSettings(), Mock.Of<IToolCursors>());
        Assert.Equal(viewModel.InkColors[0], viewModel.SelectedInkColor);
        Assert.Equal(viewModel.StrokeWidths[0], viewModel.SelectedStrokeWidth);
        Assert.Equal(viewModel.SelectedStrokeWidth, viewModel.DrawingAttributes.Width);

        viewModel.SelectedInkColor = Colors.Red;
        viewModel.SelectedStrokeWidth = 9;

        Assert.Equal(Colors.Red, viewModel.DrawingAttributes.Color);
        Assert.Equal(9, viewModel.DrawingAttributes.Width);
        Assert.Equal(9, viewModel.DrawingAttributes.Height);
    }
}