using System.Drawing;

namespace smartFactoryApp.Models;

public struct Pose
{
    public int X { get; }
    public int Y { get; }
    public double Angle { get; }
    public Pose(int x, int y, double angle)
    {
        X = x;
        Y = y;
        Angle = angle;
    }
    public Point Location => new Point(X, Y);
}

public static class ViewParameters
{
    public static Pose RobotPose = new Pose(450, 300, 0);
    public static Pose brickPose = new Pose(400, 250, 45);
    public static Rectangle Borders = new Rectangle(360, 195, 447, 450);
}
