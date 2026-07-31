using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
// using Gestion_avion.Views;

namespace Gestion_avion.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private bool _isLoading = true;

    public MainViewModel() {
        _= InitializeAppAsync();
    }

    private async Task InitializeAppAsync()
    {
        await Task.Delay(2500);
        IsLoading = false;
    }

}
