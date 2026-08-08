
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Gestion_avion.Views.Cards;
using System.Collections.Generic;
using System;
namespace Gestion_avion.ViewModels;

public enum planeModel
{
    Boeign787Dreamliner,
    Airbus350,
    Boeing737Max,
    AirbusA230Neo,
    AirbusA320
}

public partial class PlaneStatusViewModel : ViewModelBase
{
    public string ImageSourcePath {get;}

    [ObservableProperty]
    public string? _planeName, _planeId, _owner, _viewDate, _pointA, _pointB;

    [ObservableProperty]
    public bool _isDefault = false;

    public PlaneStatusViewModel(bool _default_, planeModel model = planeModel.AirbusA320 ,string name = "", string id = "", string? depart = "", string? arrivee = "",string Owner = "", string view = "")
    {
        _planeName = name;
        _planeId = id;
        _owner = Owner;
        _pointA = depart;
        _pointB = arrivee;
        _isDefault = _default_;
        _viewDate = view;
        ImageSourcePath = model switch
        {
            planeModel.Airbus350 => "avares://Gestion_avion/Assets/Airbus350.png",
            planeModel.AirbusA230Neo => "avares://Gestion_avion/Assets/Airbus320.png",
            planeModel.AirbusA320 => "avares://Gestion_avion/Assets/AirbusA350.png",
            planeModel.Boeign787Dreamliner => "avares://Gestion_avion/Assets/Boeing787Dreamliner.jpg",
            planeModel.Boeing737Max => "avares://Gestion_avion/Assets/Boeing737Max.png",
            _ => "avares://Gestion_avion/Assets/plane-profile-r.png"
        };
    }
    
}