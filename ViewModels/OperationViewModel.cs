
using CommunityToolkit.Mvvm.ComponentModel;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System;
using System.Numerics;

namespace Gestion_avion.ViewModels;

public partial class OperationViewModel: ViewModelBase
{
    [ObservableProperty]
    private DateTime? _selectedDate;

    [ObservableProperty]
    private PlaneStatusViewModel _viewPlane;

    [ObservableProperty]
    private string? _selectedPlaneName;
    
    [ObservableProperty]
    private string? _selectedPlaneId;
    
    class Plane
    {
        public string Name, Id, TotalPlace, PointA, PointB;
        public Plane(string name,string id, string places, string A, string B)
        {
            Name = name;
            Id = id;
            TotalPlace = places;
            PointA = A;
            PointB = B;
        }
    }

    List<Plane> RegisteredPlane = [
        new Plane("asterio", "p222", "222", "tana", "fianarantsoa"),
        new Plane("alaal", "p222", "222", "tana", "fianarantsoa"),
        new Plane("poopsocpa", "p222", "222", "tana", "fianarantsoa"),
        new Plane("bIAWUBh", "p222", "222", "tana", "fianarantsoa")
    ];

    public ObservableCollection<CardViewModel> PlaneList { get; set; }

    public OperationViewModel()
    {
        PlaneList = new ObservableCollection<CardViewModel>();
        foreach (Plane p in RegisteredPlane)
        {
            PlaneList.Add(new CardViewModel(p.Name, p.Id, "plane", OnPlaneSelected));
        }
        ViewPlane = new PlaneStatusViewModel(true);
    }

    private void OnPlaneSelected(CardViewModel clickedCard)
    {
        SelectedPlaneName = clickedCard.ItemName;
        SelectedPlaneId = clickedCard.ItemId;
        string arrivee, depart;
        foreach (Plane p in RegisteredPlane)
        {
            if (p.Name == SelectedPlaneName && p.Id == SelectedPlaneId)
            {
                depart = p.PointA;
                arrivee = p.PointB;
                ViewPlane = new PlaneStatusViewModel(false, SelectedPlaneName, SelectedPlaneId, depart, arrivee, "sora", DateTime.Now.ToString("yyyy-MM-dd"));
                break;
            } else
            {
                continue;
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
}
