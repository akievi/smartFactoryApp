using System;
using System.Drawing;
using System.Reflection.Metadata;
using static smartFactoryApp.Models.Parameters;

namespace smartFactoryApp.Models;

public class LegoBrick : Beweger
{
    // private LegoColor color;
    // private LegoSize size;
    public bool gedockt = false;
    public bool abgeliefert = false;
    public bool Abgeliefert { get => abgeliefert; set => abgeliefert = value;}
    private int brickWidth = 20;
    private Point originalLocation;
    private int versatz = 0;
    public bool Gedockt { get => gedockt; set => gedockt = value;}
    private int deliveryRight = ViewParameters.TestRectangle.X + ViewParameters.TestRectangleWidth;
    public LegoColor Color { get; } // Wenn (using static smartFactoryApp.Models.Parameters;) nicht da ist,
    //  dann muss man hier schreiben: public Parameters.LegoColor Color { get; }
    public LegoSize Size { get; } // hier wird auch implizit size und color als private Variablen angelegt,
    //  die nur über die Properties Color und Size gelesen werden koennen
    public LegoBrick(Pose pose, LegoColor color, LegoSize size) : base(pose, ViewParameters.Borders)
    {
        Color = color;
        Size = size;
        originalLocation = pose.Location;
    }
    public override void Move(Direction direction)
    {
        if (direction == Parameters.Direction.right) versatz++;
        else if (direction == Parameters.Direction.left) versatz--;
        else if (direction == Parameters.Direction.up) versatz++;
        else if (direction == Parameters.Direction.down) versatz--;     
        if (Gedockt)
        {
            return;
        }
        if (Abgeliefert)
        {
            if (Location.X - (brickWidth/2) <= deliveryRight)
            {
                base.Move(Parameters.Direction.right);
            }
            else if (versatz == 0)
            {
                Location = originalLocation;
                abgeliefert = false;
            }   
        }
        else
        {
            base.Move(direction);
        }
    }
    public override string ToString()
    {
        return $"{Color} {Size} ({Location.X}, {Location.Y})";
    }
}