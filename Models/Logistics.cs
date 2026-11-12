using static smartFactoryApp.Models.Parameters;
using System;
using System.Collections.Generic;

namespace smartFactoryApp.Models;

public class Logistics
{
    private Random random = new Random();
    private List<LegoColor> order = new List<LegoColor>();
    private int[] delivered = new int[4];     // Index = (int)Farbe, also delivered[0] = gelb usw.
    private int naechsterSchritt = 0;
    public List<LegoColor> Order { get => order; }
    public int[] Delivered { get => delivered; }
    public int NextStep { get => naechsterSchritt; }
    public void CreateRandomOrder()
    {
        order.Clear();
        for (int i = 0; i < orderSize; i++)
        {
            int farbe = random.Next(0, 4);
            order.Add((LegoColor)farbe);
        }
        delivered = new int[4];
        naechsterSchritt = 0;
    }
    public int GetDelivered(LegoColor farbe)
    {
        return delivered[(int)farbe];
    }
}