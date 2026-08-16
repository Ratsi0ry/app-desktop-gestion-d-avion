using CommunityToolkit.Mvvm.ComponentModel;

namespace Gestion_avion.Models;

public partial class ClientModel : ObservableObject
{
    [ObservableProperty] private string _idPasseport = "";
    [ObservableProperty] private string _nom = "";
    [ObservableProperty] private string _prenom = "";
    [ObservableProperty] private string _categorie = "";
    [ObservableProperty] private string _classe = "";
    [ObservableProperty] private string _compagnie = "";
    [ObservableProperty] private string _typeVol = "";
    [ObservableProperty] private string _depart = "";
    [ObservableProperty] private string _destination = "";
    [ObservableProperty] private string _date = "";
    [ObservableProperty] private string _heure = "";
    [ObservableProperty] private string _siege = "";
    [ObservableProperty] private decimal _tarif = 0;
    [ObservableProperty] private string _estPaye = "Non";
}
