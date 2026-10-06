using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Drawing;
using smartFactoryApp.Models;
using static smartFactoryApp.Models.Parameters;

namespace smartFactoryApp.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    Robot robot;
    public double RobotX => robot.Location.X;
    public double RobotY => robot.Location.Y;
    public int RobotRotationAngle => robot.RotationAngle;
    public MainViewModel()
    {
        Point point = new Point(0, 0);
        Rectangle borders = new Rectangle(0, 0, 900, 600);
        robot = new Robot(point, borders);
    }
    public void MoveUp()
    {
        robot.Move(Direction.up);
        OnPropertyChanged(nameof(RobotX));
        OnPropertyChanged(nameof(RobotY));
    }
    public void MoveDown()
    {
        robot.Move(Direction.down);
        OnPropertyChanged(nameof(RobotX));
        OnPropertyChanged(nameof(RobotY));
    }
    public void MoveLeft()
    {
        robot.Move(Direction.left);
        OnPropertyChanged(nameof(RobotX));
        OnPropertyChanged(nameof(RobotY));
    }
    public void MoveRight()
    {
        robot.Move(Direction.right);
        OnPropertyChanged(nameof(RobotX));
        OnPropertyChanged(nameof(RobotY));
    }
    public void MoveUpLeft()
    {
        robot.Move(Direction.up);
        robot.Move(Direction.left);
        OnPropertyChanged(nameof(RobotX));
        OnPropertyChanged(nameof(RobotY));
    }
    public void MoveUpRight()
    {
        robot.Move(Direction.up);
        robot.Move(Direction.right);
        OnPropertyChanged(nameof(RobotX));
        OnPropertyChanged(nameof(RobotY));
    }
    public void MoveDownLeft()
    {
        robot.Move(Direction.down);
        robot.Move(Direction.left);
        OnPropertyChanged(nameof(RobotX));
        OnPropertyChanged(nameof(RobotY));
    }
    public void MoveDownRight()
    {
        robot.Move(Direction.down);
        robot.Move(Direction.right);
        OnPropertyChanged(nameof(RobotX));
        OnPropertyChanged(nameof(RobotY));
    }
    public void RotateRight()
    {
        robot.Rotate(Parameters.RotationDirection.clockwise);
        OnPropertyChanged(nameof(RobotRotationAngle));
    }
    public void RotateLeft()
    {
        robot.Rotate(Parameters.RotationDirection.counterclockwise);
        OnPropertyChanged(nameof(RobotRotationAngle));
    }
}
