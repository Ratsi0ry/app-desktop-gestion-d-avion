using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System;

namespace Gestion_avion.ViewModels;

public partial class VolsViewModel : ViewModelBase
{
    [ObservableProperty]
    private string? _plane_, _d, _a, _conf;

    [ObservableProperty]
    private bool _sure;

    [ObservableProperty]
    private DateTimeOffset? _departureDate, _currentView;

    [ObservableProperty]
    private TimeSpan? _departureTime;

    [ObservableProperty]
    private ObservableCollection<string> availablePorts = new ()
    {
        "Antananarivo",
        "Toamasina",
        "Allemagne",
        "France",
        "Fianarantsoa",
        "Sambava"
    };

    [ObservableProperty]
    private ObservableCollection<string> availablePlanes = new()
    {
        "A1",
        "A2",
        "A3",
        "A4",
        "A5"
    };

    class Flight
    {
        public string Plane, Company, PortA, PortB;
        public DateTime Departure;

        public Flight(string plane, string company, DateTime departure, string A, string B)
        {
            Plane = plane;
            Company = company;
            Departure = departure;
            PortA = A;
            PortB = B;
        }
    }

    List<Flight> allFlights = [
      new Flight("A1", "azaa", DateTime.Now,"Antananarivo", "Toamasina"),
      new Flight("A2", "azaa", DateTime.Now,"France", "Toamasina"),
      new Flight("A3", "azaa", DateTime.Now,"Antananarivo", "Allemagne"),
      new Flight("A4", "azaa", DateTime.Now,"Sambava", "Toamasina"),
      new Flight("A5", "azaa", DateTime.Now,"Fianarantsoa", "Toamasina"),
    ];

    public ObservableCollection<FlightCardViewModel> FlightList{get; set;}
    public VolsViewModel()
    {
        FlightList = new ObservableCollection<FlightCardViewModel>();
        Conf = "confirmer l'ajout";
        Sure = false;
        CurrentView = (DateTimeOffset)DateTime.Now;
        foreach (Flight f in allFlights)
        {
            FlightList.Add(new FlightCardViewModel( f.Plane,
                                                    f.Company,
                                                    f.PortA,
                                                    f.PortB,
                                                    f.Departure.ToString("HH:mm"),
                                                    OnDelay,
                                                    OnDelete
                                                    ));
        }
    }

    private void OnDelay(FlightCardViewModel currflight)
    {
        Plane_ = currflight.Plane;
        foreach (Flight f in allFlights)
        {
            if (currflight.Plane == f.Plane)
            {
                DepartureTime = f.Departure.TimeOfDay;
                DepartureDate = (DateTimeOffset)f.Departure;
                D = f.PortA;
                A = f.PortB;
                Conf = "Confirmer les modifications";
            }
        }
    }

    private void OnDelete(FlightCardViewModel currFlight)
    {
        Sure = true;
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
    }

}
