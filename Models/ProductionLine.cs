using System.Collections.Generic;
using System.Drawing;
using System;

namespace smartFactoryApp.Models;
public class ProductionLine
{
    private Pose pose;
    private Parameters.LegoColor color;
    private List<LegoBrick> bricks = new List<LegoBrick>();
    public List<LegoBrick> Bricks{  get => bricks; set => bricks = value;}
    private int tickCount = 0;
    private const int stepsPerDirection = 5;
    public Point Location
    {
        get { return this.pose.Location; }
    }
    public double RotationAngle
    {
        get { return this.pose.Angle; }
    }
    public Parameters.LegoColor Color
    {
        get { return this.color; }
    }
    public ProductionLine(Pose pose, Parameters.LegoColor color)
    {
        this.pose = pose;
        this.color = color;
        CreateBricks();
    }
    // Band ohne Steine (z.B. Recycle- und Delivery-Band): Bricks bleibt leer, move() tut damit nichts
    public ProductionLine(Pose pose)
    {
        this.pose = pose;
    }
    private void CreateBricks()
    {
        // Mittelpunkt des Bands
        int cx = pose.X + ViewParameters.productionLineWidth / 2;
        int cy = pose.Y + ViewParameters.productionLineHeight / 2;

        // Mittelpunkt der Fabrikflaeche
        int mitteX = ViewParameters.FactoryArea.X + ViewParameters.FactoryArea.Width / 2;
        int mitteY = ViewParameters.FactoryArea.Y + ViewParameters.FactoryArea.Height / 2;

        // Baender liegen entweder vertikal (90°) oder horizontal (0°).
        // entlangX/entlangY: Richtung, in der die Steine auf dem Band verteilt werden
        // seiteX/seiteY:     Richtung, in der die Steine neben dem Band liegen (zur Fabrikmitte hin)
        int entlangX;
        int entlangY;
        int seiteX;
        int seiteY;
        if (IstVertikal())
        {
            entlangX = 0;
            entlangY = 1;
            seiteY = 0;
            if (cx < mitteX)
            {
                seiteX = 1;   // Band links der Mitte, Steine rechts daneben
            }
            else
            {
                seiteX = -1;  // Band rechts der Mitte, Steine links daneben
            }
        }
        else
        {
            entlangX = 1;
            entlangY = 0;
            seiteX = 0;
            if (cy < mitteY)
            {
                seiteY = 1;   // Band oberhalb der Mitte, Steine darunter
            }
            else
            {
                seiteY = -1;  // Band unterhalb der Mitte, Steine darueber
            }
        }

        for (int i = 0; i < Parameters.bricksPerLine; i++)
        {
            // Position auf dem Band: -3, -1, 1, 3 (symmetrisch um die Mitte)
            int platz = 2 * i - (Parameters.bricksPerLine - 1);

            // alle Steine sind klein (12x12)
            Parameters.LegoSize size = Parameters.LegoSize.small;
            int halbeBreite = ViewParameters.BrickSize / 2;
            int halbeHoehe = ViewParameters.BrickSize / 2;

            // Steinmitte: Bandmitte + entlang des Bands (22 px Abstand, also 11 pro Platz-Einheit)
            //             + 19 px quer zur Fabrikmitte (halbe Bandbreite 11 + 2 Luft + ca. halber Stein)
            int mitteSteinX = cx + platz * 11 * entlangX + 19 * seiteX;
            int mitteSteinY = cy + platz * 11 * entlangY + 19 * seiteY;

            // Pose braucht die linke obere Ecke, also halbe Steingroesse abziehen
            int x = mitteSteinX - halbeBreite;
            int y = mitteSteinY - halbeHoehe;

            bricks.Add(new LegoBrick(new Pose(x, y, pose.Angle), color, size));
        }
    }
    public void move()
    {
        bool movingDirection = tickCount < stepsPerDirection;
        tickCount = (tickCount + 1) % (2 * stepsPerDirection);

        // Mittelpunkt des Bands und der Fabrikflaeche
        int cx = pose.X + ViewParameters.productionLineWidth / 2;
        int cy = pose.Y + ViewParameters.productionLineHeight / 2;
        int mitteX = ViewParameters.FactoryArea.X + ViewParameters.FactoryArea.Width / 2;
        int mitteY = ViewParameters.FactoryArea.Y + ViewParameters.FactoryArea.Height / 2;

        // "Vorwaerts" heisst bei allen Baendern: im Uhrzeigersinn um die Fabrikmitte.
        // Vertikales Band: links der Mitte nach oben, rechts der Mitte nach unten.
        // Horizontales Band: oberhalb der Mitte nach rechts, unterhalb nach links.
        Parameters.Direction vor;
        Parameters.Direction zurueck;
        if (IstVertikal())
        {
            if (cx < mitteX)
            {
                vor = Parameters.Direction.up;
                zurueck = Parameters.Direction.down;
            }
            else
            {
                vor = Parameters.Direction.down;
                zurueck = Parameters.Direction.up;
            }
        }
        else
        {
            if (cy < mitteY)
            {
                vor = Parameters.Direction.right;
                zurueck = Parameters.Direction.left;
            }
            else
            {
                vor = Parameters.Direction.left;
                zurueck = Parameters.Direction.right;
            }
        }

        foreach (LegoBrick brick in bricks)
        {
            if (movingDirection)
            {
                brick.Move(vor);
            }
            else
            {
                brick.Move(zurueck);
            }
        }
    }
    // Ein Band gilt als vertikal, wenn es um 90° (oder -90°) gedreht ist, sonst als horizontal
    private bool IstVertikal()
    {
        if (Math.Abs(pose.Angle) == 90)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}