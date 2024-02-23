using System.Collections.Generic;

public class Cable
{
    public List<Pin> ins = new List<Pin>();
    public List<Pin> outs = new List<Pin>();

    public Cable()
    {
    }

    public void addPin(bool inPin, Pin pin)
    {
        if (inPin)
        {
            ins.Add(pin);
        }
        else
        {
            outs.Add(pin);
        }
    }
}