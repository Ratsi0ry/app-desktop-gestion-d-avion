
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Gestion_avion.ViewModels;

public partial class SignUpViewModel : ViewModelBase
{
    [ObservableProperty]
    private string? _selectedCompanyName;
    
    [ObservableProperty]
    private string? _selectedCompanyId;

    [ObservableProperty]
    public bool _isLogged = false;

    [RelayCommand]
    private void Log() => IsLogged = !IsLogged;
    
    Dictionary<string, string>[] LoggedCompany = new Dictionary<string, string>[]
        {
            new Dictionary<string, string>
            {
                { "Name", "Arry" },
                { "Id", "c001" }
            },
            new Dictionary<string, string>
            {
                { "Name", "Sora" },
                { "Id", "c002" }
            },
            new Dictionary<string, string>
            {
                { "Name", "Laza" },
                { "Id", "c002" }
            },
            new Dictionary<string, string>
            {
                { "Name", "Laza" },
                { "Id", "c002" }
            },
            new Dictionary<string, string>
            {
                { "Name", "Laza" },
                { "Id", "c002" }
            }
        };

    public ObservableCollection<CardViewModel> UserList { get; set; }
    public SignUpViewModel()
    {
        UserList = new ObservableCollection<CardViewModel>();
        for (int i = 0; i < LoggedCompany.Length; i++)
        {
            UserList.Add(new CardViewModel(LoggedCompany[i]["Name"], LoggedCompany[i]["Id"], "user", OnCompanySelected));
        }
    }

    private void OnCompanySelected(CardViewModel clickedCard)
    {
        SelectedCompanyName = clickedCard.ItemName;
        SelectedCompanyId = clickedCard.ItemId;

        IsLogged = true; 
    }
}