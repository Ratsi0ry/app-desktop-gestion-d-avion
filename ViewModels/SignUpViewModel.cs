using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using back.Data;
using back.Models;
using Gestion_avion.state;

namespace Gestion_avion.ViewModels;

public partial class SignUpViewModel : ViewModelBase
{
    private readonly AppState _appState;

    private readonly Contextedb _db;

    [ObservableProperty]
    private string? _selectedCompanyName;

    [ObservableProperty]
    private string? _selectedCompanyId;

    [ObservableProperty]
    private bool _isLogged = false;

    [ObservableProperty]
    private bool _isLoading = false;

    /**
     * 
     * REGISTER VARIABLE
     * 
     * */
     [ObservableProperty]
     private string _registerCompanyCode = string.Empty;

     [ObservableProperty]
     private string _registerCompanyName = string.Empty;

     [ObservableProperty]
     private string _registerCompanyEmail= string.Empty;

     [ObservableProperty]
     private string _registerCompanyPassword = string.Empty;

     [ObservableProperty]
     private string _registerCompanyConfirmPassword = string.Empty;

     [ObservableProperty]
     private string _registerCompanyCellContact = string.Empty;

     public event Action? OnRegistrationSuccess;

    public ObservableCollection<CardViewModel> UserList { get; set; } = new();

    public SignUpViewModel(Contextedb db, AppState appState)
    {
        _appState = appState;
        _db = db;
        _ = LoadCompaniesFromDbAsync();
    }

    public SignUpViewModel(AppState appState)
    {
        _appState = appState;
        _db = new Contextedb();
        _ = LoadCompaniesFromDbAsync();
    }

    [RelayCommand]
    private void Log() => IsLogged = !IsLogged;

    [RelayCommand]
    public async Task LoadCompaniesFromDbAsync()
    {
        try
        {
            IsLoading = true;
            UserList.Clear();

            var compagnies = await _db.Compagnie
                .AsNoTracking()
                .ToListAsync();

            foreach (var comp in compagnies)
            {
                UserList.Add(new CardViewModel(
                    comp.nom_compagnie,
                    comp.id_compagnie,
                    comp.tel_compagnie,
                    comp.email_compagnie,
                    "user",
                    OnCompanySelected,
                    _appState
                ));
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erreur lors de la connexion à la BDD : {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void OnCompanySelected(CardViewModel clickedCard)
    {
        SelectedCompanyName = clickedCard.ItemName;
        SelectedCompanyId = clickedCard.ItemId;
        IsLogged = true;
    }


    /**
     * 
     * REGISTER FUNCTION
     *  
     * */
     [RelayCommand]
    private async Task ConfirmRegister() {
        if(
            !string.IsNullOrWhiteSpace(RegisterCompanyPassword)
            && RegisterCompanyPassword == RegisterCompanyConfirmPassword
            && !string.IsNullOrWhiteSpace(RegisterCompanyCellContact)
            && !string.IsNullOrWhiteSpace(RegisterCompanyName) 
            && !string.IsNullOrWhiteSpace(RegisterCompanyEmail) 
            &&  RegisterCompanyCode.Length == 3) {

            await RegisterNewCompany(RegisterCompanyCode, RegisterCompanyName, RegisterCompanyPassword, RegisterCompanyEmail, RegisterCompanyCellContact);
            OnRegistrationSuccess?.Invoke();
        }
    }

    private async Task RegisterNewCompany(string code, string name, string password, string email, string contact) {
        var nouvelleCompagnie = new Compagnie
        {
            id_compagnie = code,
            nom_compagnie = name,
            tel_compagnie = contact,
            email_compagnie = email
        };

        try
        {
            _db.Compagnie.Add(nouvelleCompagnie);
            await _db.SaveChangesAsync();

            RegisterCompanyCode = string.Empty;
            RegisterCompanyName = string.Empty;
            RegisterCompanyCellContact = string.Empty;
            RegisterCompanyEmail = string.Empty;
            RegisterCompanyPassword = string.Empty;
            RegisterCompanyConfirmPassword = string.Empty;

            OnRegistrationSuccess?.Invoke();
            
            await LoadCompaniesFromDbAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Erreur BDD : {ex.Message}");
        }
    }

    /// <summary>
    /// Control le champ, cell contact
    /// value refere vers la future value 
    /// <summary>
}