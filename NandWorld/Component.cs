/* using System.Collections.Generic;
using System.IO.Compression;
using System.Numerics;
using Raylib_cs;


public class Component : IComponent
{
    public List<IComponent> components = new List<IComponent>();
    public List<Input> inputs = new List<Input>();
    public List<Output> outputs = new List<Output>();
    public Rectangle bounds = new Rectangle(0, 0, 100, 100);
    public IComponent? baseComponent = null;
    public bool isSelected;
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

    public Rectangle getBounds()
    {
        return bounds;
    }

    public void move(Vector2 vec)
    {
        this.bounds = new Rectangle(this.bounds.Position + vec, this.bounds.Size);
    }

    public void setPosition(Vector2 vec)
    {
        this.bounds = new Rectangle(vec, this.bounds.Size);
    }

    public void setSize(Vector2 vector2)
    {
        this.bounds = new Rectangle(this.bounds.Position, vector2);
    }

    public void setBaseComp(IComponent component)
    {
        this.baseComponent = component;
    }

    public IComponent? getBaseComp()
    {
        return this.baseComponent;
    }
    public IComponent getHighestComp()
    {
        var baseComp = this.baseComponent;
        if (baseComp == null || baseComp == this)
        {
            return this;
        }
        while (baseComp!.getBaseComp() != null)
        {
            baseComp = baseComp.getBaseComp();
        }
        return baseComp;
    }

    public List<IComponent> getConnectedHighestOuts()
    {
        var map = new HashSet<IComponent>();
        foreach (var x in getOutputPins())
        {
            foreach (var y in x.connectedOuts)
            {
                map.Add(y.baseComponent.getHighestComp());
            }
        }
        return map.ToList();
    }

    public void setHighestComp(IComponent component)
    {
        this.baseComponent = component;
    }

    public bool getIsSelected()
    {
        return isSelected;
    }

    public void setIsSelected(bool val)
    {
        this.isSelected = val;
    }
}

 */