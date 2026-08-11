using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Gestion_avion.ViewModels;

public class VolsViewModel : ViewModelBase
{
    Dictionary<string, string>[] allFlights = new Dictionary<string, string>[]
        {
            new Dictionary<string, string>
            {
                { "Plane", "Arry" },
                { "Company", "azaa" },
                { "PortA", "Tananarive"},
                { "PortB", "Toamasina"},
                { "Depart", "12h"}
            },
            new Dictionary<string, string>
            {
                { "Plane", "Sora" },
                { "Company", "sryi" },
                { "PortA", "Tananarive"},
                { "PortB", "Toamasina"},
                { "Depart", "12h"}
            },
            new Dictionary<string, string>
            {
                { "Plane", "Abata" },
                { "Company", "akou" },
                { "PortA", "Tananarive"},
                { "PortB", "Toamasina"},
                { "Depart", "12h"}
            },
            new Dictionary<string, string>
            {
                { "Plane", "Bibo" },
                { "Company", "kaor" },
                { "PortA", "Tananarive"},
                { "PortB", "Toamasina"},
                { "Depart", "12h"}
            },
            new Dictionary<string, string>
            {
                { "Plane", "kasp" },
                { "Company", "tsar" },
                { "PortA", "Tananarive"},
                { "PortB", "Toamasina"},
                { "Depart", "12h"}
            }
        };

    public ObservableCollection<FlightCardViewModel> FlightList{get; set;}
    public VolsViewModel()
    {
        FlightList = new ObservableCollection<FlightCardViewModel>();
        for (int i = 0; i < allFlights.Length; i++)
        {
            FlightList.Add(new FlightCardViewModel( allFlights[i]["Plane"],
                                                    allFlights[i]["Company"],
                                                    allFlights[i]["PortA"],
                                                    allFlights[i]["PortB"],
                                                    allFlights[i]["Depart"]
                                                    ));
        }
    }
    
}
