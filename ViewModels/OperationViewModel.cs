
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

    Dictionary<string, Object>[] RegisteredPlane = new Dictionary<string, Object>[]
    {
        new Dictionary<string, Object>
        {
            { "Name", "flying_bitch" },
            { "Model", planeModel.Airbus350},
            { "Id", "p001" },
            { "Place_total", 250},
            { "PointA", "Tamatave" },
            { "PointB", "Antananarivo"},
            { "Classes", new Dictionary<string, Object>
                        {
                            { "economique", 180},
                            { "vip", 40 },
                            { "vvip", 30 }
                        }
            }
        },
        new Dictionary<string, Object>
        {
            { "Name", "fritz" },
            { "Model", planeModel.Boeign787Dreamliner},
            { "Id", "p002" },
            { "place_total", 300 },
            { "PointA", "Fianarantsoa"},
            { "PointB", "Antananarivo"},
            { "Classes", new Dictionary<string, Object>
                        {
                            { "economique", 230},
                            { "vip", 40 },
                            { "vvip", 30 }
                        }
            }
        }
    };
    
    public ObservableCollection<CardViewModel> PlaneList { get; set; }

    public OperationViewModel()
    {
        PlaneList = new ObservableCollection<CardViewModel>();
        for (int i = 0; i < RegisteredPlane.Length; i++)
        {
            PlaneList.Add(new CardViewModel(RegisteredPlane[i]["Name"].ToString(), RegisteredPlane[i]["Id"].ToString(), "plane", OnPlaneSelected));
        }
        ViewPlane = new PlaneStatusViewModel(true);
    }

    private void OnPlaneSelected(CardViewModel clickedCard)
    {
        SelectedPlaneName = clickedCard.ItemName;
        SelectedPlaneId = clickedCard.ItemId;
        string arrivee, depart;
        for (int i = 0; i < RegisteredPlane.Length; i++)
        {
            if (RegisteredPlane[i]["Name"].ToString() == SelectedPlaneName && RegisteredPlane[i]["Id"].ToString() == SelectedPlaneId)
            {
                planeModel model = planeModel.Airbus350;
                depart = RegisteredPlane[i]["PointA"].ToString();
                arrivee = RegisteredPlane[i]["PointB"].ToString();
                ViewPlane = new PlaneStatusViewModel(false, model ,SelectedPlaneName, SelectedPlaneId, depart, arrivee, "sora", DateTime.Now.ToString("yyyy-MM-dd"));
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
