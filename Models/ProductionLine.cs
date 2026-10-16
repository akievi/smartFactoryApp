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
    public List<LegoBrick> Bricks
    {
        get { return this.bricks; }
    }
    public ProductionLine(Pose pose, Parameters.LegoColor color)
    {
        this.pose = pose;
        this.color = color;
        CreateBricks();
    }
    private void CreateBricks()
    {
        // Mittelpunkt des Bands
        int cx = pose.X + ViewParameters.productionLineWidth / 2;
        int cy = pose.Y + ViewParameters.productionLineHeight / 2;

        // Mittelpunkt der Fabrikflaeche
        int mitteX = ViewParameters.FactoryArea.X + ViewParameters.FactoryArea.Width / 2;
        int mitteY = ViewParameters.FactoryArea.Y + ViewParameters.FactoryArea.Height / 2;

        // Laufrichtung des Bands: nach rechts geht es bei -45° nach oben, bei +45° nach unten
        int bandY;
        if (pose.Angle < 0)
        {
            bandY = -1;
        }
        else
        {
            bandY = 1;
        }

        // Seite neben dem Band, die zur Fabrikmitte zeigt
        int seiteX;
        if (cx < mitteX)
        {
            seiteX = 1;
        }
        else
        {
            seiteX = -1;
        }
        int seiteY;
        if (cy < mitteY)
        {
            seiteY = 1;
        }
        else
        {
            seiteY = -1;
        }

        for (int i = 0; i < Parameters.bricksPerLine; i++)
        {
            // Position auf dem Band: -3, -1, 1, 3 (symmetrisch um die Mitte)
            int platz = 2 * i - (Parameters.bricksPerLine - 1);

            // die ersten zwei Steine sind gross (20x14), die restlichen klein (12x12)
            Parameters.LegoSize size;
            int halbeBreite;
            int halbeHoehe;
            if (i < 2)
            {
                size = Parameters.LegoSize.large;
                halbeBreite = 10;
                halbeHoehe = 7;
            }
            else
            {
                size = Parameters.LegoSize.small;
                halbeBreite = 6;
                halbeHoehe = 6;
            }

            // Steinmitte: Bandmitte + entlang des Bands (15 px Abstand pro Achse) + 14 px quer zur Fabrikmitte
            int mitteSteinX = cx + platz * 15 / 2         + 14 * seiteX;
            int mitteSteinY = cy + platz * 15 / 2 * bandY + 14 * seiteY;

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
        // Oben laeuft das nach rechts, unten nach links, links nach oben, rechts nach unten.
        Parameters.Direction vorX;
        if (cy < mitteY)
        {
            vorX = Parameters.Direction.right;
        }
        else
        {
            vorX = Parameters.Direction.left;
        }
        Parameters.Direction vorY;
        if (cx < mitteX)
        {
            vorY = Parameters.Direction.up;
        }
        else
        {
            vorY = Parameters.Direction.down;
        }

        // Rueckwaerts ist jeweils die Gegenrichtung
        Parameters.Direction zurueckX;
        if (vorX == Parameters.Direction.right)
        {
            zurueckX = Parameters.Direction.left;
        }
        else
        {
            zurueckX = Parameters.Direction.right;
        }
        Parameters.Direction zurueckY;
        if (vorY == Parameters.Direction.up)
        {
            zurueckY = Parameters.Direction.down;
        }
        else
        {
            zurueckY = Parameters.Direction.up;
        }

        foreach (LegoBrick brick in bricks)
        {
            if (movingDirection)
            {
                brick.Move(vorX);
                brick.Move(vorY);
            }
            else
            {
                brick.Move(zurueckX);
                brick.Move(zurueckY);
            }
        }
    }
}