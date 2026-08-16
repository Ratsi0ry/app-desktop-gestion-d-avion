using CommunityToolkit.Mvvm.ComponentModel;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System;
using System.Numerics;

namespace Gestion_avion.ViewModels;

public partial class PlaneStatusViewModel : ViewModelBase
{
    [ObservableProperty]
    public ObservableCollection<TextBlock>? _planeClasses;
    [ObservableProperty]
    public string? _planeName, _planeId, _places, _owner, _viewDate, _pointA, _pointB;

    [ObservableProperty]
    public bool _isDefault = false, _isNew = false, _isStatus = true;
    private readonly Action<PlaneStatusViewModel>? _delete;

    public PlaneStatusViewModel(bool _default_, 
                                ObservableCollection<TextBlock>? classes,
                                string places = "",
                                string name = "", 
                                string id = "", 
                                string? depart = "", 
                                string? arrivee = "",
                                string Owner = "", 
                                string view = "", 
                                bool new_ = false,
                                Action<PlaneStatusViewModel>? delete = null)
    {
        _planeName = name;
        _places = places;
        _planeId = id;
        _owner = Owner;
        _pointA = depart;
        _pointB = arrivee;
        _isDefault = _default_;
        _viewDate = view;
        _isNew = new_;
        _isStatus = !_isDefault && !_isNew;
        _planeClasses = classes;
        _delete = delete;
    }

    [CommunityToolkit.Mvvm.Input.RelayCommand]
    public void Delete()
    {
        _delete?.Invoke(this);
    }
    
}