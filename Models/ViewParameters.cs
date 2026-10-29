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
    // Groesse des Roboters, passend zu robot.png und dem Rechteck im XAML (70x50)
    public const int RobotWidth = 70;
    public const int RobotHeight = 50;
    // Lage des Greifers im Bild robot.png, gemessen von der linken oberen Ecke (gilt fuer den ungedrehten Roboter)
    public const int GreiferOffsetX = 63;
    public const int GreiferOffsetY = 15;
    // Kantenlaenge eines (kleinen) Lego-Steins
    public const int BrickSize = 12;
    // Roboter horizontal mittig, vertikal zwischen Delivery-Band (oben) und Recycle-Band (Mitte), Robotermitte bei y = 180
    public static Pose RobotPose = new Pose(FactoryArea.X + (FactoryArea.Width - RobotWidth) / 2, 180 - RobotHeight / 2, 0);
    public static Pose brickPose = new Pose(400, 250, 45);
    // Grenzen beziehen sich auf die linke obere Ecke, daher um die Objektgroesse verkleinert
    public static Rectangle RobotBorders = new Rectangle(FactoryArea.X, FactoryArea.Y, FactoryArea.Width - RobotWidth, FactoryArea.Height - RobotHeight);
    public static Rectangle Borders = new Rectangle(FactoryArea.X, FactoryArea.Y, FactoryArea.Width - 12, FactoryArea.Height - 12);
    public const int productionLineWidth = 90;
    public const int productionLineHeight = 22;
    // Pose = linke obere Ecke des UNgedrehten Bands (90x22), gedreht wird um die Bandmitte.
    // Ecke = Bandmitte minus (45, 11).
    // Rot/Blau: vertikal (90°) links/rechts auf halber Hoehe, Mitten bei (151, 254) und (447, 254)
    // Gelb/Gruen: horizontal (0°) unten, Mitten bei (161, 444) und (437, 444)
    public static Pose ProdRedPose    = new Pose(106, 243, 90);
    public static Pose ProdBluePose   = new Pose(402, 243, 90);
    public static Pose ProdYellowPose = new Pose(116, 433, 0);
    public static Pose ProdGreenPose  = new Pose(392, 433, 0);

    // Zusatzbaender ohne Steine, beide horizontal:
    // Recycle in der Fabrikmitte (299, 299), Delivery oben mittig (299, 64)
    public static Pose ProdRecyclePose  = new Pose(254, 288, 0);
    public static Pose ProdDeliveryPose = new Pose(206, 49, 0);
    // gesamtes Delivery-Band: ueberall darf abgelegt werden, jede Ablage zaehlt als Delivery
    // (muss unter ProdDeliveryPose stehen, statische Felder werden in Reihenfolge initialisiert)
    public static Rectangle DeliveryZone = new Rectangle(ProdDeliveryPose.X, ProdDeliveryPose.Y, ProductionDeliveryWidth, ProductionDeliveryHeight);
    // Delivery-Band mit drei Zonen (Recycle | Delivery | Customer), Mitte bleibt bei (299, 64)
    public const int ProductionDeliveryWidth = 186;
    public const int ProductionDeliveryHeight = 30;
    // vier Ablageplaetze fuer reparierte Steine, oben auf dem Recycle-Band nebeneinander
    public const int RecyclePlaetze = 4;
    public static Point RecyclePlatzPosition(int platz)
    {
        return new Point(ProdRecyclePose.X + 6 + platz * 22, ProdRecyclePose.Y - 14);
    }

    // Pfeile links und rechts vom Delivery-Band
    public const int ToRecycleLineWidth = 110;
    public const int ToRecycleLineHeight = 40;
    public static Point ToRecycleLinePosition = new Point(92, 44);
    public const int ToCustomerLineWidth = 110;
    public const int ToCustomerLineHeight = 40;
    public static Point ToCustomerLinePosition = new Point(396, 44);
}
