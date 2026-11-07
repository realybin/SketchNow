using SketchNow.Models;

namespace SketchNow.Services;

public sealed class UserSettingsStore : ISettingsStore
{
    public AppSettings Load() => new()
    {
        FitToCurve = Properties.Settings.Default.IsFitToCurve,
        IgnorePressure = Properties.Settings.Default.IsIgnorePressure,
        EraseByStroke = Properties.Settings.Default.IsEraseByStroke,
        CanvasBackground = Properties.Settings.Default.SelectedBackground
    };

    public void Save(AppSettings settings)
    {
        Properties.Settings.Default.IsFitToCurve = settings.FitToCurve;
        Properties.Settings.Default.IsIgnorePressure = settings.IgnorePressure;
        Properties.Settings.Default.IsEraseByStroke = settings.EraseByStroke;
        Properties.Settings.Default.SelectedBackground = settings.CanvasBackground;
        Properties.Settings.Default.Save();
    }
}