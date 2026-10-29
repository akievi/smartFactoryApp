using System;
using System.Drawing;
using System.Reflection.Metadata;
using static smartFactoryApp.Models.Parameters;

namespace smartFactoryApp.Models;

public class LegoBrick : Beweger
{
    // private LegoColor color;
    // private LegoSize size;
    private int recyclePlatz = -1;          // -1 = hat keinen Recycle-Platz
    private Point recyclePose;
    public int RecyclePlatz => recyclePlatz;
    private int brickWidth = 20;
    private Point originalLocation;
    private int versatz = 0;
    private BrickStatus status = BrickStatus.onProductionLine;   // jeder neue Stein liegt erstmal an seinem Band
    public BrickStatus Status { get => status; set => status = value; }
    private int deliveryRight = ViewParameters.DeliveryZone.Right;
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
        if (Status == BrickStatus.docked)
        {
            return;
        }
        if (Status == BrickStatus.onRecycleLine)
        {
            return;   // repariert auf dem Recycle-Platz: liegen bleiben, bis der Roboter ihn wieder greift
        }
        if (Status == BrickStatus.toCustomer)
        {
            if (Location.X - (brickWidth/2) <= deliveryRight)
            {
                base.Move(Parameters.Direction.right);
            }
            else if (versatz == 0)
            {
                Location = originalLocation;
                Status = BrickStatus.onProductionLine;
            }   
        }
        else if (Status == BrickStatus.toRecycling)
        {
            // solange der Stein noch rechts vom linken Ende des Recycling-Pfeils ist: weiter nach links
            if (Location.X > ViewParameters.ToRecycleLinePosition.X)
            {
                base.Move(Parameters.Direction.left);
            }
            else
            {
                // am Ende des Pfeils angekommen: Stein liegt repariert auf seinem Recycle-Platz
                Location = recyclePose;
                Status = BrickStatus.onRecycleLine;
            }
        }
        else
        {
            base.Move(direction);
        }
    }
    public void ZumRecycling(int platz)
    {
        recyclePlatz = platz;
        recyclePose = ViewParameters.RecyclePlatzPosition(platz);
        Status = BrickStatus.toRecycling;
    }
    public override string ToString()
    {
        return $"{Color} {Size} ({Location.X}, {Location.Y})";
    }
}