using SketchNow.Models;

namespace SketchNow.Services;

public interface ISettingsStore
{
    AppSettings Load();
    void Save(AppSettings settings);
}