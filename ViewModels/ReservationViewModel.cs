using CommunityToolkit.Mvvm.ComponentModel;
using Gestion_avion.Views;

namespace Gestion_avion.ViewModels;

public partial class ReservationViewModel: ViewModelBase
{
    [ObservableProperty]
    private string _villeDisponibles, _villeDepart, _villeArrivee, _dateVol, _heureVol, _nom, _prenom;

    [ObservableProperty]
    private short _age;
    public ReservationViewModel()
    {
        VilleDepart = "";
        VilleDisponibles = "";
        VilleArrivee = "";
        DateVol = "";
        HeureVol = "";
        Nom = "";
        Prenom = "";
        Age = 0;
    }
}
