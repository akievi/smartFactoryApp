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
}