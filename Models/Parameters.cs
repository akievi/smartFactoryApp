namespace smartFactoryApp.Models;

internal class Parameters
{
    public const int step = 3;
    public const int degreeStep = 5;
    public enum Direction
    {
        stop = 0,
        up = 1,
        down = 2,
        left = 3,
        right = 4
    }
    public enum RotationDirection
    {
        clockwise = 1,
        counterclockwise = 2
    }
}