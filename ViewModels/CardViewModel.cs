
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Gestion_avion.Views.Cards;
using System.Collections.Generic;
using System;
namespace Gestion_avion.ViewModels;

public partial class CardViewModel : ViewModelBase
{
    [ObservableProperty]
    public string _itemName, _itemId, _cardType;

    private readonly Action<CardViewModel> _isFocused;
    public CardViewModel(string name, string id, string type, Action<CardViewModel> isfocused)
    {
        ItemId = id;
        ItemName = name;
        _cardType = type;
        _isFocused = isfocused;
    }

    [RelayCommand]
    public void action()
    {
        _isFocused?.Invoke(this);
    }
}