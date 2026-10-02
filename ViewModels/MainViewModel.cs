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
}
