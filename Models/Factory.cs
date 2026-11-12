using System.Drawing;
using System;
using System.Collections.Generic;
using smartFactoryApp.Models;
using static smartFactoryApp.Models.Parameters;
using static smartFactoryApp.Models.ViewParameters;

namespace smartFactoryApp.Models;

public class Factory
{
    private Robot robot;
    public double RobotX => robot.Location.X;
    public double RobotY => robot.Location.Y;
    public double RobotRotationAngle => robot.RotationAngle;
    private Random zufall = new Random();
    private bool naechsterZumRecycling = false;
    //public Robot Robot => robot;
    private ProductionLine productionRed = new ProductionLine(ProdRedPose, Parameters.LegoColor.red);
    private ProductionLine productionBlue = new ProductionLine(ProdBluePose, Parameters.LegoColor.blue);
    private ProductionLine productionYellow = new ProductionLine(ProdYellowPose, Parameters.LegoColor.yellow);
    private ProductionLine productionGreen = new ProductionLine(ProdGreenPose, Parameters.LegoColor.green);
    private Logistics logistics = new Logistics();
    public List<LegoBrick> RedBricks => productionRed.Bricks;
    public List<LegoBrick> BlueBricks => productionBlue.Bricks;
    public List<LegoBrick> YellowBricks => productionYellow.Bricks;
    public List<LegoBrick> GreenBricks => productionGreen.Bricks;
    public int ProductionRedX => productionRed.Location.X;
    public int ProductionRedY => productionRed.Location.Y;
    public double ProductionRedAngle => productionRed.RotationAngle;
    public int ProductionBlueX => productionBlue.Location.X;
    public int ProductionBlueY => productionBlue.Location.Y;
    public double ProductionBlueAngle => productionBlue.RotationAngle;
    public int ProductionYellowX => productionYellow.Location.X;
    public int ProductionYellowY => productionYellow.Location.Y;
    public double ProductionYellowAngle => productionYellow.RotationAngle;
    public int ProductionGreenX => productionGreen.Location.X;
    public int ProductionGreenY => productionGreen.Location.Y;
    public double ProductionGreenAngle => productionGreen.RotationAngle;
    // Zusatzbaender ohne Steine
    private ProductionLine productionRecycle = new ProductionLine(ProdRecyclePose);
    private ProductionLine productionDelivery = new ProductionLine(ProdDeliveryPose);
    public int ProductionRecycleX => productionRecycle.Location.X;
    public int ProductionRecycleY => productionRecycle.Location.Y;
    public double ProductionRecycleAngle => productionRecycle.RotationAngle;
    public int ProductionDeliveryX => productionDelivery.Location.X;
    public int ProductionDeliveryY => productionDelivery.Location.Y;
    public double ProductionDeliveryAngle => productionDelivery.RotationAngle;
    public List<LegoColor> LegoOrder => logistics.Order;
    public string LogisticsStep
    {
        get
        {
            if (logistics.OrderRunning)
            {
                return logistics.CurrentStep(PickStation(logistics.NeededColor));
            }
            else if (logistics.Order.Count == 0)
            {
                return "No order in progress";
            }
            else
            {
                return "Order completed";
            }
        }
    }
    public Factory()
    {
        robot = new Robot(RobotPose.Location, RobotBorders);
        //robot = new Robot(new Point(450, 300), new Rectangle(10, 10, 590, 890));
    }
    public void RobotMove(Direction direction)
    {
        robot.Move(direction);
        KontaktPruefen();
    }
    public void RobotRotate(Direction direction)
    {
        robot.Rotate(direction);
        KontaktPruefen();
    }
    // Loslassen: der Roboter legt den Stein ab und faehrt danach 4 Schritte nach links, also weg vom Greifer (rechts).
    // Wuerde das Wegfahren eine Kollision oder ein Anstossen an die Wand verursachen,
    // wird gar nicht losgelassen (Rueckgabe false).
    public UndockResult RobotUndock()
    {
        const int schritteWeg = 4;
        if (!robot.HaeltObjekt)
        {
            return UndockResult.noBrickDocked;
        }
        if (!robot.NachLinksFahrenMoeglich(schritteWeg))
        {
            return UndockResult.blocked;
        }
        LegoBrick? stein = robot.Dockobjekt as LegoBrick;
        UndockResult ergebnis = robot.Undock();
        if (ergebnis != UndockResult.delivered)
        {
            return ergebnis;   // z.B. notOnDeliveryLine einfach durchreichen
        }
        for (int i = 0; i < schritteWeg; i++)
        {
            robot.Move(Direction.left);
        }
        naechsterZumRecycling = zufall.Next(2) == 0;
        if (naechsterZumRecycling && stein != null)
        {
            int platz = FreierRecyclePlatz();
            if (platz >= 0)
            {
                stein.ZumRecycling(platz);
                return UndockResult.defect;
            }
            // alle vier Recycle-Plaetze belegt: Stein geht ausnahmsweise trotzdem zum Kunden
        }
        if (stein != null)
        {
            logistics.BrickDelivered(stein.Color);
        }
        return UndockResult.delivered;
    }
    // Prueft nach jeder Roboterbewegung alle Steine aller Baender: liegt einer am Greifer, wird er angedockt.
    // Kontakt dockt nur, solange der Roboter noch nichts haelt, also wird hoechstens ein Stein gegriffen.
    private void KontaktPruefen()
    {
        foreach (LegoBrick brick in productionRed.Bricks)
        {
            robot.Kontakt(brick);
        }
        foreach (LegoBrick brick in productionBlue.Bricks)
        {
            robot.Kontakt(brick);
        }
        foreach (LegoBrick brick in productionYellow.Bricks)
        {
            robot.Kontakt(brick);
        }
        foreach (LegoBrick brick in productionGreen.Bricks)
        {
            robot.Kontakt(brick);
        }
    }
    public void RobotResetRotation()
    {
        robot.ResetRotation();
    }
    public void LegoBricksMove()
    {
        productionRed.move();
        productionBlue.move();
        productionYellow.move();
        productionGreen.move();
        KontaktPruefen(); // ein Stein kann auch von selbst in den Greifer fahren
    }
    private List<LegoBrick> AlleBricks()
    {
        List<LegoBrick> alle = new List<LegoBrick>();
        alle.AddRange(productionRed.Bricks);
        alle.AddRange(productionBlue.Bricks);
        alle.AddRange(productionYellow.Bricks);
        alle.AddRange(productionGreen.Bricks);
        return alle;
    }
    private int FreierRecyclePlatz()
    {
        for (int platz = 0; platz < ViewParameters.RecyclePlaetze; platz++)
        {
            bool belegt = false;
            foreach (LegoBrick b in AlleBricks())
            {
                if (b.RecyclePlatz == platz &&
                    (b.Status == BrickStatus.toRecycling || b.Status == BrickStatus.onRecycleLine))
                {
                    belegt = true;
                }
            }
            if (!belegt)
            {
                return platz;
            }
        }
        return -1;
    }
    public void SetupProductionLines()
    {
        productionRed.CreateBricks();
        productionBlue.CreateBricks();
        productionYellow.CreateBricks();
        productionGreen.CreateBricks();
    }
    public void CreateOrder()
    {
        logistics.CreateRandomOrder();
    }
    public int GetDelivered(LegoColor farbe)
    {
        return logistics.GetDelivered(farbe);
    }
    private string PickStation(LegoColor farbe)
    {
        if (HatStein(AlleBricks(), farbe, BrickStatus.onRecycleLine))
        {
            return "recycle line";
        }
        if (HatStein(productionRed.Bricks, farbe, BrickStatus.onProductionLine))
        {
            return $"{productionRed.Color} production line";
        }
        if (HatStein(productionBlue.Bricks, farbe, BrickStatus.onProductionLine))
        {
            return $"{productionBlue.Color} production line";
        }
        if (HatStein(productionYellow.Bricks, farbe, BrickStatus.onProductionLine))
        {
            return $"{productionYellow.Color} production line";
        }
        if (HatStein(productionGreen.Bricks, farbe, BrickStatus.onProductionLine))
        {
            return $"{productionGreen.Color} production line";
        }
        return "nowhere (not available)";
    }
    private bool HatStein(List<LegoBrick> steine, LegoColor farbe, BrickStatus status)
    {
        foreach (LegoBrick stein in steine)
        {
            if (stein.Color == farbe && stein.Status == status)
            {
                return true;
            }
        }
        return false;
    }
}