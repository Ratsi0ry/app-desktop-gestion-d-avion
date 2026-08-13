using System;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using back.Models;

namespace Gestion_avion.ViewModels;

public partial class DashboardViewModel : ViewModelBase
{
    // Initialise les fonctions pour les vols
    private Volfunc _volfunc = new Volfunc();

    [ObservableProperty]
    private int _nbVolsJournee, _nbVolsEnCours, _nbVolsEffectues, _nbVolsAnnuler;

    [ObservableProperty]
    private ObservableCollection<Vol> _vols = new ObservableCollection<Vol>();

    [ObservableProperty]
    private string _meteoDuJour, _dateDuJour;

    public DashboardViewModel()
    {
        DateDuJour = "";

        NbVolsEnCours = 7;
        NbVolsEffectues = 3;
        NbVolsAnnuler = 0;
        MeteoDuJour = "Ciel degagé 24° C ";
    }

    public async Task OnLoaded()
    {
        GetTodaysDate();
        await ListsVols();
    }


    ////////////////////////////////////////////////////////////////////////////////////
    /**
     * 
     *      calcule la date du jour
     * 
     * */
    private void GetTodaysDate()
    {
        CultureInfo fr = new CultureInfo("fr-FR");
        DateTime now = DateTime.Now;
        String jour = now.ToString("dddd", fr);
        jour = char.ToUpper(jour[0]) + jour.Substring(1);
        String mois = now.ToString("MMM", fr);
        mois = char.ToUpper(mois[0]) + mois.Substring(1);
        String date = now.ToString("dd");
        String annee = now.ToString("yyyy");

        DateDuJour = $"{jour}, {date} {mois} {annee}";
    }

    /**
     * 
     *      Lister tous les vols dans la base de donnee
     * 
     * */
     public async Task ListsVols()
     {
        string aujourdhui = DateTime.Today.ToString("yyyy-MM-dd");

        var listevols = await _volfunc.ListerVol();

        var volDaujourdhui = listevols
            .Where(v => v.Date_vol != null && v.Date_vol.date_depart.Contains(aujourdhui))
            .ToList();

        Vols = new ObservableCollection<Vol>(volDaujourdhui);
        NbVolsJournee = Vols.Count;
     }
}