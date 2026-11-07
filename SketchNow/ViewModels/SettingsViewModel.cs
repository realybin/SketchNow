using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging.Messages;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Input;
using Velopack;
using MaterialDesignThemes.Wpf;
using SketchNow.Models;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace SketchNow.ViewModels;

public partial class SettingsViewModel : ObservableRecipient
{
    [ObservableProperty]
    public partial ObservableCollection<Brush> BackGroundBrushes { get; set; } =
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

    private readonly UpdateManager _updateManager;

    public SettingsViewModel(IMessenger messenger, ISnackbarMessageQueue messageQueue, UpdateManager updateManager)
    {
        MessageQueue = messageQueue ?? throw new ArgumentNullException(nameof(messageQueue));
        _updateManager = updateManager ?? throw new ArgumentNullException(nameof(updateManager));
        IsActive = true;

        Settings.PropertyChanged += (_, _) =>
        {
            Settings.SaveSettingsCommand.Execute(null);
            messenger.Send(
                new ValueChangedMessage<Settings>(Settings)
            );
        };
    }

    [ObservableProperty]
    [NotifyPropertyChangedRecipients]
    public partial Settings Settings { get; set; } = new();

    [ObservableProperty]
    public partial ISnackbarMessageQueue MessageQueue { get; set; }

    [RelayCommand]
    private static void Close()
    {
        Application.Current.Shutdown();
    }

    [ObservableProperty]
    public partial Progress Progress { get; set; } = new();

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(CheckForUpdatesCommand))]
    [NotifyCanExecuteChangedFor(nameof(UpdateAppCommand))]
    public partial bool IsUpdating { get; set; }

    private bool CanUpdate() => !IsUpdating;

    [RelayCommand(CanExecute = nameof(CanUpdate))]
    private async Task CheckForUpdates()
    {
        if (IsUpdating)
            return;

        IsUpdating = true;
        try
        {
            UpdateInfo? newVersion = await _updateManager.CheckForUpdatesAsync();
            if (newVersion != null)
                MessageQueue.Enqueue(Properties.Resource.NewVersionFound, Properties.Resource.InstallAndUpdate,
                    _ => UpdateAppCommand.ExecuteAsync(newVersion),
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
            IsUpdating = false;
        }
    }

    [RelayCommand(CanExecute = nameof(CanUpdate))]
    private async Task UpdateApp(UpdateInfo newVersion)
    {
        if (IsUpdating)
            return;

        IsUpdating = true;
        Progress.Value = 0;
        Progress.IsVisible = true;
        Progress.IsIndeterminate = false;

        try
        {
            MessageQueue.Enqueue(Properties.Resource.DownloadingUpdatesPleaseWait);
            IProgress<int> progress = new System.Progress<int>(value => Progress.Value = value);
            await _updateManager.DownloadUpdatesAsync(newVersion, progress.Report);
            MessageQueue.Enqueue(Properties.Resource.DownloadedUpdatesPleaseWait);
            Progress.IsIndeterminate = true;
            _updateManager.ApplyUpdatesAndRestart(newVersion);
        }
        catch (Exception e)
        {
            MessageQueue.Enqueue(e.Message);
        }
        finally
        {
            Progress.IsVisible = false;
            Progress.IsIndeterminate = false;
            IsUpdating = false;
        }
    }

}


