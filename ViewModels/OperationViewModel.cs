
using CommunityToolkit.Mvvm.ComponentModel;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System;
using CommunityToolkit.Mvvm.Input;

namespace Gestion_avion.ViewModels;

public partial class OperationViewModel: ViewModelBase
{
    [ObservableProperty]
    private DateTime? _selectedDate;

    [ObservableProperty]
    private PlaneStatusViewModel _viewPlane;

    [ObservableProperty]
    private string? _selectedPlaneName;
    
    [ObservableProperty]
    private string? _selectedPlaneId;

    [ObservableProperty]
    private string? _upName, _upCompanie, _upPointA, _upPointB;

    [ObservableProperty]
    private bool _confirmDelete = false, _isCreating = false;
    
    class Classes
        {
            public string CName;
            public int Places;
            public Classes(string n, int p)
            {
                CName = n;
                Places = p;
            }
        }

    class Plane
    {
        
        public List<Classes> PClasses;
        public string Name, Id, TotalPlace, PointA, PointB, Companie;
        public Plane(string name,string id, string places, string A, string B, List<Classes> cliste, string companie)
        {
            Name = name;
            Id = id;
            TotalPlace = places;
            PointA = A;
            PointB = B;
            PClasses = cliste;
            Companie = companie;
        }
    }

    List<Plane> RegisteredPlane = new List<Plane>
    {
        new Plane("asterio", "p222", "220", "tana", "fianarantsoa", new List<Classes>
        {
            new Classes("economique", 200),
            new Classes("VIP", 20)
        }, "aaa"),
        new Plane("alaal", "p282", "220", "tana", "fianarantsoa", new List<Classes>
        {
            new Classes("economique", 210),
            new Classes("VIP", 10)
        }, "bbb"),
        new Plane("poopsocpa", "p262", "260", "tana", "fianarantsoa", new List<Classes>
        {
            new Classes("economique", 200),
            new Classes("VIP", 60)
        }, "ccc"),
        new Plane("bIAWUBh", "p272", "270", "tana", "fianarantsoa", new List<Classes>
        {
            new Classes("economique", 200),
            new Classes("VIP", 70)
        }, "ddd")
    };

    public ObservableCollection<CardViewModel> PlaneList { get; set; }
    public ObservableCollection<TextBlock> Classlist {get; set;}

    public OperationViewModel()
    {
        PlaneList = new ObservableCollection<CardViewModel>();
        Classlist = new ObservableCollection<TextBlock>();
        Classlist.Add(new TextBlock {Text = ""});
        foreach (Plane p in RegisteredPlane)
        {
            PlaneList.Add(new CardViewModel(p.Name, p.Id, "plane", OnPlaneSelected));
        }
        ViewPlane = new PlaneStatusViewModel(true, Classlist, delete: OnPlaneDelete);
    }

    private void OnPlaneSelected(CardViewModel clickedCard)
    {
        IsCreating = false;
        SelectedPlaneName = clickedCard.ItemName;
        SelectedPlaneId = clickedCard.ItemId;
        string arrivee, depart;
        foreach (Plane p in RegisteredPlane)
        {
            if (p.Name == SelectedPlaneName && p.Id == SelectedPlaneId)
            {
                depart = p.PointA;
                arrivee = p.PointB;
                if (Classlist != null)
                {
                    Classlist.Clear();
                    foreach(Classes c in p.PClasses)
                    {
                        Classlist.Add(new TextBlock {Text = c.CName + " :" + c.Places} );
                    }
                }
                UpName = SelectedPlaneName;
                UpCompanie = p.Companie;
                UpPointA = depart;
                UpPointB = arrivee;
                ViewPlane = new PlaneStatusViewModel(false, Classlist, p.TotalPlace, SelectedPlaneName, SelectedPlaneId, depart, arrivee, p.Companie, DateTime.Now.ToString("yyyy-MM-dd"), delete: OnPlaneDelete);
                break;
            } else
            {
                continue;
            }
        }

    }

    private void RefreshPlaneList()
    {
        PlaneList.Clear();
        foreach (Plane p in RegisteredPlane)
        {
            PlaneList.Add(new CardViewModel(p.Name, p.Id, "plane", OnPlaneSelected));
        }

        if (RegisteredPlane.Count > 0)
            ViewPlane = new PlaneStatusViewModel(true, Classlist, delete: OnPlaneDelete);
        else
        {
            ViewPlane = new PlaneStatusViewModel(true, Classlist, delete: OnPlaneDelete);
        }
    }
    private void OnPlaneDelete(PlaneStatusViewModel vm)
    {
        for (int i = 0; i < RegisteredPlane.Count; i++)
        {
            if (RegisteredPlane[i].Name == vm.PlaneName && RegisteredPlane[i].Id == vm.PlaneId)
            {
                SelectedPlaneName = vm.PlaneName;
                SelectedPlaneId = vm.PlaneId;
                ConfirmDelete = true;     
                break;
            }
        }
    }

    partial void OnSelectedDateChanged(DateTime? value)
    {
        if (value.HasValue)
        {
            string formattedDate = value.Value.ToString("yyyy-MM-dd");
            ViewPlane.ViewDate = formattedDate;
        }
    }

    [RelayCommand]
    private void CreatePlane()
    {
        IsCreating = true;
    }

    [RelayCommand]
    private void ConfirmDelete_()
    {
        if (ViewPlane != null)
        {
            OnPlaneDelete(ViewPlane);
        }

        for (int i = 0; i < RegisteredPlane.Count; i++)
        {
            if (RegisteredPlane[i].Name == SelectedPlaneName && RegisteredPlane[i].Id == SelectedPlaneId)
            {
                RegisteredPlane.RemoveAt(i);
                break;
            }
        }

        RefreshPlaneList();
        ConfirmDelete = false;
    }

    [RelayCommand]
    private void Cancel()
    {
        ConfirmDelete = false;
        IsCreating = false;
        RefreshPlaneList();
    }

    [RelayCommand]
    private void ConfirmCreate()
    {
        if (!string.IsNullOrEmpty(UpName) && !string.IsNullOrEmpty(UpCompanie) && !string.IsNullOrEmpty(UpPointA) && !string.IsNullOrEmpty(UpPointB))
        {
            List<Classes> newClasses = new List<Classes>
            {
                new Classes("economique", 200),
                new Classes("VIP", 20)
            };

            RegisteredPlane.Add(new Plane(UpName, "p55", "220", UpPointA, UpPointB, newClasses, UpCompanie));
            RefreshPlaneList();
            IsCreating = false;
        }
    }

    [RelayCommand]
    private void ConfirmModify()
    {
        if (string.IsNullOrEmpty(SelectedPlaneId)) return;

        for (int i = 0; i < RegisteredPlane.Count; i++)
        {
            if (RegisteredPlane[i].Id == SelectedPlaneId)
            {
                var p = RegisteredPlane[i];
                // update fields only when provided to preserve existing values
                if (!string.IsNullOrEmpty(UpName)) p.Name = UpName;
                if (!string.IsNullOrEmpty(UpCompanie)) p.Companie = UpCompanie;
                if (!string.IsNullOrEmpty(UpPointA)) p.PointA = UpPointA;
                if (!string.IsNullOrEmpty(UpPointB)) p.PointB = UpPointB;

                // refresh Classlist for the updated plane
                if (Classlist != null)
                {
                    Classlist.Clear();
                    foreach (Classes c in p.PClasses)
                    {
                        Classlist.Add(new TextBlock { Text = c.CName + " :" + c.Places });
                    }
                }

                // keep SelectedPlaneName in sync if name changed
                SelectedPlaneName = p.Name;
                break;
            }
        }

        RefreshPlaneList();

        // update ViewPlane to reflect changes
        for (int i = 0; i < RegisteredPlane.Count; i++)
        {
            if (RegisteredPlane[i].Id == SelectedPlaneId)
            {
                var q = RegisteredPlane[i];
                ViewPlane = new PlaneStatusViewModel(false, Classlist, q.TotalPlace, q.Name, q.Id, q.PointA, q.PointB, q.Companie, DateTime.Now.ToString("yyyy-MM-dd"), delete: OnPlaneDelete);
                break;
            }
        }
    }
}
