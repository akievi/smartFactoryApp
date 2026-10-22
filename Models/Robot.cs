using System;
using System.Drawing;
namespace smartFactoryApp.Models;

public class Robot : Beweger
{
    public Robot(Point location, Rectangle borders) : base(location, borders)
    {
    }
    // Prueft vorab, ob der Roboter 'schritte' Schritte nach links (relativ zu seiner Ausrichtung, also weg vom
    // Greifer, der rechts sitzt) fahren koennte, ohne an die Fabrikgrenze oder an ein Band zu stossen.
    // Bewegt den Roboter dabei NICHT, rechnet nur die Positionen durch.
    public bool NachLinksFahrenMoeglich(int schritte)
    {
        double rad = RotationAngle * Math.PI / 180;
        // links = Gegenrichtung von rechts (cos, sin), gerundet wie in Move
        int schrittX = (int)Math.Round(-Math.Cos(rad) * Parameters.step);
        int schrittY = (int)Math.Round(-Math.Sin(rad) * Parameters.step);

        for (int i = 1; i <= schritte; i++)
        {
            Point ziel = new Point(Location.X + i * schrittX, Location.Y + i * schrittY);
            if (!InnerhalbGrenzen(ziel, Borders))
            {
                return false;
            }
            if (KollidiertMitProductionLine(ziel, RotationAngle))
            {
                return false;
            }
        }
        return true;
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
        if (InnerhalbGrenzen(new Point(neuX, neuY), Borders))
        {
            
        }
        if (neuX < Borders.Left || neuX > Borders.Right)
        {
            neuX = Location.X;
        }
        if (neuY < Borders.Top || neuY > Borders.Bottom)
        {
            neuY = Location.Y;
        }
        // Kollision mit den Baendern: erst den ganzen Schritt versuchen, sonst nur x oder nur y,
        // damit der Roboter am Band entlang rutscht statt komplett stehen zu bleiben
        if (KollidiertMitProductionLine(new Point(neuX, neuY), RotationAngle))
        {
            if (!KollidiertMitProductionLine(new Point(neuX, Location.Y), RotationAngle))
            {
                neuY = Location.Y;
            }
            else if (!KollidiertMitProductionLine(new Point(Location.X, neuY), RotationAngle))
            {
                neuX = Location.X;
            }
            else
            {
                neuX = Location.X;
                neuY = Location.Y;
            }
        }
        Location = new Point(neuX, neuY);
        DockobjektNachfuehren(); // gegriffenen Stein mitnehmen
    }
}