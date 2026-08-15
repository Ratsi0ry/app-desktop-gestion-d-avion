
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Gestion_avion.Views.Cards;
using System.Collections.Generic;
using System;
using back.Models;
using Gestion_avion.state;

namespace Gestion_avion.ViewModels;

public partial class CardViewModel : ViewModelBase
{
    private readonly AppState _appState;

    [ObservableProperty]
    public string _itemName, _itemId, _itemTel, itemEmail, _cardType;

    private readonly Action<CardViewModel> _isFocused;
    public CardViewModel(string name, string id, string tel, string email, string type, Action<CardViewModel> isfocused, AppState appState)
    {
        _appState = appState;
        ItemId = id;
        ItemName = name;
        ItemTel = tel;
        ItemEmail = email;
        _cardType = type;
        _isFocused = isfocused;
    }

    [RelayCommand]
    public void action()
    {   
        Compagnie currentCompanie = new Compagnie
        {
            id_compagnie = ItemId,
            nom_compagnie = ItemName,
            tel_compagnie = ItemTel,
            email_compagnie = itemEmail

        };

        _appState.currentCompanie = currentCompanie;

        _isFocused?.Invoke(this);
    }
}