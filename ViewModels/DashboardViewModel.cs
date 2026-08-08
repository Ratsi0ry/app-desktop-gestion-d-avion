using CommunityToolkit.Mvvm.ComponentModel;

namespace Gestion_avion.ViewModels;

public partial class DashboardViewModel : ViewModelBase
{
    [ObservableProperty]
    private int _nbVolsJournee, _nbVolsEnCours, _nbVolsEffectues, _nbVolsAnnuler;


    [ObservableProperty]
    private string _meteoDuJour, _dateDuJour;

    public DashboardViewModel()
    {
        NbVolsJournee = 30;
        NbVolsEnCours = 7;
        NbVolsEffectues = 3;
        NbVolsAnnuler = 0;
        MeteoDuJour = "Ciel degagé 24° C ";
        DateDuJour = "01/08/26";
    }
}