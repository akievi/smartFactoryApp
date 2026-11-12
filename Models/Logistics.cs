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
    public LegoColor NeededColor => order[naechsterSchritt];
    public bool OrderRunning => naechsterSchritt < order.Count;
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
    public bool BrickDelivered(LegoColor farbe)
    {
        if (OrderRunning && order[naechsterSchritt] == farbe)
        {
            delivered[(int)farbe]++;
            naechsterSchritt++;
            return true;
        }
        else
        {
            return false;
        }
    }
    public string CurrentStep(string station)
    {
        if (OrderRunning)
        {
            return "Next: " + "( Step " + (naechsterSchritt + 1) + "/" + order.Count + " ): " + order[naechsterSchritt] + " at " + station;
        }
        else if (order.Count == 0)
        {
            return "No order in progress";
        }
        else
        {
            return "Order completed";
        }
    }
}