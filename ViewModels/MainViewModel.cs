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
namespace smartFactoryApp.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    Factory factory;
    DispatcherTimer timer;
    public List<LegoBricksViewModel> BricksView { get; } = new List<LegoBricksViewModel>();
    public double RobotX => factory.RobotX;
    public double RobotY => factory.RobotY;
    public double RobotRotationAngle => factory.RobotRotationAngle;
    public MainViewModel()
    {
        factory = new Factory();
        foreach (var brick in factory.Bricks)
            BricksView.Add(new LegoBricksViewModel(brick));
        timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromMilliseconds(timerInterval);
        timer.Tick += Timer_Tick;
        timer.Start();
    }
    private void Timer_Tick(object? sender, EventArgs e)
    {
        factory.LegoBricksMove();
        BricksPoseChanged();
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
        foreach (var brick in BricksView)
        {
            brick.Refresh();
        }
    }
    private void RobotPoseChanged()
    {
        OnPropertyChanged(nameof(RobotX));
        OnPropertyChanged(nameof(RobotY));
        OnPropertyChanged(nameof(RobotRotationAngle));
    }
}
