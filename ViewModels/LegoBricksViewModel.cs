using System.Security.Cryptography.X509Certificates;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using smartFactoryApp.Models;

namespace smartFactoryApp.ViewModels;

public partial class LegoBricksViewModel : ViewModelBase
{
    private LegoBrick _brick;
    [ObservableProperty] private double x; // weitere Kurzschreibweise für Properties, die automatisch OnPropertyChanged aufrufen
    [ObservableProperty] private double y; // weitere Kurzschreibweise für Properties, die automatisch OnPropertyChanged aufrufen
    [ObservableProperty] private double angle; // weitere Kurzschreibweise für Properties, die automatisch OnPropertyChanged aufrufen
    public LegoBricksViewModel(LegoBrick brick)
    {
        _brick = brick;
        Refresh();
    }
    public void Refresh()
    {
        X = _brick.Location.X;
        Y = _brick.Location.Y;
        Angle = _brick.RotationAngle;
    }
}