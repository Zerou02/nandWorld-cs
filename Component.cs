using System.Collections.Generic;

public class Component : IComponent
{
    public List<IComponent> components = new List<IComponent>();
    public List<Input> inputs = new List<Input>();
    public List<Output> outputs = new List<Output>();

    public string type = "";

    public Component(string type)
    {
        this.type = type;
    }


    public void addComp(IComponent component)
    {
        this.components.Add(component);
        if (component.getType() == "input")
        {
            inputs.Add((Input)component);
        }
        else if (component.getType() == "output")
        {
            outputs.Add((Output)component);
        }
    }

    public void eval()
    {
        inputs.ForEach(x => x.pins[0].setOuts());
    }
    public virtual void print()
    {
        var printStr = type + ": ";
        inputs.ForEach(x =>
        {
            printStr += x.pins[0].state + ", ";
        });
        printStr += "=> ";
        outputs.ForEach(x =>
        {
            printStr += x.pins[0].state + ", ";
        });
        Console.WriteLine(printStr);
    }

    public virtual void connectPin(int idx, Pin pin)
    {
        getPins()[idx].connectedOuts.Add(pin);
    }
    public virtual Pin[] getPins()
    {
        var retPins = new Pin[inputs.Count + outputs.Count];
        var inputPins = getInputPins();
        var outputPins = getOutputPins();
        inputPins.CopyTo(retPins, 0);
        outputPins.CopyTo(retPins, inputPins.Length);
        return retPins;
    }
    public virtual Pin[] getInputPins()
    {
        var retPins = new Pin[inputs.Count];
        for (int i = 0; i < retPins.Length; i++)
        {
            retPins[i] = inputs[i].pins[0];
        }
        return retPins;
    }
    public virtual Pin[] getOutputPins()
    {
        var retPins = new Pin[outputs.Count];
        for (int i = 0; i < retPins.Length; i++)
        {
            retPins[i] = outputs[i].pins[0];
        }
        return retPins;
    }

    public string getType()
    {
        return this.type;
    }

    public List<IComponent> getComponents()
    {
        return this.components;
    }
}
