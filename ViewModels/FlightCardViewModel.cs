using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Gestion_avion.Views.Cards;
using System.Collections.Generic;
using System;
namespace Gestion_avion.ViewModels;

public partial class FlightCardViewModel : ViewModelBase
{
    [ObservableProperty]
    public string _plane, _company, _portA, _portB, _date_depart;

    [ObservableProperty]
    private DateTime _depart;

    public object? Tag { get; set; }

    private readonly Action<FlightCardViewModel> _delayed;
    private readonly Action<FlightCardViewModel> _delete;
    public FlightCardViewModel(string plane, string company, string portA, string portB, DateTime depart, Action<FlightCardViewModel> delay, Action<FlightCardViewModel> remove)
    {
        _plane = plane;
        _company = company;
        _portA = portA;
        _portB = portB;
        _depart = depart;
        _date_depart = _depart.ToString("yyyy-MM-dd");
        _delayed = delay;
        _delete = remove;
    }

    [RelayCommand]
    public void Delay()
    {
        _delayed?.Invoke(this);
    }

    [RelayCommand]
    public void Delete()
    {
        _delete?.Invoke(this);
    }
}