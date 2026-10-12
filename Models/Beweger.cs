using System;
using System.Drawing;

namespace smartFactoryApp.Models
{
    public class Beweger
    {
        private Point location;
        private Rectangle borders;
        private double rotationAngle = 0;
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
