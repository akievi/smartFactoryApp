namespace smartFactoryApp.Models;

public class Parameters
{
    public const int step = 3;
    public const int degreeStep = 5;
    public const int bricksPerLine = 4;
    public const int timerInterval = 50; //in ms
    public const int robotTimerInterval = 150; //in ms, eigener Takt fuer die kontinuierliche Roboterbewegung
    public enum Direction
    {
        up = 0,
        down = 1,
        left = 2,
        right = 3,
        clockwise = 4,
        counterclockwise = 5
    }
    public enum LegoColor
    {
        yellow = 0,
        blue = 1,
        red = 2,
        green = 3
    }
    public enum LegoSize
    {
        large = 0,
        small = 1
    }
    public enum BrickStatus
    {
        onProductionLine,   // liegt an seinem Farbband und faehrt dort hin und her
        docked,             // haengt am Greifer
        toCustomer,         // faehrt nach rechts zum Kunden
        toRecycling,        // faehrt nach links zum Recycling
        onRecycleLine,      // liegt repariert am Recycle-Band und kann neu gegriffen werden
        delivered           // beim Kunden angekommen, nicht mehr im Spiel
    }
    public enum UndockResult
    {
        delivered,          // ok, Stein geht zum Kunden
        defect,             // ok abgelegt, aber defekt, geht zum Recycling
        noBrickDocked,      // Roboter haelt nichts
        notOnDeliveryLine,  // Stein ist nicht ueber der Delivery-Zone
        blocked             // Roboter koennte nach dem Ablegen nicht wegfahren (Kollision)
    }
}