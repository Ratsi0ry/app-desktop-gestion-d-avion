using CommunityToolkit.Mvvm.ComponentModel;
using Gestion_avion.Views;

namespace Gestion_avion.ViewModels;

public partial class ReservationViewModel: ViewModelBase
{
    [ObservableProperty]
    private string _villeDisponibles, _villeDepart, _villeArrivee, _dateVol, _heureVol, _nom, _prenom,
                    _idPasseport, _categoriePersonne, _classeAvion, _compagnieAerienne;

                    
    [ObservableProperty]
    private short _age, _nbPersonne, _sejour;

    //affichage du label sejour
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsSejourVisible))]
    private string _typeVol;

    public bool IsSejourVisible => TypeVol == "Aller-retour";

    
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
        IdPasseport = "";
        NbPersonne = 0;
        CategoriePersonne = "";
        ClasseAvion = "";
        TypeVol = "";
        CompagnieAerienne = "";
        Sejour = 0;
    }
}
