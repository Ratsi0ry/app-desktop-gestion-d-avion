using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System;
using System.Numerics;
using System.Threading.Tasks;
using Avalonia.Controls.Notifications;
using Tmds.DBus.Protocol;

namespace Gestion_avion.ViewModels;

public partial class VolsViewModel : ViewModelBase
{
    private Flight? SelectedFlight;
    private DateTime dt;

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

    private bool IsEditing = false;
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

        List<Flight> allFlights = new List<Flight>
        {
                new Flight("A1", "azaa", DateTime.Now, "Antananarivo", "Toamasina"),
                new Flight("A2", "azaa", DateTime.Now, "France", "Toamasina"),
                new Flight("A3", "azaa", DateTime.Now, "Antananarivo", "Allemagne"),
                new Flight("A4", "azaa", DateTime.Now, "Sambava", "Toamasina"),
                new Flight("A5", "azaa", DateTime.Now, "Fianarantsoa", "Toamasina"),
        };

    public ObservableCollection<FlightCardViewModel> FlightList{get; set;}
    private void Refresh(List<Flight> flights)
    {
        FlightList.Clear();
        foreach (Flight f in flights)
        {
            FlightList.Add(new FlightCardViewModel( f.Plane,
                                                    f.Company,
                                                    f.PortA,
                                                    f.PortB,
                                                    f.Departure,
                                                    OnDelay,
                                                    OnDelete
                                                    ));
        }
    }
    public VolsViewModel()
    {
        FlightList = new ObservableCollection<FlightCardViewModel>();
        Conf = "confirmer l'ajout";
        Sure = false;
        Refresh(allFlights);
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
        IsEditing = true;
    }

    private void OnDelete(FlightCardViewModel currFlight)
    {
        Sure = true;
        IsEditing = false;
        SelectedFlight = new Flight(currFlight.Plane, currFlight.Company, currFlight.Depart, currFlight.PortA, currFlight.PortB);
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
        if (!IsEditing)
        {
            if (Plane_ != null && A != null && D !=null)
            {
                allFlights.Add(new Flight(Plane_, "me", DateTime.Now, A, D));
            }
            await NotificationAsync("insertion effectuee");
        } else
        {
            if (SelectedFlight != null && DepartureDate != null && DepartureTime != null && A != null && D != null)
            {
                // combine DateTimeOffset and TimeSpan into DateTime
                if (DepartureDate.HasValue)
                {
                    var date = DepartureDate.Value.Date; // DateTime at 00:00
                    var time = DepartureTime ?? TimeSpan.Zero;
                    dt = date.Add(time);
                }
                else
                {
                    dt = DateTime.Now;
                }
                for (int i = 0; i < allFlights.Count; i++)
                {
                    if (allFlights[i].Plane == SelectedFlight.Plane)
                    {
                        allFlights.Add(new Flight(SelectedFlight.Plane, SelectedFlight.Company, dt,D,A));
                        allFlights.RemoveAt(i);
                        await NotificationAsync("modification effectuee");
                        break;
                    }
                }
            }
        }
        Refresh(allFlights);
        Reset();
    }

    [RelayCommand]
    private async Task DelConfirm()
    {
        if (SelectedFlight != null)
        {
            for (int i = 0; i < allFlights.Count; i++)
            {
                if (allFlights[i].Plane == SelectedFlight.Plane &&
                    allFlights[i].Departure == SelectedFlight.Departure)
                {
                    allFlights.RemoveAt(i);
                }
            }
        }
        Refresh(allFlights);
        Reset();
        await NotificationAsync("suppression reussie");

    }
}
