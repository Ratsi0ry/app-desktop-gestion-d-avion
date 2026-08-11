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

        CurrentPage = new DashboardViewModel();

        IsLoading = false;
    }

    // au clic du btn modifier
    public void Receive(DemandeModificationClientMessage message)
    {
        var client = message.Value;

        // Instanciation de ReservationViewModel pré-remplie
        var reservationVm = new ReservationViewModel
        {
            IdPasseport = client.IdPasseport,
            Nom = client.Nom,
            Prenom = client.Prenom,
            CategoriePersonne = client.Categorie,
            ClasseAvion = client.Classe,
            CompagnieAerienne = client.Compagnie,
            TypeVol = client.TypeVol,
            VilleDepart = client.Depart,
            VilleArrivee = client.Destination,
            DateVol = client.Date,
            HeureVol = client.Heure
        };


        if (string.IsNullOrWhiteSpace(client.Siege))
        {
            reservationVm.SiegeSelectionne = new Siege { Numero = client.Siege, EstSelectionne = true };
        }
        // redirection
        CurrentPage = reservationVm;
    }

    //nav
    [RelayCommand]
    private void GoToDashboard() => CurrentPage = new DashboardViewModel();

    [RelayCommand]
    private void GoToOperation() => CurrentPage = new OperationViewModel();

    [RelayCommand]
    private void GoToVols() => CurrentPage = new VolsViewModel();

    [RelayCommand]
    private void GoToReservation() => CurrentPage = new ReservationViewModel();

    [RelayCommand]
    private void GoToPassager() => CurrentPage = new PassagerViewModel();
}