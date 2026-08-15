
using CommunityToolkit.Mvvm.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Gestion_avion.ViewModels;
using Gestion_avion.state;

namespace Gestion_avion.ViewModels;

public partial class MainViewModel : ViewModelBase
{

    private readonly AppState _appState;

    //interface handling
    [ObservableProperty]
    private ViewModelBase? _currentInterface;

    [ObservableProperty]
    private bool _isLoading = true;

    [ObservableProperty]
    private string _loadingMessage = "Démarrage de Fast Travel...";

    public MainViewModel(AppState appState)                                    
    {
        _appState = appState;
        _ = InitializeAppAsync();
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

    // 3. S'abonner au changement de propriété
    System.ComponentModel.PropertyChangedEventHandler handler = null!;
    handler = (s, e) =>
    {
        // Remplacer "IsLogged" par le nom exact de votre propriété dans SignUpViewModel
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
