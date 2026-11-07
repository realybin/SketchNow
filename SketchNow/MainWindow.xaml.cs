using System.Windows.Input;

using SketchNow.ViewModels;

namespace SketchNow;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public MainWindow(
        MainWindowViewModel mainWindowViewModel,
        SettingsViewModel settingsViewModel)
    {
        InitializeComponent();

        DataContext = mainWindowViewModel;
        SettingsView.DataContext = settingsViewModel;

        CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, OnClose));
    }

    private void OnClose(object sender, ExecutedRoutedEventArgs e)
    {
        Close();
    }
}