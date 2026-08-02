using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
// using Gestion_avion.ViewModels;

namespace Gestion_avion.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    //initialisation
    [ObservableProperty]
    private ViewModelBase? _currentPage;

    [ObservableProperty]
    private bool _isLoading = true;

    [ObservableProperty]
    private string _loadingMessage = "Démarrage de Fast Travel...";

    public MainViewModel()                                    
    {
        _ = InitializeAppAsync();
    }

    private async Task InitializeAppAsync()
    {
        LoadingMessage = "Connexion à la base de données...";
        await Task.Delay(1000);

        LoadingMessage = "Chargement de la liste des vols...";
        await Task.Delay(1000);

        LoadingMessage = "Préparation de l'interface...";
        await Task.Delay(500);

        CurrentPage = new DashboardViewModel();

        IsLoading = false;
    }

    //navigation entre les pages
    [RelayCommand]
    private void GoToDashboard() => CurrentPage = new DashboardViewModel();

    [RelayCommand]
    private void GoToOperation() => CurrentPage = new OperationViewModel();

    [RelayCommand]
    private void GoToVols() => CurrentPage = new VolsViewModel();

    [RelayCommand]
    private void GoToReservation() => CurrentPage = new ReservationViewModel();

}