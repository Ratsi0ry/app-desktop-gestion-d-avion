using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Gestion_avion.Models;
using Gestion_avion.Messages;
using Gestion_avion.state;

namespace Gestion_avion.ViewModels;

public partial class PassagerViewModel : ViewModelBase
{
    // Source de données originale pour conserver l'état complet
    private readonly List<ClientModel> _tousLesClients = new();

    private readonly AppState _appState;

    [ObservableProperty]
    private string _rechercheId = "";

    [ObservableProperty]
    private ObservableCollection<ClientModel> _listeClients = new();

    [ObservableProperty]
    private ObservableCollection<string> _categoriePersonneDispo = new() { "Adulte", "Enfant", "Bébé" };

    [ObservableProperty]
    private ObservableCollection<string> _classeAvionDispo = new() { "Economique", "Economique Premium", "Classe Affaire", "Première Classe" };

    [ObservableProperty]
    private ObservableCollection<string> _compagnieAerienneDispo = new() { "Madagascar Airlines", "Ethiopian Airlines", "Air France", "Air Mauritius" };

    [ObservableProperty]
    private ObservableCollection<string> _typeVolDispo = new() { "Aller simple", "Aller-retour" };

    [ObservableProperty]
    private ObservableCollection<string> _villeDepartDispo = new() { "Antananarivo", "Fianarantsoa", "Toamasina" };

    [ObservableProperty]
    private ObservableCollection<string> _villeDispo = new() { "Toliara", "Antsirabe", "Nosy Be" };

    [ObservableProperty]
    private ObservableCollection<string> _dateVolDispo = new() { "06/08/2026", "18/08/2026", "30/08/2026" };

    [ObservableProperty]
    private ObservableCollection<string> _heureVolDispo = new() { "03:00", "14:30", "22:15" };


    public PassagerViewModel(AppState appState)
    {

        _appState = appState;

    public PassagerViewModel()
    {
        // Chargement des données dans la liste maîtresse
        _tousLesClients.Add(new ClientModel 
        { 
            IdPasseport = "F4b937", 
            Nom = "John", 
            Prenom = "Doe", 
            Categorie = "Adulte", 
            Classe = "Economique", 
            Compagnie = "Air France",
            TypeVol = "Aller simple",
            Depart = "Antananarivo", 
            Destination = "Nosy Be", 
            Date = "06/08/2026", 
            Heure = "14:30",
            Siege = "M1"
        });

        _tousLesClients.Add(new ClientModel 
        { 
            IdPasseport = "A98765", 
            Nom = "Jane", 
            Prenom = "Dolph", 
            Categorie = "Adulte", 
            Classe = "Classe Affaire", 
            Compagnie = "Madagascar Airlines",
            TypeVol = "Aller-retour",
            Depart = "Antananarivo", 
            Destination = "Toamasina", 
            Date = "18/08/2026", 
            Heure = "03:00",
            Siege = "E3"
        });

        // Affichage initial
        FiltrerClients();
    }


    partial void OnRechercheIdChanged(string value)
    {
        FiltrerClients();
    }

    private void FiltrerClients()
    {
        if (string.IsNullOrWhiteSpace(RechercheId))
        {
            ListeClients = new ObservableCollection<ClientModel>(_tousLesClients);
            return;
        }

        var filtre = RechercheId.Trim().ToLower();

        var resultats = _tousLesClients.Where(c =>
            c.IdPasseport.ToLower().Contains(filtre) ||
            c.Nom.ToLower().Contains(filtre) ||
            c.Prenom.ToLower().Contains(filtre) ||
            c.Categorie.ToLower().Contains(filtre) ||
            c.Classe.ToLower().Contains(filtre) ||
            c.Compagnie.ToLower().Contains(filtre) ||
            c.TypeVol.ToLower().Contains(filtre) ||
            c.Depart.ToLower().Contains(filtre) ||
            c.Destination.ToLower().Contains(filtre) ||
            c.Date.ToLower().Contains(filtre) ||
            c.Heure.ToLower().Contains(filtre) 

        );

        ListeClients = new ObservableCollection<ClientModel>(resultats);
    }

    // Supprimer client
    [RelayCommand]
    private void SupprimerClient(ClientModel client)
    {
        if (client != null && _tousLesClients.Contains(client))
        {
            _tousLesClients.Remove(client);
            FiltrerClients();
        }
    }

    // Modification client
    [RelayCommand]
    private void ModifierClient(ClientModel client)
    {
        if (client != null)
        {
            WeakReferenceMessenger.Default.Send(new DemandeModificationClientMessage(client));
        }
    }
}