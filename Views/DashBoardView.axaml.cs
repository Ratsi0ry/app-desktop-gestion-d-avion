using Avalonia.Controls;
using Avalonia.Interactivity;
using Gestion_avion.ViewModels;

namespace Gestion_avion.Views;

public partial class DashboardView : UserControl
{
    public DashboardView()
    {
        InitializeComponent();
    }

    public async void DashboardView_OnLoaded(object? sender, RoutedEventArgs e) 
    {
        if(DataContext is DashboardViewModel vm)
        {
            await vm.OnLoaded();
        }
    }
}
