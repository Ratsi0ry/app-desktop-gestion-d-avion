using System;
using System.Collections.ObjectModel;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Gestion_avion.ViewModels;

public partial class ReservationViewModel : ViewModelBase
{
    // --- input 
    [ObservableProperty]
    private string _villeDepart = "", _villeArrivee = "", _dateVol = "", 
                   _heureVol = "", _nom = "", _prenom = "", _idPasseport = "", 
                   _categoriePersonne = "", _classeAvion = "", _compagnieAerienne = "";

    // affichage sejour
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsSejourVisible))]
    private string _typeVol = "";

    [ObservableProperty]
    private short _age, _nbPersonne, _sejour;

    public bool IsSejourVisible => TypeVol == "Aller-retour";


    // liste choix
    [ObservableProperty]
    private ObservableCollection<string> _categoriePersonneDispo = new() { "Adulte", "Enfant", "Bébé" };

    [ObservableProperty]
    private ObservableCollection<string> _classeAvionDispo = new() { "Economique", "Economique Premium", "classe Affaire", "Première classe" };

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


    // siege
    [ObservableProperty]
    private int _nbLignesGrille; 

    [ObservableProperty]
    private ObservableCollection<Siege> _listeSieges = new(); 

    [ObservableProperty]
    private Siege? _siegeSelectionne;

    // ticket
    [ObservableProperty]
    private bool _isTicketVisible;

    [ObservableProperty]
    private string _ticketNom = "", _ticketTrajet = "", _ticketDateHeure = "",
                   _ticketSiege = "", _ticketClasse = "", _ticketCompagnie = "";

    public ReservationViewModel()
    {
        ChargerSieges("Classe Affaire");   
    }

    [RelayCommand]
    private void ChoisirClasse(string classe) => ChargerSieges(classe);

    [RelayCommand]
    private void SelectionnerSiege(Siege siege)
    {
        if (siege == null || siege.EstReserve || siege.IsEspace) return;

        foreach (var s in ListeSieges) s.EstSelectionne = false;
        
        siege.EstSelectionne = true;
        SiegeSelectionne = siege;
    }

    private void ChargerSieges(string classe)
    {
        ListeSieges.Clear();

        switch (classe)
        {
            case "Première Classe":
                GenererSieges(4, "A", "B", "C", "D"); 
                break;
            case "Classe Affaire":
                GenererSieges(5, "E", "F", "G", "H");
                break;
            case "Economique Premium":
                GenererSieges(6, "I", "J", "K", "L");
                break;
            case "Economique":
            default:
                GenererSieges(8, "M", "N", "O", "P");
                break;
        }
    }

    //affichage sieges selon classe choisi
    partial void OnClasseAvionChanged(string value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            ChargerSieges(value);
        }
    }

    private void GenererSieges(int nbRangees, params string[] lettres)
    {
        NbLignesGrille = lettres.Length + 1;

        int milieu = lettres.Length / 2;

        for (int i = 0; i < lettres.Length; i++)
        {
            if (i == milieu)
            {
                for (int r = 1; r <= nbRangees; r++)
                    ListeSieges.Add(new Siege { IsEspace = true });
            }

            for (int r = 1; r <= nbRangees; r++)
            {
                ListeSieges.Add(new Siege { Numero = $"{lettres[i]}{r}", EstReserve = false });
            }
        }
    }
    
    //bouton reserver et annuler
    [RelayCommand]
    private void Reserver()
    {
        if (SiegeSelectionne == null || string.IsNullOrWhiteSpace(Nom) || string.IsNullOrWhiteSpace(Prenom))
        {
            return;
        }

        // recuperation donee atao amn ticket
        TicketNom = $"{Nom} {Prenom}".ToUpper();
        TicketTrajet = $"{VilleDepart} à {VilleArrivee}";
        TicketDateHeure = $"{DateVol} - {HeureVol}";
        TicketSiege = SiegeSelectionne.Numero ?? "N/A";
        TicketClasse = ClasseAvion;
        TicketCompagnie = CompagnieAerienne;

        // pop up ticket
        IsTicketVisible = true;
    }

    [RelayCommand]
    private void FermerTicket()
    {
        IsTicketVisible = false;
        
        // reinitialisatio champ
        Annuler(); 
    }

    [RelayCommand]
    private void Annuler()
    {
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
            SiegeSelectionne.EstSelectionne = false;
            SiegeSelectionne = null;
        }
    }
                   
    // telechargement ticket avec QuestPDF
    [RelayCommand]
    private void TelechargerEtFermerTicket()
    {
        try
        {
            // Configuration de la licence QuestPDF Community
            QuestPDF.Settings.License = LicenseType.Community;

            string dossierDownloads = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            string nomFichier = $"Ticket_{TicketNom.Replace(" ", "_")}.pdf";
            string cheminFichier = Path.Combine(dossierDownloads, nomFichier);

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(500, 300);
                    page.Margin(20);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11).FontColor(Colors.Black));

                    // En-tête du PDF
                    page.Header().Element(header =>
                    {
                        header.Background("#1E293B").Padding(15).Row(row =>
                        {
                            row.RelativeItem().Text("TICKET").Bold().FontSize(18).FontColor(Colors.White);
                            row.ConstantItem(120).AlignRight().Text(TicketCompagnie).FontColor(Colors.White).Bold();
                        });
                    });

                    // Contenu du PDF
                    page.Content().Padding(15).Column(column =>
                    {
                        column.Spacing(12);

                        column.Item().Text(text =>
                        {
                            text.Span("PASSAGER :\n").FontSize(9).FontColor(Colors.Grey.Medium);
                            text.Span(TicketNom).Bold().FontSize(16);
                        });

                        column.Item().Text(text =>
                        {
                            text.Span("TRAJET :\n").FontSize(9).FontColor(Colors.Grey.Medium);
                            text.Span(TicketTrajet).SemiBold().FontSize(14);
                        });

                        column.Item().Text(text =>
                        {
                            text.Span("DATE ET HEURE :\n").FontSize(9).FontColor(Colors.Grey.Medium);
                            text.Span(TicketDateHeure).FontSize(12);
                        });

                        column.Item().Row(row =>
                        {
                            row.RelativeItem().Text(text =>
                            {
                                text.Span("CLASSE :\n").FontSize(9).FontColor(Colors.Grey.Medium);
                                text.Span(TicketClasse).Bold().FontSize(12);
                            });
                            row.RelativeItem().Text(text =>
                            {
                                text.Span("SIÈGE :\n").FontSize(9).FontColor(Colors.Grey.Medium);
                                text.Span(TicketSiege).Bold().FontSize(16);
                            });
                        });
                    });

                    // Pied de page
                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Fast Travel").FontSize(9).FontColor(Colors.Grey.Darken1);
                    });
                });
            })
            .GeneratePdf(cheminFichier);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la génération du PDF : {ex.Message}");
        }

        IsTicketVisible = false;
        Annuler();
    }
}

public partial class Siege : ObservableObject
{
    [ObservableProperty]
    private string? _numero;

    [ObservableProperty]
    private bool _estReserve, _estSelectionne, _isEspace;
}