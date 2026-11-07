using Velopack;

namespace SketchNow.Services;

public sealed class VelopackUpdateService(UpdateManager updateManager) : IAppUpdateService
{
    public Task<UpdateInfo?> CheckForUpdatesAsync() => updateManager.CheckForUpdatesAsync();

    public Task DownloadUpdatesAsync(UpdateInfo update, IProgress<int> progress) =>
        updateManager.DownloadUpdatesAsync(update, progress.Report);

    public void InstallAndRestart(UpdateInfo update) => updateManager.ApplyUpdatesAndRestart(update);
}