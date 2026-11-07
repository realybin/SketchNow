using Velopack;

namespace SketchNow.Services;

public interface IAppUpdateService
{
    Task<UpdateInfo?> CheckForUpdatesAsync();
    Task DownloadUpdatesAsync(UpdateInfo update, IProgress<int> progress);
    void InstallAndRestart(UpdateInfo update);
}