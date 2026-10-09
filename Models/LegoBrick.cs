using System;
using System.Drawing;
using System.Reflection.Metadata;
using static smartFactoryApp.Models.Parameters;

namespace smartFactoryApp.Models;

public class LegoBrick : Beweger
{
    // private LegoColor color;
    // private LegoSize size;
    public LegoColor Color { get; } // Wenn (using static smartFactoryApp.Models.Parameters;) nicht da ist,
    //  dann muss man hier schreiben: public Parameters.LegoColor Color { get; }
    public LegoSize Size { get; } // hier wird auch implizit size und color als private Variablen angelegt,
    //  die nur über die Properties Color und Size gelesen werden koennen
    public LegoBrick(Pose pose, LegoColor color, LegoSize size) : base(pose)
    {
        Borders = ViewParameters.Borders; // sonst ist Borders leer und der Stein kann sich nicht frei bewegen
        Color = color;
        Size = size;
    }
}