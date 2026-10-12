using System.Drawing;
namespace smartFactoryApp.Models;

public class Robot : Beweger
{
    public Robot(Point location, Rectangle borders) : base(location, borders)
    {
    }
}