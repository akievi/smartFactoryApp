using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Drawing;
using smartFactoryApp.Models;
using static smartFactoryApp.Models.Parameters;
using static smartFactoryApp.Models.ViewParameters;
using System.Threading.Tasks;
using System.Collections.Generic;
using Avalonia.Threading;
using System;
using System.Collections.ObjectModel;
namespace smartFactoryApp.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    Factory factory;
    DispatcherTimer timer;
    public ObservableCollection<LegoBrickViewModel> RedBricks { get; } = new ObservableCollection<LegoBrickViewModel>();
    public ObservableCollection<LegoBrickViewModel> BlueBricks { get; } = new ObservableCollection<LegoBrickViewModel>();
    public ObservableCollection<LegoBrickViewModel> YellowBricks { get; } = new ObservableCollection<LegoBrickViewModel>();
    public ObservableCollection<LegoBrickViewModel> GreenBricks { get; } = new ObservableCollection<LegoBrickViewModel>();
    public int ProductionRedX => factory.ProductionRedX;
    public int ProductionRedY => factory.ProductionRedY;
    public double ProductionRedAngle => factory.ProductionRedAngle;
    public int ProductionBlueX => factory.ProductionBlueX;
    public int ProductionBlueY => factory.ProductionBlueY;
    public double ProductionBlueAngle => factory.ProductionBlueAngle;
    public int ProductionYellowX => factory.ProductionYellowX;
    public int ProductionYellowY => factory.ProductionYellowY;
    public double ProductionYellowAngle => factory.ProductionYellowAngle;
    public int ProductionGreenX => factory.ProductionGreenX;
    public int ProductionGreenY => factory.ProductionGreenY;
    public double ProductionGreenAngle => factory.ProductionGreenAngle;
    public double RobotX => factory.RobotX;
    public double RobotY => factory.RobotY;
    public double RobotRotationAngle => factory.RobotRotationAngle;
    public MainViewModel()
    {
        factory = new Factory();
        foreach (var brick in factory.RedBricks)
        {
            RedBricks.Add(new LegoBrickViewModel(brick));
        }
        foreach (var brick in factory.BlueBricks)
        {
            BlueBricks.Add(new LegoBrickViewModel(brick));
        }
        foreach (var brick in factory.GreenBricks)
        {
            GreenBricks.Add(new LegoBrickViewModel(brick));
        }
        foreach (var brick in factory.YellowBricks)
        {
            YellowBricks.Add(new LegoBrickViewModel(brick));
        }
        timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromMilliseconds(timerInterval);
        timer.Tick += Timer_Tick;
    }
    private void Timer_Tick(object? sender, EventArgs e)
    {
        factory.LegoBricksMove();
        BricksPoseChanged();
    }
    public void StartTimer()
    {
        timer.Start();
    }
    public void StopTimer()
    {
        timer.Stop();
    }
    public void MoveUp()
    {
        factory.RobotMove(Direction.up);
        RobotPoseChanged();
    }
    public void MoveDown()
    {
        factory.RobotMove(Direction.down);
        RobotPoseChanged();
    }
    public void MoveLeft()
    {
        factory.RobotMove(Direction.left);
        RobotPoseChanged();
    }
    public void MoveRight()
    {
        factory.RobotMove(Direction.right);
        RobotPoseChanged();
    }
    public void RotateRight()
    {
        factory.RobotRotate(Parameters.Direction.clockwise);
        RobotPoseChanged();
    }
    public void RotateLeft()
    {
        factory.RobotRotate(Parameters.Direction.counterclockwise);
        RobotPoseChanged();
    }
    public void ResetRotation()
    {
        factory.RobotResetRotation();
        RobotPoseChanged();
    }
    private void BricksPoseChanged()
    {
        foreach (var brick in RedBricks) brick.Refresh();
        foreach (var brick in BlueBricks) brick.Refresh();
        foreach (var brick in YellowBricks) brick.Refresh();
        foreach (var brick in GreenBricks) brick.Refresh();
    }
    private void RobotPoseChanged()
    {
        OnPropertyChanged(nameof(RobotX));
        OnPropertyChanged(nameof(RobotY));
        OnPropertyChanged(nameof(RobotRotationAngle));
    }
}
