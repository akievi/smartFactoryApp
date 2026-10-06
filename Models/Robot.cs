using System.Drawing;
namespace smartFactoryApp.Models;

internal class Robot
{
    private Point location;
    private Rectangle borders;
    private int rotationAngle = 0;
    public Point Location
    {
        get { return location; }
        set { location = value; }
    }

    public Rectangle Borders { get => borders; set => borders = value; } //verkuerzte Syntax fuer Properties

    public Robot(Point location, Rectangle borders)
    {
        this.location = location;
        this.borders = borders;
        //Location = location;
        //Borders = borders;
        //hier unnoetige Nutzung der Properties, da die privaten Parameter direkt angesprochen werden koennen
    }
    public Robot(Point location)
    {
        this.location = location;
        //Location = location;
    }
    public int RotationAngle
    {
        get { return rotationAngle; }
        set { rotationAngle = value; }
    }
    public void Move(Parameters.Direction direction)
    {
        switch (direction)
        {
            case Parameters.Direction.up:
                if (location.Y - Parameters.step >= borders.Top)
                    location.Y -= Parameters.step;
                break;
            case Parameters.Direction.down:
                if (location.Y + Parameters.step <= borders.Bottom)
                    location.Y += Parameters.step;
                break;
            case Parameters.Direction.left:
                if (location.X - Parameters.step >= borders.Left)
                    location.X -= Parameters.step;
                break;
            case Parameters.Direction.right:
                if (location.X + Parameters.step <= borders.Right)
                    location.X += Parameters.step;
                break;
            default:
                break;
        }
    }
    public void Rotate(Parameters.RotationDirection rotationDirection) 
    {
        if (rotationDirection == Parameters.RotationDirection.clockwise)
        {
            rotationAngle += Parameters.degreeStep;
            rotationAngle = (rotationAngle % 360 + 360) % 360;
        }
        else if (rotationDirection == Parameters.RotationDirection.counterclockwise)
        {
            rotationAngle -= Parameters.degreeStep;
            rotationAngle = (rotationAngle % 360 + 360) % 360;
        }
    }
}