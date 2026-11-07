using System.Windows;
using System.Windows.Media;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using MaterialDesignThemes.Wpf;

using SketchNow.Models;
using SketchNow.Services;

using Velopack;

namespace SketchNow.ViewModels;

public sealed partial class SettingsViewModel : ObservableObject
{
    public IReadOnlyList<Brush> BackgroundBrushes { get; } =
    [
        new SolidColorBrush(Colors.White),
        new SolidColorBrush(Color.FromRgb(255, 253, 209)),
        new SolidColorBrush(Color.FromRgb(1, 71, 54)),
        new SolidColorBrush(Color.FromRgb(25, 25, 25)),
        new DrawingBrush
        {
            Viewport = new Rect(0, 0, 80, 80),
            ViewportUnits = BrushMappingMode.Absolute,
            TileMode = TileMode.Tile,
            Drawing = new DrawingGroup
            {
                Children =
                {
                    new GeometryDrawing
                    {
                        Brush = Brushes.White, Geometry = new RectangleGeometry(new Rect(0, 0, 1, 1))
                    },
                    new GeometryDrawing
                    {
                        Brush = new SolidColorBrush(Color.FromArgb(76, 0, 0, 0)),
                        Geometry = Geometry.Parse("M 0,0 L 0,1 0.1,1 0.1,0.1 1,0.1 1,0 Z")
                    }
                }
            }
        }
    ];

    private readonly IAppUpdateService _updateService;

    public SettingsViewModel(AppSettings settings, ISnackbarMessageQueue messageQueue, IAppUpdateService updateService)
    {
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(messageQueue);
        ArgumentNullException.ThrowIfNull(updateService);

        Settings = settings;
        MessageQueue = messageQueue;
        _updateService = updateService;
    }

    public AppSettings Settings { get; }

    public ISnackbarMessageQueue MessageQueue { get; }

    [RelayCommand]
    private static void ExitApplication()
    {
        Application.Current.Shutdown();
    }

    public UpdateProgress UpdateProgress { get; } = new();

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(CheckForUpdatesCommand))]
    [NotifyCanExecuteChangedFor(nameof(InstallUpdateCommand))]
    public partial bool IsUpdateInProgress { get; private set; }

    private bool CanUpdate() => !IsUpdateInProgress;

    [RelayCommand(CanExecute = nameof(CanUpdate))]
    private async Task CheckForUpdatesAsync()
    {
        if (IsUpdateInProgress)
            return;

        IsUpdateInProgress = true;
        try
        {
            UpdateInfo? update = await _updateService.CheckForUpdatesAsync();
            if (update is not null)
                MessageQueue.Enqueue(Properties.Resource.NewVersionFound, Properties.Resource.InstallAndUpdate,
                    _ => InstallUpdateCommand.ExecuteAsync(update),
                    null,
                    false,
                    true,
                    TimeSpan.FromSeconds(10));
            else
                MessageQueue.Enqueue(Properties.Resource.UpdatesAreNotAvailable);
        }
        catch (Exception e)
        {
            MessageQueue.Enqueue(e.Message);
        }
        finally
        {
            IsUpdateInProgress = false;
        }
    }

    [RelayCommand(CanExecute = nameof(CanUpdate))]
    private async Task InstallUpdateAsync(UpdateInfo update)
    {
        if (IsUpdateInProgress)
            return;

        IsUpdateInProgress = true;
        UpdateProgress.Percentage = 0;
        UpdateProgress.IsVisible = true;
        UpdateProgress.IsIndeterminate = false;

        try
        {
            MessageQueue.Enqueue(Properties.Resource.DownloadingUpdatesPleaseWait);
            IProgress<int> progress = new Progress<int>(value => UpdateProgress.Percentage = value);
            await _updateService.DownloadUpdatesAsync(update, progress);
            MessageQueue.Enqueue(Properties.Resource.DownloadedUpdatesPleaseWait);
            UpdateProgress.IsIndeterminate = true;
            _updateService.InstallAndRestart(update);
        }
        catch (Exception e)
        {
            MessageQueue.Enqueue(e.Message);
        }
        finally
        {
            UpdateProgress.IsVisible = false;
            UpdateProgress.IsIndeterminate = false;
            IsUpdateInProgress = false;
        }
    }
}