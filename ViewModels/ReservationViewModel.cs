using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using back.Models;
using back.Data;
using Gestion_avion.Messages;
using Gestion_avion.Messages;
using Gestion_avion.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Linq;

namespace Gestion_avion.ViewModels;

public partial class SiegeUiModel : ObservableObject
{
    public string Numero { get; set; } = string.Empty;
    public bool IsEspace { get; set; }
public partial class ReservationViewModel : ViewModelBase, IRecipient<DemandeModificationClientMessage>
{   
    //ref si en modification ou pas
    private ClientModel? _clientEnCoursDeModification = null;

    // --- input 
    [ObservableProperty]
    private string _villeDepart = "", _villeArrivee = "", _dateVol = "", 
                   _heureVol = "", _nom = "", _prenom = "", _idPasseport = "",
                   _categoriePersonne = "", _classeAvion = "", _compagnieAerienne = "",
                   _rechercheId = "";

    [ObservableProperty] private bool _estReserve;
    [ObservableProperty] private bool _estSelectionne;
}

public partial class ReservationViewModel : ViewModelBase, IRecipient<DemandeModificationClientMessage>
{
    private readonly Contextedb _context;

    // --- Saisie Client ---
    [ObservableProperty] private string _idPasseport = string.Empty;
    [ObservableProperty] private string _nom = string.Empty;
    [ObservableProperty] private string _prenom = string.Empty;
    [ObservableProperty] private string _categoriePersonne = "Adulte";

    // --- Sélections Vol & Option ---
    [ObservableProperty] private string _typeVol = "Aller simple";
    [ObservableProperty] private short _sejour;
    [ObservableProperty] private string _classeAvion = "Economique";
    [ObservableProperty] private Compagnie? _compagnieSelectionnee;
    [ObservableProperty] private Vol? _volSelectionne;

    public bool IsSejourVisible => TypeVol == "Aller-retour";

    // --- Propriétés de filtrage ppour lrd villes ---
    [ObservableProperty] private ObservableCollection<string> _villeDepartDispo = new();
    [ObservableProperty] private string? _villeDepart;

    [ObservableProperty] private ObservableCollection<string> _villeDispo = new();
    [ObservableProperty] private string? _villeArrivee;

    [ObservableProperty] private ObservableCollection<string> _dateVolDispo = new();
    [ObservableProperty] private string? _dateVol;

    [ObservableProperty] private ObservableCollection<string> _heureVolDispo = new();
    [ObservableProperty] private string? _heureVol;

    // --- Collections de Données ---
    [ObservableProperty] private ObservableCollection<Compagnie> _compagniesDispo = new();
    [ObservableProperty] private ObservableCollection<Vol> _volsDispo = new();
    [ObservableProperty] private ObservableCollection<string> _typeVolDispo = new() { "Aller simple", "Aller-retour" };
    [ObservableProperty] private ObservableCollection<string> _categoriePersonneDispo = new() { "Adulte", "Enfant", "Bébé" };
    [ObservableProperty] private ObservableCollection<string> _classeAvionDispo = new() { "Economique", "Economique Premium", "Classe Affaire", "Première Classe" };

    // --- Grille de Sièges ---
    [ObservableProperty] private int _nbLignesGrille;
    [ObservableProperty] private ObservableCollection<SiegeUiModel> _listeSieges = new();
    [ObservableProperty] private SiegeUiModel? _siegeSelectionne;

    // --- Modale & Modèle de Ticket ---
    [ObservableProperty] private bool _isTicketVisible;
    [ObservableProperty] private string _ticketNom = string.Empty;
    [ObservableProperty] private string _ticketTrajet = string.Empty;
    [ObservableProperty] private string _ticketDateHeure = string.Empty;
    [ObservableProperty] private string _ticketSiege = string.Empty;
    [ObservableProperty] private string _ticketClasse = string.Empty;
    [ObservableProperty] private string _ticketCompagnie = string.Empty;

    public ReservationViewModel(Contextedb context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));

        WeakReferenceMessenger.Default.Register(this);
        QuestPDF.Settings.License = LicenseType.Community;
    [ObservableProperty]
    private decimal _tarif = 0;

    [ObservableProperty]
    private string _estPaye = "Non";

    [ObservableProperty]
    private ObservableCollection<string> _estPayeDispo = new() { "Oui", "Non" };

    // liste choix
    [ObservableProperty]
    private ObservableCollection<string> _categoriePersonneDispo = new() { "Adulte", "Enfant", "Bébé" };

        _ = InitialiserViewModelAsync();
    }

    private async Task InitialiserViewModelAsync()
    {
        try
        {
            await ChargerCompagniesAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erreur d'initialisation : {ex.Message}");
        }
    }

    public async Task ChargerCompagniesAsync()
    {
        var compagnies = await _context.Compagnie.AsNoTracking().ToListAsync();
        CompagniesDispo = new ObservableCollection<Compagnie>(compagnies);

        if (CompagniesDispo.Any())
        {
            CompagnieSelectionnee = CompagniesDispo.First();
        }
    }

    partial void OnTypeVolChanged(string value) => OnPropertyChanged(nameof(IsSejourVisible));

    partial void OnCompagnieSelectionneeChanged(Compagnie? value)
    {
        _ = ChargerVolsPourCompagnieAsync(value);
    }

    private async Task ChargerVolsPourCompagnieAsync(Compagnie? compagnie)
    {
        if (compagnie == null)
        {
            VolsDispo.Clear();
            VolSelectionne = null;
            MettreAJourListesFiltres();
            return;
        }

        var vols = await _context.Vol
            .Include(v => v.Trajet)
            .Include(v => v.Avion)
            .Where(v => v.Avion.fk_id_compagnie == compagnie.id_compagnie)
            .AsNoTracking()
            .ToListAsync();

        VolsDispo = new ObservableCollection<Vol>(vols);
        
        MettreAJourListesFiltres();
    // siege
    [ObservableProperty]
    private int _nbLignesGrille; 

        VolSelectionne = VolsDispo.FirstOrDefault();
    }

    private void MettreAJourListesFiltres()
    {
        var departList = VolsDispo
            .Select(v => v.Trajet?.lieu_depart)
            .Where(d => !string.IsNullOrWhiteSpace(d))
            .Distinct()
            .Cast<string>()
            .ToList();

        VilleDepartDispo = new ObservableCollection<string>(departList);
        VilleDepart = VilleDepartDispo.FirstOrDefault();

        ActualiserVillesArriveeEtDates();
    }

    partial void OnVilleDepartChanged(string? value)
    {
        ActualiserVillesArriveeEtDates();
        ChargerSieges("Classe Affaire");   
        
        // Enregistrement pour la réception du message
        WeakReferenceMessenger.Default.Register(this);
    }

    // Méthode de réception des données du client à modifier
   public void Receive(DemandeModificationClientMessage message)
    {
        var client = message.Value;

        //suavegarde de la reference
        _clientEnCoursDeModification = client;

        IdPasseport = client.IdPasseport;
        Nom = client.Nom;
        Prenom = client.Prenom;
        CategoriePersonne = client.Categorie;
        ClasseAvion = client.Classe;
        CompagnieAerienne = client.Compagnie;
        TypeVol = client.TypeVol;
        VilleDepart = client.Depart;
        VilleArrivee = client.Destination;
        DateVol = client.Date;
        HeureVol = client.Heure;
        Tarif = client.Tarif;
        EstPaye = client.EstPaye;   
        
        //preselection siege
         var siege = ListeSieges.FirstOrDefault(s => s.Numero == client.Siege);
        if (siege != null)
        {
            siege.EstSelectionne = true;
            SiegeSelectionne = siege;
        }
    }

    private void ActualiserVillesArriveeEtDates()
    {
        var volsFiltres = VolsDispo.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(VilleDepart))
        {
            volsFiltres = volsFiltres.Where(v => v.Trajet?.lieu_depart == VilleDepart);
        }

        var listVols = volsFiltres.ToList();

        var arriveeList = listVols
            .Select(v => v.Trajet?.destination)
            .Where(a => !string.IsNullOrWhiteSpace(a))
            .Distinct()
            .Cast<string>()
            .ToList();

        VilleDispo = new ObservableCollection<string>(arriveeList);
        VilleArrivee = VilleDispo.FirstOrDefault();

        var datesList = listVols
            .Select(v => v.fk_date_depart)
            .Where(d => !string.IsNullOrWhiteSpace(d))
            .Distinct()
            .Cast<string>()
            .ToList();

        DateVolDispo = new ObservableCollection<string>(datesList);
        DateVol = DateVolDispo.FirstOrDefault();

        VolSelectionne = listVols.FirstOrDefault();
    }

    partial void OnVolSelectionneChanged(Vol? value)
    {
        if (value?.Trajet != null)
        {
            _villeDepart = value.Trajet.lieu_depart;
            OnPropertyChanged(nameof(VilleDepart));

            _villeArrivee = value.Trajet.destination;
            OnPropertyChanged(nameof(VilleArrivee));

            _dateVol = value.fk_date_depart;
            OnPropertyChanged(nameof(DateVol));
        }

        _ = RechargerGrilleSiegesAsync();
    }

    partial void OnClasseAvionChanged(string value)
    {
        _ = RechargerGrilleSiegesAsync();
    }

    private async Task RechargerGrilleSiegesAsync()
    {
        if (VolSelectionne == null || string.IsNullOrWhiteSpace(ClasseAvion)) return;

        SiegeSelectionne = null;
        ListeSieges.Clear();

        var siegesOccupes = await _context.Reservation
            .Where(r => r.fk_id_vol == VolSelectionne.id_vol)
            .Select(r => r.fk_numero_place)
            .AsNoTracking()
            .ToListAsync();

        switch (ClasseAvion)
        {
            case "Première Classe":
                GenererSieges(4, siegesOccupes, "A", "B", "C", "D");
                break;
            case "Classe Affaire":
                GenererSieges(5, siegesOccupes, "E", "F", "G", "H");
                break;
            case "Economique Premium":
                GenererSieges(6, siegesOccupes, "I", "J", "K", "L");
                break;
            default:
                GenererSieges(8, siegesOccupes, "M", "N", "O", "P");
                break;
        }
    }

    private void GenererSieges(int nbRangees, List<string> siegesOccupes, params string[] lettres)
    {
        NbLignesGrille = lettres.Length + 1;
        int milieu = lettres.Length / 2;

        for (int i = 0; i < lettres.Length; i++)
        {
            if (i == milieu)
            {
                for (int r = 1; r <= nbRangees; r++)
                {
                    ListeSieges.Add(new SiegeUiModel { IsEspace = true });
                }
            }

            for (int r = 1; r <= nbRangees; r++)
            {
                string numSiege = $"{lettres[i]}{r}";
                ListeSieges.Add(new SiegeUiModel
                {
                    Numero = numSiege,
                    EstReserve = siegesOccupes.Contains(numSiege)
                });
            }
        }
    }

    [RelayCommand]
    private void SelectionnerSiege(SiegeUiModel? siege)
    {
        if (siege == null || siege.EstReserve || siege.IsEspace) return;

        foreach (var s in ListeSieges)
        {
            s.EstSelectionne = false;
        }

        siege.EstSelectionne = true;
        SiegeSelectionne = siege;
        // recuperation donee atao amn ticket
        TicketNom = $"{Nom} {Prenom}".ToUpper();
        TicketTrajet = $"{VilleDepart} à {VilleArrivee}";
        TicketDateHeure = $"{DateVol} - {HeureVol}";
        TicketSiege = SiegeSelectionne.Numero ?? "N/A";
        TicketClasse = ClasseAvion;
        TicketCompagnie = CompagnieAerienne;

         // AJOUT : écrire les données dans le client
        var client = _clientEnCoursDeModification ?? new ClientModel();
        bool estNouveau = _clientEnCoursDeModification == null;

        client.IdPasseport = IdPasseport;
        client.Nom = Nom;
        client.Prenom = Prenom;
        client.Categorie = CategoriePersonne;
        client.Classe = ClasseAvion;
        client.Compagnie = CompagnieAerienne;
        client.TypeVol = TypeVol;
        client.Depart = VilleDepart;
        client.Destination = VilleArrivee;
        client.Date = DateVol;
        client.Heure = HeureVol;
        client.Siege = SiegeSelectionne.Numero ?? "";
        client.Tarif = Tarif;
        client.EstPaye = EstPaye;

        if (estNouveau)
        WeakReferenceMessenger.Default.Send(new ClientEnregistreMessage(client));


        // pop up ticket
        IsTicketVisible = true;
    }

    [RelayCommand]
    private async Task ReserverAsync()
    {
        if (SiegeSelectionne == null || VolSelectionne == null || CompagnieSelectionnee == null ||
            string.IsNullOrWhiteSpace(Nom) || string.IsNullOrWhiteSpace(Prenom) || string.IsNullOrWhiteSpace(IdPasseport))
        {
            return;
        }

        try
    [RelayCommand]
    private void Annuler()
    {   
        _clientEnCoursDeModification = null;
        IdPasseport = "";
        Nom = "";
        Prenom = "";
        Sejour = 0;
        CategoriePersonne = "";
        ClasseAvion = "";
        CompagnieAerienne = "";
        TypeVol = "";
        VilleDepart = "";
        VilleArrivee = "";
        DateVol = "";
        HeureVol = "";

        if (SiegeSelectionne != null)
        {
            var passager = await _context.Passager.FirstOrDefaultAsync(p => p.passeport == IdPasseport);
            if (passager == null)
            {
                passager = new Passager
                {
                    passeport = IdPasseport,
                    nom_passager = Nom,
                    prenom_passager = Prenom,
                    tel_passager = "0000000000",
                    categorie_passager = CategoriePersonne
                };
                _context.Passager.Add(passager);
            }

            var place = await _context.Place.FirstOrDefaultAsync(p => p.numero_place == SiegeSelectionne.Numero);
            if (place == null)
            {
                place = new Place
                {
                    numero_place = SiegeSelectionne.Numero,
                    classe_siege = ClasseAvion,
                    occupee = 1
                };
                _context.Place.Add(place);
            }
            else
            {
                place.occupee = 1;
            }

            var reservation = new Reservation
            {
                id_reservation = Guid.NewGuid().ToString("N")[..10],
                date_reservation = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                valide = 1,
                fk_passeport = passager.passeport,
                fk_id_vol = VolSelectionne.id_vol,
                fk_numero_place = SiegeSelectionne.Numero
            };

            _context.Reservation.Add(reservation);
            await _context.SaveChangesAsync();

            TicketNom = $"{Nom} {Prenom}".ToUpperInvariant();
            TicketTrajet = VolSelectionne.Trajet != null 
                ? $"{VolSelectionne.Trajet.lieu_depart} ➔ {VolSelectionne.Trajet.destination}" 
                : "Trajet Inconnu";
            TicketDateHeure = VolSelectionne.fk_date_depart;
            TicketSiege = SiegeSelectionne.Numero;
            TicketClasse = ClasseAvion;
            TicketCompagnie = CompagnieSelectionnee.nom_compagnie;

            IsTicketVisible = true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erreur lors de la réservation : {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task TelechargerEtFermerTicketAsync()
    {
        try
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string downloadsFolder = Path.Combine(folderPath, "Downloads");

            if (!Directory.Exists(downloadsFolder))
            {
                Directory.CreateDirectory(downloadsFolder);
            }

            string safeFileName = $"Ticket_{TicketNom.Replace(" ", "_")}.pdf";
            string filePath = Path.Combine(downloadsFolder, safeFileName);

            GenererDocumentPdf().GeneratePdf(filePath);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erreur lors de l'impression PDF : {ex.Message}");
        }
        finally
        {
            IsTicketVisible = false;
            await AnnulerAsync();
        }
    }

    public IRelayCommand FermerTicketCommand => TelechargerEtFermerTicketCommand;

    private Document GenererDocumentPdf()
    {
        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(500, 300);
                page.Margin(20);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(11).FontColor(Colors.Black));

                page.Header().Element(header =>
                {
                    header.Background("#1E293B").Padding(15).Row(row =>
                    {
                        row.RelativeItem().Text("TICKET DE BORD").Bold().FontSize(16).FontColor(Colors.White);
                        row.ConstantItem(150).AlignRight().Text(TicketCompagnie).FontColor(Colors.White).Bold();
                    });
                });

                page.Content().Padding(15).Column(column =>
                {
                    column.Spacing(10);
                    column.Item().Text(t => { t.Span("PASSAGER :\n").FontSize(8).FontColor(Colors.Grey.Medium); t.Span(TicketNom).Bold().FontSize(14); });
                    column.Item().Text(t => { t.Span("TRAJET :\n").FontSize(8).FontColor(Colors.Grey.Medium); t.Span(TicketTrajet).SemiBold().FontSize(12); });
                    column.Item().Text(t => { t.Span("DATE ET HEURE :\n").FontSize(8).FontColor(Colors.Grey.Medium); t.Span(TicketDateHeure).FontSize(11); });
                    column.Item().Row(row =>
                    {
                        row.RelativeItem().Text(t => { t.Span("CLASSE :\n").FontSize(8).FontColor(Colors.Grey.Medium); t.Span(TicketClasse).Bold().FontSize(11); });
                        row.RelativeItem().Text(t => { t.Span("SIÈGE :\n").FontSize(8).FontColor(Colors.Grey.Medium); t.Span(TicketSiege).Bold().FontSize(14); });
                    });
                });
            });
        });
    }

    public void ChargerClient(ClientModel client)
    {
        if (client == null) return;

        IdPasseport = client.IdPasseport;
        Nom = client.Nom;
        Prenom = client.Prenom;
        CategoriePersonne = client.Categorie;
        ClasseAvion = client.Classe;
        CompagnieAerienne = client.Compagnie;
        TypeVol = client.TypeVol;
        VilleDepart = client.Depart;
        VilleArrivee = client.Destination;
        DateVol = client.Date;
        HeureVol = client.Heure;

    }
}

    [RelayCommand]
    private async Task AnnulerAsync()
    {
        IdPasseport = string.Empty;
        Nom = string.Empty;
        Prenom = string.Empty;
        Sejour = 0;

        await RechargerGrilleSiegesAsync();
    }

    public void Receive(DemandeModificationClientMessage message)
    {
        if (message?.Value == null) return;

        var client = message.Value;
        IdPasseport = client.IdPasseport;
        Nom = client.Nom;
        Prenom = client.Prenom;
        CategoriePersonne = client.Categorie;
        ClasseAvion = client.Classe;
    }
    [ObservableProperty]
    private bool _estReserve, _estSelectionne, _isEspace;
}