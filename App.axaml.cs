using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Gestion_avion.ViewModels;
using Gestion_avion.Views;
using Gestion_avion.state;

namespace Gestion_avion;

public partial class App : Application
{
    private readonly AppState _appState = new AppState();

    //activation du font awesome
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainViewModel(_appState),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}