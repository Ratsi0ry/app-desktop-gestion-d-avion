
using System;
using Avalonia.Media.Imaging;
using Avalonia.Platform;               
using CommunityToolkit.Mvvm.ComponentModel;

namespace Gestion_avion.ViewModels;

public partial class PlaneStatusViewModel : ViewModelBase
{
    [ObservableProperty]
    public Bitmap? _modelImage;
    [ObservableProperty]
    public string? _planeName, _planeId, _owner, _model,_viewDate, _pointA, _pointB, _imageSourcePath;

    [ObservableProperty]
    public bool _isDefault = false;

    public PlaneStatusViewModel(bool _default_, string model = "",string name = "", string id = "", string? depart = "", string? arrivee = "",string Owner = "", string view = "")
    {
        _planeName = name;
        _planeId = id;
        _owner = Owner;
        _pointA = depart;
        _pointB = arrivee;
        _isDefault = _default_;
        _viewDate = view;
        _model = model;
        ImageSourcePath = "avares://Gestion_avion/Assets/Airbus350.png";
        switch (model)
        {
            case "Airbus350":
                ImageSourcePath = "avares://Gestion_avion/Assets/Airbus350.png";
                break;
            case "AirbusA230Neo":
                ImageSourcePath = "avares://Gestion_avion/Assets/AirbusA230Neo.png";
                break;
            case "AirbusA320":
                ImageSourcePath = "avares://Gestion_avion/Assets/AirbusA320.png";
                break;
            case "Boeign787Dreamliner":
                ImageSourcePath = "avares://Gestion_avion/Assets/Boeing787Dreamliner.jpg";
                break;
            case "Boeing737Max":
                ImageSourcePath = "avares://Gestion_avion/Assets/Boeing737Max.png";
                break;
            default:
                ImageSourcePath = "avares://Gestion_avion/Assets/plane-profile-r.png";
                break;
        }

        try
        {
            // 3. Load the asset stream safely into a real Bitmap object
            var uri = new Uri(ImageSourcePath);
            ModelImage = new Bitmap(AssetLoader.Open(uri));
        }
        catch (Exception)
        {
            ModelImage = null; 
        }
    }
    
}