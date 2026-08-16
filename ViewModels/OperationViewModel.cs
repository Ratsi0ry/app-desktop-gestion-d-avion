
using CommunityToolkit.Mvvm.ComponentModel;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Numerics;
using Gestion_avion.state;
using CommunityToolkit.Mvvm.Input;
using back.Models;

namespace Gestion_avion.ViewModels;

public partial class OperationViewModel: ViewModelBase
{
    private readonly AppState _appState;

    private readonly Avionfunc _avionfunc = new();
    private readonly Trajetfunc _trajetfunc = new();
    private readonly Volfunc _volfunc = new();
    private readonly Compagniefunc _compagniefunc = new();
    private readonly Caracfunc _caracfunc = new();
    private readonly Statut_avionfunc _statut_avionfunc = new();

    private List<Avion> RegisteredPlane = new();

    [ObservableProperty]
    private DateTime? _selectedDate;

    [ObservableProperty]
    private PlaneStatusViewModel _viewPlane;

    [ObservableProperty]
    private string? _selectedPlaneName;
    
    [ObservableProperty]
    private string? _selectedPlaneId;

    [ObservableProperty]
    private string? _upName, _upCompanie, _upPointA, _upPointB;

    [ObservableProperty]
    private bool _confirmDelete = false, _isCreating = false;

    public ObservableCollection<CardViewModel> PlaneList { get; set; }
    public ObservableCollection<TextBlock> Classlist {get; set;}

    public OperationViewModel(AppState appState)
    {
        _appState = appState;
        PlaneList = new ObservableCollection<CardViewModel>();
        Classlist = new ObservableCollection<TextBlock>();
        Classlist.Add(new TextBlock {Text = ""});
        ViewPlane = new PlaneStatusViewModel(true, Classlist, delete: OnPlaneDelete);
        _ = InitializeAsync();

        Console.WriteLine(_appState.currentCompanie?.id_compagnie ?? "Hello");
    }

    private async Task InitializeAsync()
    {
        await RefreshPlaneListAsync();
    }

    private async void OnPlaneSelected(CardViewModel clickedCard)
    {
        IsCreating = false;
        SelectedPlaneName = clickedCard.ItemName;
        SelectedPlaneId = clickedCard.ItemId;

        foreach (Avion av in RegisteredPlane)
        {
            if (av.nom_avion == SelectedPlaneName && av.id_avion == SelectedPlaneId)
            {
                string depart = "";
                string arrivee = "";

                var vols = await _volfunc.RechercheVol(v => v.fk_id_avion == av.id_avion);
                if (vols.Count > 0)
                {
                    depart = vols[0].Trajet.lieu_depart;
                    arrivee = vols[0].Trajet.destination;
                }

                string companyName = "";
                var compagnies = await _compagniefunc.RechercheCompagnie(c => c.id_compagnie == av.fk_id_compagnie);
                if (compagnies.Count > 0)
                {
                    companyName = compagnies[0].nom_compagnie;
                }

                UpName = SelectedPlaneName;
                UpCompanie = companyName;
                UpPointA = depart;
                UpPointB = arrivee;
                ViewPlane = new PlaneStatusViewModel(false, Classlist, "", SelectedPlaneName, SelectedPlaneId, depart, arrivee, companyName, DateTime.Now.ToString("yyyy-MM-dd"), delete: OnPlaneDelete);
                break;
            }
        }
    }

    private async Task RefreshPlaneListAsync()
    {
        var avions = await _avionfunc.ListerAvions();

        var actifs = new List<Avion>();
        foreach (var av in avions)
        {
            var statuts = await _caracfunc.RechercheAvionStatut(c => c.fk_id_avion == av.id_avion);
            bool estInactif = false;
            foreach (var s in statuts)
            {
                var stAvion = await _statut_avionfunc.RechercheStatutAvion(st => st.code_statut == s.fk_code_statut);
                if (stAvion.Count > 0 && stAvion[0].libelle_statut == "Inactif")
                {
                    estInactif = true;
                    break;
                }
            }

            if (!estInactif)
            {
                actifs.Add(av);
            }
        }

        RegisteredPlane = actifs;

        PlaneList.Clear();
        foreach (Avion av in RegisteredPlane)
        {
            PlaneList.Add(new CardViewModel(av.nom_avion, av.id_avion, "", "", "plane", OnPlaneSelected, _appState));
        }

        ViewPlane = new PlaneStatusViewModel(true, Classlist, delete: OnPlaneDelete);
    }

    private void OnPlaneDelete(PlaneStatusViewModel vm)
    {
        foreach (Avion av in RegisteredPlane)
        {
            if (av.nom_avion == vm.PlaneName && av.id_avion == vm.PlaneId)
            {
                SelectedPlaneName = vm.PlaneName;
                SelectedPlaneId = vm.PlaneId;
                ConfirmDelete = true;
                break;
            }
        }
    }

    partial void OnSelectedDateChanged(DateTime? value)
    {
        if (value.HasValue)
        {
            string formattedDate = value.Value.ToString("yyyy-MM-dd");
            ViewPlane.ViewDate = formattedDate;
        }
    }

    [RelayCommand]
    private void CreatePlane()
    {
        IsCreating = true;
    }

    [RelayCommand]
    private async Task ConfirmDelete_()
    {
        if (!string.IsNullOrEmpty(SelectedPlaneId))
        {
            var statuts = await _statut_avionfunc.RechercheStatutAvion(s => s.libelle_statut == "Inactif");
            Statut_avion statutInactif;
            if (statuts.Count > 0)
            {
                statutInactif = statuts[0];
            }
            else
            {
                statutInactif = new Statut_avion
                {
                    code_statut = "ST" + DateTime.Now.Ticks,
                    libelle_statut = "Inactif"
                };
                statutInactif = await _statut_avionfunc.AjouterStatutAvion(statutInactif);
            }

            var anciens = await _caracfunc.RechercheAvionStatut(c => c.fk_id_avion == SelectedPlaneId);
            foreach (var anc in anciens)
            {
                await _caracfunc.SupprimerAvionStatut(anc.fk_id_avion, anc.fk_code_statut);
            }

            var nouveau = new Caracteriser
            {
                fk_id_avion = SelectedPlaneId,
                fk_code_statut = statutInactif.code_statut
            };
            await _caracfunc.AjouterAvionStatut(nouveau);
        }

        await RefreshPlaneListAsync();
        ConfirmDelete = false;
    }

    [RelayCommand]
    private async Task Cancel()
    {
        ConfirmDelete = false;
        IsCreating = false;
        await RefreshPlaneListAsync();
    }

    [RelayCommand]
    private async Task ConfirmCreate()
    {
        if (!string.IsNullOrEmpty(UpName) && _appState.currentCompanie != null)
        {
            var avion = new Avion
            {
                id_avion = "AV" + DateTime.Now.Ticks,
                nom_avion = UpName,
                fk_id_compagnie = _appState.currentCompanie.id_compagnie
            };
            await _avionfunc.AjouterAvion(avion);

            await RefreshPlaneListAsync();
            IsCreating = false;
        }
    }

    [RelayCommand]
    private async Task ConfirmModify()
    {
        if (string.IsNullOrEmpty(SelectedPlaneId)) return;

        Avion? avionActuel = null;
        foreach (var av in RegisteredPlane)
        {
            if (av.id_avion == SelectedPlaneId)
            {
                avionActuel = av;
                break;
            }
        }
        if (avionActuel == null) return;

        string nom = !string.IsNullOrEmpty(UpName) ? UpName : avionActuel.nom_avion;
        string fkCompagnie = avionActuel.fk_id_compagnie;

        if (!string.IsNullOrEmpty(UpCompanie))
        {
            var compagnies = await _compagniefunc.RechercheCompagnie(c => c.nom_compagnie == UpCompanie);
            if (compagnies.Count > 0)
            {
                fkCompagnie = compagnies[0].id_compagnie;
            }
        }

        var avionModifie = new Avion
        {
            id_avion = SelectedPlaneId,
            nom_avion = nom,
            fk_id_compagnie = fkCompagnie
        };
        await _avionfunc.ModifierAvion(avionModifie);

        SelectedPlaneName = nom;

        await RefreshPlaneListAsync();

        string depart = "";
        string arrivee = "";
        var vols = await _volfunc.RechercheVol(v => v.fk_id_avion == SelectedPlaneId);
        if (vols.Count > 0)
        {
            depart = vols[0].Trajet.lieu_depart;
            arrivee = vols[0].Trajet.destination;
        }

        var compagnieFinale = await _compagniefunc.RechercheCompagnie(c => c.id_compagnie == fkCompagnie);
        string companyName = compagnieFinale.Count > 0 ? compagnieFinale[0].nom_compagnie : "";

        ViewPlane = new PlaneStatusViewModel(false, Classlist, "", nom, SelectedPlaneId, depart, arrivee, companyName, DateTime.Now.ToString("yyyy-MM-dd"), delete: OnPlaneDelete);
    }
}
