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
        protected Beweger dockobjekt; // Object, which the robot is currently holding, null = robot is holding nothing
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
        private static bool InnerhalbGrenzen(Point p, Rectangle b)
        {
            return (p.X >= b.Left && p.X <= b.Right && p.Y >= b.Top && p.Y <= b.Bottom);
        }
        private static Point Greifpunkt(Point p)
        {
            return (new Point(p.X + ViewParameters.RobotWidth/2, p.Y));
        }
        private bool KollidiertMitProductionLine(Point newLoc)
        {
            if (this is not Robot)
            {
                return false;
            }
            Rectangle robot = new Rectangle(newLoc, new Size(ViewParameters.RobotWidth, ViewParameters.RobotHeight));
            return robot.IntersectsWith(new Rectangle(new Size(ViewParameters.productionLineWidth, ViewParameters.productionLineHeight)));
        }
        public bool Kontakt(Beweger objekt)
        {
            Point tcp = Greifpunkt(location);
            bool proximity = Math.Abs(tcp.X - objekt.Location.X) <= 10 && Math.Abs(tcp.Y - objekt.Location.Y) <= 10;

            if (proximity && dockobjekt == null && objekt is LegoBrick aBrick && !aBrick.Gedockt)
            {
                aBrick.Gedockt = true;
                dockobjekt = objekt;
                dockobjekt.Location = tcp;
            }
            return proximity;
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
            dockobjekt = null;
            return true;
        }
        public void Rotate(Parameters.Direction rotationDirection) 
        {
            if (rotationDirection == Parameters.Direction.clockwise)
            {
                rotationAngle += Parameters.degreeStep;
                rotationAngle = (rotationAngle % 360 + 360) % 360;
            }
            else if (rotationDirection == Parameters.Direction.counterclockwise)
            {
                rotationAngle -= Parameters.degreeStep;
                rotationAngle = (rotationAngle % 360 + 360) % 360;
            }
        }
        public void ResetRotation()
        {
            rotationAngle = 0;
        }
    }
}
