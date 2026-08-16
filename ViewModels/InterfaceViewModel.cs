using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using back.Data;
using Gestion_avion.Messages;
using Gestion_avion.state;
using System.Threading.Tasks;

namespace Gestion_avion.ViewModels;

public partial class InterfaceViewModel : ViewModelBase, IRecipient<DemandeModificationClientMessage>
{
    private readonly AppState _appState;

    private readonly Contextedb _context;

    // Page courante affichée dans le MainView
    [ObservableProperty]
    private ViewModelBase? _currentPage;

    // ViewModels conservés pour éviter la réinstanciation inutile
    public DashboardViewModel DashboardVM { get; }
    public OperationViewModel OperationVM { get; }
    public VolsViewModel VolsVM { get; }
    public ReservationViewModel ReservationVM { get; }
    public PassagerViewModel PassagerVM { get; }

    // Constructeur principal avec injection du DbContext
    public InterfaceViewModel(Contextedb context, AppState appState)
    {
        _appState = appState;
        _context = context ?? throw new ArgumentNullException(nameof(context));

        // Enregistrement au bus de messages
        WeakReferenceMessenger.Default.Register(this);

        // Instanciation des ViewModels avec le DbContext partagé
        DashboardVM = new DashboardViewModel(_appState);
        OperationVM = new OperationViewModel(_appState);
        VolsVM = new VolsViewModel(_appState);
        ReservationVM = new ReservationViewModel(_context);
        PassagerVM = new PassagerViewModel(_appState);

        // Page par défaut
        CurrentPage = DashboardVM;
    }

    // Constructeur sans paramètre (pour le Designer XAML / Fallback)
    public InterfaceViewModel(AppState appState) : this(new Contextedb(), appState)
    {
    }  

    // --- Commandes de Navigation ---
    [RelayCommand]
    private void GoToDashboard() => CurrentPage = DashboardVM;

    [RelayCommand]
    private void GoToOperation() => CurrentPage = OperationVM;

    [RelayCommand]
    private async Task GoToVols()
    {
        CurrentPage = VolsVM;
        await VolsVM.RefreshAvailablePlanesAsync();
    }

    [RelayCommand]
    private void GoToReservation() => CurrentPage = ReservationVM;

    [RelayCommand]
    private void GoToPassager() => CurrentPage = PassagerVM;

    // --- Gestion de la modification d'un client ---
    public void Receive(DemandeModificationClientMessage message)
    {
        if (message?.Value == null) return;
        CurrentPage = ReservationVM;
        ReservationVM.Receive(message);
    }
}