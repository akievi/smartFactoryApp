using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Drawing;
using System.Collections.Generic;
using smartFactoryApp.Models;
using static smartFactoryApp.Models.Parameters;
using static smartFactoryApp.Models.ViewParameters;
using System.Threading.Tasks;

namespace smartFactoryApp.ViewModels;

public class Factory
{
    private Robot robot;
    private List<LegoBrick> bricks = new List<LegoBrick>();
    public List<LegoBrick> Bricks { get { return bricks; } }
    public double RobotX => robot.Location.X;
    public double RobotY => robot.Location.Y;
    public double RobotRotationAngle => robot.RotationAngle;
    //public Robot Robot => robot;
    public Factory()
    {
        robot = new Robot(RobotPose.Location, Borders);
        //robot = new Robot(new Point(450, 300), new Rectangle(10, 10, 590, 890));
        CreateBricks();
    }
    public void CreateBricks()
    {
        LegoColor[] colors = { LegoColor.red, LegoColor.yellow, LegoColor.blue, LegoColor.green };
        for (int i = 0; i < Parameters.bricksPerLine; i++)
        {
            Pose pose = new Pose(brickPose.X + 50 * i, brickPose.Y, brickPose.Angle);
            bricks.Add(new LegoBrick(pose, colors[i % colors.Length], LegoSize.small));
        }
    }
    public void RobotMove(Direction direction)
    {
        robot.Move(direction);
    }
    public void RobotRotate(Direction direction)
    {
        robot.Rotate(direction);
    }
    int tickCount = 0;
    public void LegoBricksMove()
    {
        bool movingDirection = tickCount < 10;
        tickCount = (tickCount + 1) % 20;
        foreach (LegoBrick brick in bricks)
        {
            if (movingDirection)
            {
                brick.Move(direction: Direction.up);
                brick.Move(direction: Direction.left);
            }
            else
            {
                brick.Move(direction: Direction.down);
                brick.Move(direction: Direction.right);
            }
        }
    }
}