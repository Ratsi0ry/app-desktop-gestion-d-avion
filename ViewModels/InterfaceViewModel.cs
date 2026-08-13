using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Gestion_avion.Messages;

namespace Gestion_avion.ViewModels;

public partial class InterfaceViewModel : ViewModelBase, IRecipient<DemandeModificationClientMessage>
{
     //initialisation
    [ObservableProperty]
    private ViewModelBase? _currentPage;

    //navigation entre les pages
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

     // au clic du btn modifier
    public void Receive(DemandeModificationClientMessage message)
    {
        var client = message.Value;

        // Instanciation de ReservationViewModel pré-remplie
        ReservationViewModel reservationVm = new ReservationViewModel
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
    public InterfaceViewModel()
    {
        WeakReferenceMessenger.Default.Register(this);
        CurrentPage = new DashboardViewModel();
    }
}