using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using back.Models;
using back.Data;
using Gestion_avion.state;

namespace Gestion_avion.ViewModels;

public partial class VolsViewModel : ViewModelBase
{
    private readonly AppState _appState;
    private readonly Contextedb _context;
    private CancellationTokenSource? _notifyCts;
    private Vol? _selectedVol;

    #region Propriétés Observable (Formulaire)

    [ObservableProperty]
    private DateTimeOffset? _currentView = DateTimeOffset.Now;

    [ObservableProperty]
    private string? _plane_;

    [ObservableProperty]
    private string? _d;

    [ObservableProperty]
    private string? _a;

    [ObservableProperty]
    private DateTimeOffset? _departureDate;

    [ObservableProperty]
    private TimeSpan? _departureTime;

    [ObservableProperty]
    private string _conf = "Confirmer l'ajout";

    [ObservableProperty]
    private bool _isEditing;

    [ObservableProperty]
    private bool _sure;

    #endregion

    #region Notifications UI

    [ObservableProperty]
    private bool _notify;

    [ObservableProperty]
    private string? _message;

    #endregion

    #region Collections UI

    [ObservableProperty]
    private ObservableCollection<string> _availablePorts = new();

    [ObservableProperty]
    private ObservableCollection<string> _availablePlanes = new();

    public ObservableCollection<FlightCardViewModel> FlightList { get; } = new();

    #endregion

    public VolsViewModel(Contextedb context, AppState appState)
    {
        _appState = appState;
        _context = context;
        _ = ChargerDonneesDepuisBDDAsync();
    }

    public VolsViewModel(AppState appState) : this(new Contextedb(), appState) { }

    private async Task ChargerDonneesDepuisBDDAsync()
    {
        try
        {
            var avions = await _context.Avion
                .AsNoTracking()
                .Select(a => a.nom_avion)
                .Where(n => n != null)
                .ToListAsync();

            AvailablePlanes = new ObservableCollection<string>(avions!);

            var departPorts = await _context.Trajet.Select(t => t.lieu_depart).ToListAsync();
            var destPorts = await _context.Trajet.Select(t => t.destination).ToListAsync();
            
            var ports = departPorts.Concat(destPorts)
                                   .Where(p => !string.IsNullOrEmpty(p))
                                   .Distinct();

            AvailablePorts = new ObservableCollection<string>(ports!);

            var vols = await _context.Vol
                .Include(v => v.Avion)
                .Include(v => v.Trajet)
                .AsNoTracking()
                .ToListAsync();

            RefreshUI(vols);
        }
        catch (Exception ex)
        {
            await ShowNotificationAsync($"Erreur BDD : {ex.Message}");
        }
    }

    private void RefreshUI(IEnumerable<Vol> vols)
    {
        FlightList.Clear();
        foreach (var v in vols)
        {
            DateTime.TryParse(v.fk_date_depart, out DateTime dateParsed);

            FlightList.Add(new FlightCardViewModel(
                v.Avion?.nom_avion ?? v.fk_id_avion ?? "Inconnu",
                v.Avion?.Compagnie?.nom_compagnie ?? "Compagnie Inconnu",
                v.Trajet?.lieu_depart ?? "Inconnu",
                v.Trajet?.destination ?? "Inconnu",
                dateParsed,
                OnDelay,
                OnDelete
            )
            {
                Tag = v.id_vol
            });
        }
    }

    private async Task ShowNotificationAsync(string text)
    {
        _notifyCts?.Cancel();
        _notifyCts = new CancellationTokenSource();

        Message = text;
        Notify = true;

        try
        {
            await Task.Delay(2500, _notifyCts.Token);
            Notify = false;
            Message = string.Empty;
        }
        catch (TaskCanceledException) { }
    }

    #region Handlers Cartes

    private async void OnDelay(FlightCardViewModel card)
    {
        string? idVol = card.Tag as string;
        if (string.IsNullOrEmpty(idVol)) return;

        _selectedVol = await _context.Vol
            .Include(v => v.Avion)
            .Include(v => v.Trajet)
            .FirstOrDefaultAsync(v => v.id_vol == idVol);

        if (_selectedVol == null) return;

        Plane_ = _selectedVol.Avion?.nom_avion;
        D = _selectedVol.Trajet?.lieu_depart;
        A = _selectedVol.Trajet?.destination;

        if (DateTime.TryParse(_selectedVol.fk_date_depart, out DateTime dt))
        {
            DepartureDate = dt.Date;
            DepartureTime = dt.TimeOfDay;
        }

        IsEditing = true;
        Conf = "Confirmer les modifications";
    }

    private void OnDelete(FlightCardViewModel card)
    {
        string? idVol = card.Tag as string;
        if (string.IsNullOrEmpty(idVol)) return;

        _selectedVol = _context.Vol.FirstOrDefault(v => v.id_vol == idVol);
        Sure = true;
    }

    #endregion

    #region Commandes MVVM

    [RelayCommand]
    private void Reset()
    {
        Plane_ = null;
        D = null;
        A = null;
        DepartureDate = null;
        DepartureTime = null;

        IsEditing = false;
        Sure = false;
        Conf = "Confirmer l'ajout";
        _selectedVol = null;
    }

    [RelayCommand]
    private async Task ConfirmAsync()
    {
        if (string.IsNullOrWhiteSpace(Plane_) || string.IsNullOrWhiteSpace(D) || string.IsNullOrWhiteSpace(A))
        {
            await ShowNotificationAsync("Champs incomplets !");
            return;
        }

        try
        {
            var avion = await _context.Avion.FirstOrDefaultAsync(a => a.nom_avion == Plane_);
            if (avion == null)
            {
                if(_appState.currentCompanie == null) {
                    throw new Exception("Compagnie not found");
                }
                avion = new Avion { id_avion = $"AV-{Guid.NewGuid().ToString()[..6]}", nom_avion = Plane_};
                _context.Avion.Add(avion);
            }

            var trajet = await _context.Trajet.FirstOrDefaultAsync(t => t.lieu_depart == D && t.destination == A);
            if (trajet == null)
            {
                trajet = new Trajet { id_trajet = $"TRJ-{Guid.NewGuid().ToString()[..6]}", lieu_depart = D, destination = A };
                _context.Trajet.Add(trajet);
            }

            DateTime fullDate = (DepartureDate?.Date ?? DateTime.Today).Add(DepartureTime ?? TimeSpan.Zero);
            string dateString = fullDate.ToString("yyyy-MM-dd HH:mm:ss");

            var dateVolExist = await _context.Date_vol.FirstOrDefaultAsync(d => d.date_depart == dateString);
            if (dateVolExist == null)
            {
                dateVolExist = new Date_vol { date_depart = dateString };
                _context.Date_vol.Add(dateVolExist);
            }

            if (!IsEditing)
            {
                var newVol = new Vol
                {
                    id_vol = $"VOL-{Guid.NewGuid().ToString()[..6]}",
                    status_vol = "Prevu",
                    fk_date_depart = dateString,
                    fk_id_avion = avion.id_avion,
                    fk_id_trajet = trajet.id_trajet,
                    Avion = avion,
                    Trajet = trajet,
                    Date_vol = dateVolExist
                };

                _context.Vol.Add(newVol);
                await _context.SaveChangesAsync();
                await ShowNotificationAsync("Vol inséré en BDD");
            }
            else
            {
                if (_selectedVol != null)
                {
                    _selectedVol.fk_date_depart = dateString;
                    _selectedVol.fk_id_avion = avion.id_avion;
                    _selectedVol.fk_id_trajet = trajet.id_trajet;

                    _context.Vol.Update(_selectedVol);
                    await _context.SaveChangesAsync();
                    await ShowNotificationAsync("Vol modifié en BDD");
                }
            }

            await ChargerDonneesDepuisBDDAsync();
            Reset();
        }
        catch (Exception ex)
        {
            await ShowNotificationAsync($"Erreur : {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task DelConfirmAsync()
    {
        if (_selectedVol != null)
        {
            try
            {
                _context.Vol.Remove(_selectedVol);
                await _context.SaveChangesAsync();

                await ShowNotificationAsync("Suppression BDD réussie");
                await ChargerDonneesDepuisBDDAsync();
            }
            catch (Exception ex)
            {
                await ShowNotificationAsync($"Erreur suppression : {ex.Message}");
            }
        }

        Reset();
    }

    #endregion
}