
using System;
using Avalonia.Media.Imaging;
using Avalonia.Platform;               
using CommunityToolkit.Mvvm.ComponentModel;

namespace Gestion_avion.ViewModels;

public partial class PlaneStatusViewModel : ViewModelBase
{
    [ObservableProperty]
    public Bitmap? _modelImage;
    [ObservableProperty]
    public string? _planeName, _planeId, _owner, _viewDate, _pointA, _pointB;

    [ObservableProperty]
    public bool _isDefault = false;

    public PlaneStatusViewModel(bool _default_, string name = "", string id = "", string? depart = "", string? arrivee = "",string Owner = "", string view = "")
    {
        _planeName = name;
        _planeId = id;
        _owner = Owner;
        _pointA = depart;
        _pointB = arrivee;
        _isDefault = _default_;
        _viewDate = view;
        
    }
    
}