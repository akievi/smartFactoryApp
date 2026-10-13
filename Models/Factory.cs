using System.Drawing;
using System.Collections.Generic;
using smartFactoryApp.Models;
using static smartFactoryApp.Models.Parameters;
using static smartFactoryApp.Models.ViewParameters;

namespace smartFactoryApp.Models;

public class Factory
{
    private Robot robot;
    public double RobotX => robot.Location.X;
    public double RobotY => robot.Location.Y;
    public double RobotRotationAngle => robot.RotationAngle;
    //public Robot Robot => robot;
    private ProductionLine productionRed = new ProductionLine(ProdRedPose, Parameters.LegoColor.red);
    private ProductionLine productionBlue = new ProductionLine(ProdBluePose, Parameters.LegoColor.blue);
    private ProductionLine productionYellow = new ProductionLine(ProdYellowPose, Parameters.LegoColor.yellow);
    private ProductionLine productionGreen = new ProductionLine(ProdGreenPose, Parameters.LegoColor.green);
    public List<LegoBrick> RedBricks => productionRed.Bricks;
    public List<LegoBrick> BlueBricks => productionBlue.Bricks;
    public List<LegoBrick> YellowBricks => productionYellow.Bricks;
    public List<LegoBrick> GreenBricks => productionGreen.Bricks;
    public int ProductionRedX => productionRed.Location.X;
    public int ProductionRedY => productionRed.Location.Y;
    public double ProductionRedAngle => productionRed.RotationAngle;
    public int ProductionBlueX => productionBlue.Location.X;
    public int ProductionBlueY => productionBlue.Location.Y;
    public double ProductionBlueAngle => productionBlue.RotationAngle;
    public int ProductionYellowX => productionYellow.Location.X;
    public int ProductionYellowY => productionYellow.Location.Y;
    public double ProductionYellowAngle => productionYellow.RotationAngle;
    public int ProductionGreenX => productionGreen.Location.X;
    public int ProductionGreenY => productionGreen.Location.Y;
    public double ProductionGreenAngle => productionGreen.RotationAngle;
    public Factory()
    {
        robot = new Robot(RobotPose.Location, RobotBorders);
        //robot = new Robot(new Point(450, 300), new Rectangle(10, 10, 590, 890));
    }
    public void RobotMove(Direction direction)
    {
        robot.Move(direction);
    }
    public void RobotRotate(Direction direction)
    {
        robot.Rotate(direction);
    }
    public void RobotResetRotation()
    {
        robot.ResetRotation();
    }
    public void LegoBricksMove()
    {
        productionRed.move();
        productionBlue.move();
        productionYellow.move();
        productionGreen.move();
    }
}