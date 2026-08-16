
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Gestion_avion.ViewModels;
using Gestion_avion.state;
using Gestion_avion.Messages;

namespace Gestion_avion.ViewModels;

public partial class MainViewModelR : ViewModelBase, IRecipient<DemandeModificationClientMessage>
{

    private readonly AppState _appState;

    [ObservableProperty]
    private ViewModelBase? _currentInterface;

    [ObservableProperty]
    private bool _isLoading = true;

    [ObservableProperty]
    private string _loadingMessage = "Démarrage de Fast Travel...";

    public MainViewModelR(AppState appState)                                    
    {
        _appState = appState;
        _ = InitializeAppAsync();

        WeakReferenceMessenger.Default.Register(this);
    }

    public void Receive(DemandeModificationClientMessage message)
    {
        throw new System.NotImplementedException();
    }

    private async Task InitializeAppAsync()
    {
        LoadingMessage = "Connexion à la base de données...";
        await Task.Delay(1000);

        LoadingMessage = "Chargement de la liste des vols...";
        await Task.Delay(1000);

        LoadingMessage = "Préparation de l'interface...";
        await Task.Delay(500);

        var signUpVM = new SignUpViewModel(_appState);
        CurrentInterface = signUpVM;
        IsLoading = false;

        
        var logTask = new TaskCompletionSource<bool>();

        System.ComponentModel.PropertyChangedEventHandler handler = null!;
        handler = (s, e) =>
        {
            if (e.PropertyName == nameof(SignUpViewModel.IsLogged) && signUpVM.IsLogged)
            {
                signUpVM.PropertyChanged -= handler;
                logTask.SetResult(true);
            }
        };

        signUpVM.PropertyChanged += handler;

        await logTask.Task;

        IsLoading = true;

        LoadingMessage = "Préparation de l'interface...";
        await Task.Delay(500);

        CurrentInterface = new InterfaceViewModel(_appState);
        IsLoading = false;
    }

}
