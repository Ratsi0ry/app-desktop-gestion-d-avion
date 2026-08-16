using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System;
using System.Numerics;
using System.Threading.Tasks;
using Avalonia.Controls.Notifications;
using Tmds.DBus.Protocol;
using back.Models;
using Gestion_avion.state;

namespace Gestion_avion.ViewModels;

public partial class VolsViewModel : ViewModelBase
{

    private readonly AppState _appState;

    [ObservableProperty]
    private bool _notify = false;

    [ObservableProperty]
    private string? _plane_, _d, _a, _conf, _message;

    [ObservableProperty]
    private bool _sure;

    [ObservableProperty]
    private DateTimeOffset? _departureDate, _currentView;

    [ObservableProperty]
    private TimeSpan? _departureTime;

    private readonly Trajetfunc _trajetfunc = new();
    private readonly Avionfunc _avionfunc = new();



    public ObservableCollection<FlightCardViewModel> FlightList{get; set;}
    
    private Vol? SelectedVol;
    private DateTime dt;

    private readonly Volfunc _volfunc = new();
    private readonly Compagniefunc _compagniefunc = new();
    private readonly Date_volfunc _date_volfunc = new();

    private List<Vol> allVols = new();


    private bool IsEditing = false;

    [ObservableProperty]
    private ObservableCollection<string> availablePorts = new();

    [ObservableProperty]
    private ObservableCollection<string> availablePlanes = new();

    private async Task LoadAvailablePortsAsync()
    {
        var trajets = await _trajetfunc.ListerTrajet();

        var villes = new List<string>();
        foreach (var t in trajets)
        {
            if (!villes.Contains(t.lieu_depart))
                villes.Add(t.lieu_depart);

            if (!villes.Contains(t.destination))
                villes.Add(t.destination);
        }

        AvailablePorts = new ObservableCollection<string>(villes);
    }


    // CHARGEMENT DES AVIONS
    private async Task LoadAvailablePlanesAsync()
    {
        if (_appState.currentCompanie == null)
        {
            AvailablePlanes = new ObservableCollection<string>();
            return;
        }

        string companieId = _appState.currentCompanie.id_compagnie;
        var avions = await _avionfunc.RechercheAvion(av => av.fk_id_compagnie == companieId);

        var noms = new List<string>();
        foreach (var av in avions)
        {
            noms.Add(av.nom_avion);
        }

        AvailablePlanes = new ObservableCollection<string>(noms);
    }


    //REFRESH CHARGEMENT DES AVIONS
    public async Task RefreshAvailablePlanesAsync()
    {
        await LoadAvailablePlanesAsync();
    }

    private async Task LoadVolsAsync()
    {
        if (_appState.currentCompanie == null)
        {
            allVols = new List<Vol>();
        }
        else
        {
            string companieId = _appState.currentCompanie.id_compagnie;

            //allVols = await _volfunc.ListerVol();
            allVols = await _volfunc.RechercheVol(v => v.Avion.fk_id_compagnie == companieId);
        }
        await RefreshAsync(allVols);
    }

    private async Task RefreshAsync(List<Vol> vols)
    {
        FlightList.Clear();
        foreach (Vol v in vols)
        {

            if (v.status_vol == "Annule") continue;
            
            string company = "";
            var compagnies = await _compagniefunc.RechercheCompagnie(c => c.id_compagnie == v.Avion.fk_id_compagnie);
            if (compagnies.Count > 0)
            {
                company = compagnies[0].nom_compagnie;
            }

            DateTime depart = DateTime.TryParse(v.Date_vol.date_depart, out var d) ? d : DateTime.Now;

            FlightList.Add(new FlightCardViewModel(
                v.Avion.nom_avion,
                company,
                v.Trajet.lieu_depart,
                v.Trajet.destination,
                depart,
                OnDelay,
                OnDelete
            ));
        }
    }


    public VolsViewModel(AppState appState)
    {
        _appState = appState;
        FlightList = new ObservableCollection<FlightCardViewModel>();
        Conf = "confirmer l'ajout";
        Sure = false;
        _ = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        await LoadAvailablePortsAsync();
        await LoadAvailablePlanesAsync();
        await LoadVolsAsync();
    }

    
    private void OnDelay(FlightCardViewModel currflight)
    {
        Plane_ = currflight.Plane;
        foreach (Vol v in allVols)
        {
            if (currflight.Plane == v.Avion.nom_avion &&
                currflight.PortA == v.Trajet.lieu_depart &&
                currflight.PortB == v.Trajet.destination)
            {
                DepartureTime = currflight.Depart.TimeOfDay;
                DepartureDate = (DateTimeOffset)currflight.Depart;
                D = v.Trajet.lieu_depart;
                A = v.Trajet.destination;
                Conf = "Confirmer les modifications";
                SelectedVol = v;
            }
        }
        IsEditing = true;
    }

    private void OnDelete(FlightCardViewModel currFlight)
    {
        Sure = true;
        IsEditing = false;
        foreach (Vol v in allVols)
        {
            if (currFlight.Plane == v.Avion.nom_avion &&
                currFlight.PortA == v.Trajet.lieu_depart &&
                currFlight.PortB == v.Trajet.destination &&
                currFlight.Depart == (DateTime.TryParse(v.Date_vol.date_depart, out var dd) ? dd : DateTime.MinValue))
            {
                SelectedVol = v;
            }
        }
    }

    
    private async Task NotificationAsync(string mess)
    {
        Notify = true;
        Message = mess;
        await Task.Delay(2000);
        Message = "";
        Notify = false;
    }

    [RelayCommand]
    private void Reset()
    {
        A = null;
        Plane_ = null;
        D = null;
        DepartureDate = null;
        DepartureTime = null;
        Conf = "confirmer l'ajout";
        Sure = false;
        IsEditing = false;
    }

    [RelayCommand]
    private async Task Confirm()
    {
        if (DepartureDate.HasValue)
        {
            var date = DepartureDate.Value.Date;
            var time = DepartureTime ?? TimeSpan.Zero;
            dt = date.Add(time);
        }
        else
        {
            dt = DateTime.Now;
        }

        if (!IsEditing)
        {
            if (Plane_ != null && A != null && D != null)
            {
                var avions = await _avionfunc.RechercheAvion(av => av.nom_avion == Plane_);
                if (avions.Count > 0)
                {
                    var avion = avions[0];

                    var trajets = await _trajetfunc.RechercheTrajet(t => t.lieu_depart == D && t.destination == A);
                    Trajet trajet;
                    if (trajets.Count > 0)
                    {
                        trajet = trajets[0];
                    }
                    else
                    {
                        trajet = new Trajet
                        {
                            id_trajet = "TRJ" + DateTime.Now.Ticks,
                            lieu_depart = D,
                            destination = A
                        };
                        trajet = await _trajetfunc.AjouterTrajet(trajet);
                    }

                    string dateStr = dt.ToString("yyyy-MM-dd HH:mm:ss");
                    var dateVols = await _date_volfunc.RechercheDateVol(d => d.date_depart == dateStr);
                    Date_vol date_vol;
                    if (dateVols.Count > 0)
                    {
                        date_vol = dateVols[0];
                    }
                    else
                    {
                        date_vol = new Date_vol { date_depart = dateStr };
                        date_vol = await _date_volfunc.AjouterDateVol(date_vol);
                    }

                    var vol = new Vol
                    {
                        id_vol = "V" + DateTime.Now.Ticks,
                        status_vol = "Prevu",
                        fk_date_depart = date_vol.date_depart,
                        fk_id_trajet = trajet.id_trajet,
                        fk_id_avion = avion.id_avion
                    };
                    await _volfunc.AjouterVol(vol);
                }
            }
            await NotificationAsync("insertion effectuee");
        }
        else
        {
            if (SelectedVol != null && DepartureDate != null && DepartureTime != null && A != null && D != null)
            {
                var trajets = await _trajetfunc.RechercheTrajet(t => t.lieu_depart == D && t.destination == A);
                Trajet trajet;
                if (trajets.Count > 0)
                {
                    trajet = trajets[0];
                }
                else
                {
                    trajet = new Trajet
                    {
                        id_trajet = "TRJ" + DateTime.Now.Ticks,
                        lieu_depart = D,
                        destination = A
                    };
                    trajet = await _trajetfunc.AjouterTrajet(trajet);
                }

                string dateStr = dt.ToString("yyyy-MM-dd HH:mm:ss");
                var dateVols = await _date_volfunc.RechercheDateVol(d => d.date_depart == dateStr);

                Date_vol date_vol;
                if (dateVols.Count > 0)
                {
                    date_vol = dateVols[0];
                }
                else
                {
                    date_vol = new Date_vol { date_depart = dateStr };
                    date_vol = await _date_volfunc.AjouterDateVol(date_vol);
                }

                SelectedVol.fk_date_depart = date_vol.date_depart;
                SelectedVol.fk_id_trajet = trajet.id_trajet;

                await _volfunc.ModifierVol(SelectedVol);

                await NotificationAsync("modification effectuee");
            }
        }
        await LoadVolsAsync();
        Reset();
    }

    [RelayCommand]
    private async Task DelConfirm()
    {
        if (SelectedVol != null)
        {
            SelectedVol.status_vol = "Annule";
            await _volfunc.ModifierVol(SelectedVol);
        }

        await LoadVolsAsync();
        Reset();
        await NotificationAsync("annulation reussie");
    }
}
