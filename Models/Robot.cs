using System;
using System.Drawing;
namespace smartFactoryApp.Models;

public class Robot : Beweger
{
    public Robot(Point location, Rectangle borders) : base(location, borders)
    {
    }
    // Bewegung relativ zur Ausrichtung des Roboters: "up" heisst vorwaerts in Blickrichtung,
    // nicht nach oben auf dem Bildschirm
    public override void Move(Parameters.Direction direction)
    {
        double rad = RotationAngle * Math.PI / 180;

        // Vorwaerts-Richtung: bei 0° nach oben (y wird kleiner)
        double relativeUpX = Math.Sin(rad);
        double relativeUpY = -Math.Cos(rad);
        // Rechts-Richtung: 90° im Uhrzeigersinn zu vorwaerts
        double relativeRightX = Math.Cos(rad);
        double relativeRightY = Math.Sin(rad);

        double dx;
        double dy;
        if (direction == Parameters.Direction.up)
        {
            dx = relativeUpX;
            dy = relativeUpY;
        }
        else if (direction == Parameters.Direction.down)
        {
            dx = -relativeUpX;
            dy = -relativeUpY;
        }
        else if (direction == Parameters.Direction.right)
        {
            dx = relativeRightX;
            dy = relativeRightY;
        }
        else if (direction == Parameters.Direction.left)
        {
            dx = -relativeRightX;
            dy = -relativeRightY;
        }
        else
        {
            return; // Drehrichtungen werden ueber Rotate behandelt, nicht hier
        }

        // Schritt in Pixeln, gerundet, weil Location ganze Zahlen speichert
        int neuX = Location.X + (int)Math.Round(dx * Parameters.step);
        int neuY = Location.Y + (int)Math.Round(dy * Parameters.step);

        // jede Achse einzeln gegen die Grenzen pruefen, damit der Roboter an der Wand entlang rutschen kann
        if (neuX < Borders.Left || neuX > Borders.Right)
        {
            neuX = Location.X;
        }
        if (neuY < Borders.Top || neuY > Borders.Bottom)
        {
            neuY = Location.Y;
        }
        Location = new Point(neuX, neuY);
    }
}