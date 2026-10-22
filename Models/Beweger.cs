using System;
using System.Drawing;
using smartFactoryApp.Models;

namespace smartFactoryApp.Models
{
    public class Beweger
    {
        private Point location;
        private Rectangle borders;
        private double rotationAngle = 0;
        protected Beweger? dockobjekt; // Object, which the robot is currently holding, null = robot is holding nothing
        private Beweger? zuletztLosgelassen; // gerade losgelassener Stein, wird erst wieder gegriffen, wenn der Greifer sich entfernt hat
        public bool HaeltObjekt => dockobjekt != null;
        public double RotationAngle
        {
            get { return rotationAngle; }
            set { rotationAngle = value; }
            }
        public Point Location
        {
            get { return location; }
            set { location = value; }
        }

        public Rectangle Borders { get => borders; set => borders = value; } //verkuerzte Syntax fuer Properties

        public Beweger(Point location, Rectangle borders)
        {
            this.location = location;
            this.borders = borders;
            //Location = location;
            //Borders = borders;
            //hier unnoetige Nutzung der Properties, da die privaten Parameter direkt angesprochen werden koennen
        }
        public Beweger(Pose pose, Rectangle borders) : this(pose.Location, borders)
        {
            RotationAngle = pose.Angle;
        }
        public virtual void Move(Parameters.Direction direction)
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
        protected static bool InnerhalbGrenzen(Point p, Rectangle b)
        {
            return (p.X >= b.Left && p.X <= b.Right && p.Y >= b.Top && p.Y <= b.Bottom);
        }
        // Rechnet einen Punkt im ungedrehten Roboterbild (lokalX/lokalY ab linker oberer Ecke) in Bildschirmkoordinaten um,
        // fuer einen Roboter mit linker oberer Ecke p und dem aktuellen Drehwinkel. Gedreht wird um die Robotermitte,
        // genau wie die RotateTransform in der View.
        protected Point PunktAmRoboter(Point p, double lokalX, double lokalY)
        {
            double mitteX = p.X + ViewParameters.RobotWidth / 2.0;
            double mitteY = p.Y + ViewParameters.RobotHeight / 2.0;

            // Versatz von der Mitte im ungedrehten Bild
            double versatzX = lokalX - ViewParameters.RobotWidth / 2.0;
            double versatzY = lokalY - ViewParameters.RobotHeight / 2.0;

            // Versatz um den Drehwinkel drehen (im Uhrzeigersinn, weil y auf dem Bildschirm nach unten waechst)
            double rad = rotationAngle * Math.PI / 180;
            double gedrehtX = versatzX * Math.Cos(rad) - versatzY * Math.Sin(rad);
            double gedrehtY = versatzX * Math.Sin(rad) + versatzY * Math.Cos(rad);

            return new Point((int)Math.Round(mitteX + gedrehtX), (int)Math.Round(mitteY + gedrehtY));
        }
        // Greifpunkt: die Greiferspitze im Bild (oben rechts), mitgedreht
        protected Point Greifpunkt(Point p)
        {
            return PunktAmRoboter(p, ViewParameters.GreiferOffsetX, ViewParameters.GreiferOffsetY);
        }
        // Tragepunkt: Mitte des getragenen Steins. Liegt auf Hoehe des Greifers, aber rechts AUSSERHALB des Roboters
        // (nur 4 px ueberlappen), damit der Stein seitlich herausragt und nicht vom Roboter verdeckt wird.
        protected Point Tragepunkt(Point p)
        {
            double lokalX = ViewParameters.RobotWidth + ViewParameters.BrickSize / 2.0 - 4;
            return PunktAmRoboter(p, lokalX, ViewParameters.GreiferOffsetY);
        }
        // Prueft, ob der Roboter an Position newLoc (linke obere Ecke) mit dem Winkel winkel eines der vier Baender beruehrt.
        // Roboter und Baender werden dabei als gedrehte Rechtecke gegeneinander geprueft.
        protected bool KollidiertMitProductionLine(Point newLoc, double winkel)
        {
            if (this is not Robot)
            {
                return false;
            }
            PointF[] robot = Ecken(newLoc, ViewParameters.RobotWidth, ViewParameters.RobotHeight, winkel);

            Pose[] baender = { ViewParameters.ProdRedPose, ViewParameters.ProdBluePose, ViewParameters.ProdYellowPose, ViewParameters.ProdGreenPose,
                               ViewParameters.ProdRecyclePose, ViewParameters.ProdDeliveryPose };
            foreach (Pose band in baender)
            {
                PointF[] bandEcken = Ecken(band.Location, ViewParameters.productionLineWidth, ViewParameters.productionLineHeight, band.Angle);
                if (UeberschneidenSich(robot, bandEcken))
                {
                    return true;
                }
            }
            return false;
        }
        // Die vier Ecken eines Rechtecks (linke obere Ecke ecke, breite x hoehe), um seine Mitte um winkel gedreht.
        // Reihenfolge: oben links, oben rechts, unten rechts, unten links (im ungedrehten Rechteck).
        protected static PointF[] Ecken(Point ecke, int breite, int hoehe, double winkel)
        {
            double mitteX = ecke.X + breite / 2.0;
            double mitteY = ecke.Y + hoehe / 2.0;
            double rad = winkel * Math.PI / 180;
            double cos = Math.Cos(rad);
            double sin = Math.Sin(rad);

            double[,] versatz = { { -breite / 2.0, -hoehe / 2.0 }, { breite / 2.0, -hoehe / 2.0 },
                                  { breite / 2.0, hoehe / 2.0 }, { -breite / 2.0, hoehe / 2.0 } };
            PointF[] ergebnis = new PointF[4];
            for (int i = 0; i < 4; i++)
            {
                double vx = versatz[i, 0];
                double vy = versatz[i, 1];
                ergebnis[i] = new PointF((float)(mitteX + vx * cos - vy * sin), (float)(mitteY + vx * sin + vy * cos));
            }
            return ergebnis;
        }
        // Trennachsen-Test (Separating Axis Theorem) fuer zwei gedrehte Rechtecke:
        // Zwei konvexe Formen ueberschneiden sich NICHT, wenn es eine Achse gibt, auf der sich ihre "Schatten" nicht ueberlappen.
        // Bei Rechtecken reicht es, die Kantenrichtungen beider Rechtecke als Achsen zu testen (je 2, also 4 Achsen).
        protected static bool UeberschneidenSich(PointF[] a, PointF[] b)
        {
            PointF[][] formen = { a, b };
            foreach (PointF[] form in formen)
            {
                // die zwei Kanten oben (0->1) und rechts (1->2) liefern die beiden Achsen dieses Rechtecks
                for (int k = 0; k < 2; k++)
                {
                    double achseX = form[k + 1].X - form[k].X;
                    double achseY = form[k + 1].Y - form[k].Y;

                    // alle Ecken beider Rechtecke auf die Achse projizieren (Skalarprodukt) und Minimum/Maximum merken
                    double minA = double.MaxValue;
                    double maxA = double.MinValue;
                    foreach (PointF punkt in a)
                    {
                        double projektion = punkt.X * achseX + punkt.Y * achseY;
                        minA = Math.Min(minA, projektion);
                        maxA = Math.Max(maxA, projektion);
                    }
                    double minB = double.MaxValue;
                    double maxB = double.MinValue;
                    foreach (PointF punkt in b)
                    {
                        double projektion = punkt.X * achseX + punkt.Y * achseY;
                        minB = Math.Min(minB, projektion);
                        maxB = Math.Max(maxB, projektion);
                    }

                    // Luecke auf dieser Achse gefunden: dann beruehren sich die Rechtecke nicht
                    if (maxA < minB || maxB < minA)
                    {
                        return false;
                    }
                }
            }
            // auf keiner Achse eine Luecke: die Rechtecke ueberschneiden sich
            return true;
        }
        public bool Kontakt(Beweger objekt)
        {
            Point tcp = Greifpunkt(location);
            // mit der MITTE des Steins vergleichen, nicht mit seiner linken oberen Ecke
            int objektMitteX = objekt.Location.X + ViewParameters.BrickSize / 2;
            int objektMitteY = objekt.Location.Y + ViewParameters.BrickSize / 2;
            bool proximity = Math.Abs(tcp.X - objektMitteX) <= 10 && Math.Abs(tcp.Y - objektMitteY) <= 10;

            // den gerade losgelassenen Stein nicht sofort wieder greifen
            if (objekt == zuletztLosgelassen)
            {
                if (!proximity)
                {
                    zuletztLosgelassen = null; // Greifer ist weg, ab jetzt darf er wieder gegriffen werden
                }
                return proximity;
            }

            if (proximity && dockobjekt == null && objekt is LegoBrick aBrick && !aBrick.Gedockt)
            {
                aBrick.Gedockt = true;
                dockobjekt = objekt;
                DockobjektNachfuehren();
            }
            return proximity;
        }
        // Setzt das gegriffene Objekt so, dass seine Mitte auf dem Tragepunkt liegt (seitlich neben dem Roboter).
        // Muss nach jeder Bewegung und Drehung des Roboters aufgerufen werden, sonst bleibt der Stein stehen.
        protected void DockobjektNachfuehren()
        {
            if (dockobjekt == null)
            {
                return;
            }
            Point trage = Tragepunkt(location);
            dockobjekt.Location = new Point(trage.X - ViewParameters.BrickSize / 2, trage.Y - ViewParameters.BrickSize / 2);
        }
        public bool Undock()
        {
            if (dockobjekt == null)
            {
                return false;
            }
            if (dockobjekt is LegoBrick aBrick)
            {
                aBrick.Gedockt = false;
            }
            zuletztLosgelassen = dockobjekt;
            dockobjekt = null;
            return true;
        }
        public void Rotate(Parameters.Direction rotationDirection)
        {
            double neuerWinkel = rotationAngle;
            if (rotationDirection == Parameters.Direction.clockwise)
            {
                neuerWinkel += Parameters.degreeStep;
            }
            else if (rotationDirection == Parameters.Direction.counterclockwise)
            {
                neuerWinkel -= Parameters.degreeStep;
            }
            neuerWinkel = (neuerWinkel % 360 + 360) % 360;

            // nicht drehen, wenn der Roboter dabei in ein Band ragen wuerde
            if (KollidiertMitProductionLine(location, neuerWinkel))
            {
                return;
            }
            rotationAngle = neuerWinkel;
            DockobjektNachfuehren(); // Greifer wandert beim Drehen mit, der Stein auch
        }
        public void ResetRotation()
        {
            if (KollidiertMitProductionLine(location, 0))
            {
                return;
            }
            rotationAngle = 0;
            DockobjektNachfuehren();
        }
    }
}
