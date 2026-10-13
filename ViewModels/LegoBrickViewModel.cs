using System.Security.Cryptography.X509Certificates;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using smartFactoryApp.Models;

namespace smartFactoryApp.ViewModels;

public partial class LegoBrickViewModel : ViewModelBase
{
    private LegoBrick _brick;
    [ObservableProperty] private double x; // weitere Kurzschreibweise für Properties, die automatisch OnPropertyChanged aufrufen
    [ObservableProperty] private double y;
    [ObservableProperty] private double angle;
    // Groesse des Steins in Pixeln, haengt nur von brick.Size ab und aendert sich nie (daher kein ObservableProperty)
    public int Width { get; }
    public int Height { get; }
    public LegoBrickViewModel(LegoBrick brick)
    {
        _brick = brick;
        if (brick.Size == Parameters.LegoSize.large)
        {
            Width = 20;
            Height = 14;
        }
        else
        {
            Width = 12;
            Height = 12;
        }
        Refresh();
    }
    public void Refresh()
    {
        X = _brick.Location.X;
        Y = _brick.Location.Y;
        Angle = _brick.RotationAngle;
    }
}