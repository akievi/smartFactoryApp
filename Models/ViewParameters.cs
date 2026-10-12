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
    // graue Fabrikflaeche im factory_plan.png (x/y 38 bis 562)
    // muss vor RobotPose stehen, da statische Felder in Reihenfolge initialisiert werden
    public static Rectangle FactoryArea = new Rectangle(38, 38, 525, 525);
    // Roboter (70x50) mittig in der Fabrikflaeche
    public static Pose RobotPose = new Pose(FactoryArea.X + (FactoryArea.Width - 70) / 2, FactoryArea.Y + (FactoryArea.Height - 50) / 2, 0);
    public static Pose brickPose = new Pose(400, 250, 45);
    // Grenzen beziehen sich auf die linke obere Ecke, daher um die Objektgroesse verkleinert
    public static Rectangle RobotBorders = new Rectangle(FactoryArea.X, FactoryArea.Y, FactoryArea.Width - 70, FactoryArea.Height - 50);
    public static Rectangle Borders = new Rectangle(FactoryArea.X, FactoryArea.Y, FactoryArea.Width - 12, FactoryArea.Height - 12);
    public const int productionLineWidth = 90;
    public const int productionLineHeight = 22;
    public static Pose ProdRedPose    = new Pose(75, 107, -45);
    public static Pose ProdBluePose   = new Pose(440, 107, 45);
    public static Pose ProdYellowPose = new Pose(75, 470, 45);
    public static Pose ProdGreenPose  = new Pose(440, 470, -45);

}
