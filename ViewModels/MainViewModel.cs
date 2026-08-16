using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Gestion_avion.Messages;

namespace Gestion_avion.ViewModels;

public partial class MainViewModel : ViewModelBase, IRecipient<DemandeModificationClientMessage>
{
    //initialisation
    [ObservableProperty]
    private ViewModelBase? _currentPage;

    [ObservableProperty]
    private bool _isLoading = true;

    [ObservableProperty]
    private string _loadingMessage = "Démarrage de Fast Travel...";

    private readonly DashboardViewModel _dashboardVm = new();
    private readonly OperationViewModel _operationVm = new();
    private readonly VolsViewModel _volsVm = new();
    private readonly ReservationViewModel _reservationVm = new();
    private readonly PassagerViewModel _passagerVm = new();

    public MainViewModel()                                    
    {
        _ = InitializeAppAsync();

        // Enregistrement
        WeakReferenceMessenger.Default.Register(this);
    }

    private async Task InitializeAppAsync()
    {
        LoadingMessage = "Connexion à la base de données...";
        await Task.Delay(1000);

        LoadingMessage = "Chargement de la liste des vols...";
        await Task.Delay(1000);

        LoadingMessage = "Préparation de l'interface...";
        await Task.Delay(100);

        CurrentPage = _dashboardVm;

        IsLoading = false;
    }

    public void Receive(DemandeModificationClientMessage message)
    {
        CurrentPage = _reservationVm;
    }

    //nav
    [RelayCommand]
    private void GoToDashboard() => CurrentPage = _dashboardVm;

    [RelayCommand]
    private void GoToOperation() => CurrentPage = _operationVm;

    [RelayCommand]
    private void GoToVols() => CurrentPage = _volsVm;

    [RelayCommand]
    private void GoToReservation() => CurrentPage = _reservationVm;

    [RelayCommand]
    private void GoToPassager() => CurrentPage = _passagerVm;
}