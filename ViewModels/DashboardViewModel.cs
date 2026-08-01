using CommunityToolkit.Mvvm.ComponentModel;

namespace Gestion_avion.ViewModels;

public partial class DashboardViewModel : ViewModelBase
{
    [ObservableProperty]
    private int _nbVolsJournee;

    [ObservableProperty]
    private int _nbVolsEnCours;

    [ObservableProperty]
    private int _nbVolsEffectues;

    [ObservableProperty]
    private int _nbVolsAnnuler;

    [ObservableProperty]
    private string _meteoDuJour;

    public DashboardViewModel()
    {
        NbVolsJournee = 30;
        NbVolsEnCours = 7;
        NbVolsEffectues = 3;
        NbVolsAnnuler = 0;
        MeteoDuJour = "Ciel degagé 24° C ";
    }
}