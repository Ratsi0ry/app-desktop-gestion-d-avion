using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
namespace Gestion_avion.ViewModels;

public partial class InterfaceViewModel : ViewModelBase
{
     //initialisation
    [ObservableProperty]
    private ViewModelBase? _currentPage;

    //navigation entre les pages
    [RelayCommand]
    private void GoToDashboard() => CurrentPage = new DashboardViewModel();

    [RelayCommand]
    private void GoToOperation() => CurrentPage = new OperationViewModel();

    [RelayCommand]
    private void GoToVols() => CurrentPage = new VolsViewModel();

    [RelayCommand]
    private void GoToReservation() => CurrentPage = new ReservationViewModel();
    public InterfaceViewModel()
    {
        CurrentPage = new DashboardViewModel();
    }
}