using System.Numerics;
using Raylib_cs;

public abstract class BaseComponent : IComponent
{
    public Pin[] pins;
    public int inputPins;
    public string type = "";
    public Rectangle bounds = new Rectangle(0, 0, 0, 0);
    public IComponent? baseComponent;
    public BaseComponent(int amountPins, int amountInputPins)
    {
        this.pins = new Pin[amountPins];
        this.inputPins = amountInputPins;
        for (int i = 0; i < amountPins; i++)
        {
            this.pins[i] = new Pin(this, i >= amountInputPins);
        }
        baseComponent = null;
        eval();
    }

    public abstract void eval();

    public virtual void propagate()
    {
        for (int i = inputPins; i < pins.Length; i++)
        {
            pins[i].setOuts();
        }
    }

    public virtual Pin[] getInputPins()
    {
        var retPins = new Pin[inputPins];
        for (int i = 0; i < inputPins; i++)
        {
            retPins[i] = pins[i];
        }
        return retPins;
    }

    public virtual Pin[] getOutputPins()
    {
        var retPins = new Pin[pins.Length - inputPins];
        for (int i = inputPins; i < pins.Length; i++)
        {
            retPins[i - inputPins] = pins[i];
        }
        return retPins;
    }
    public virtual void print()
    {
        var retStr = this.type + ": ";
        for (int i = 0; i < pins.Length; i++)
        {
            retStr += pins[i].state;
            if (i != pins.Length - 1 && i != inputPins)
            {
                retStr += ", ";
            }
            if (i == inputPins && i > 0)
            {
                retStr += " -> ";
            }
        }
        Console.WriteLine(retStr);
    }

    public Pin[] getPins()
    {
        return this.pins;
    }
    public void connectPin(int idx, Pin pin)
    {
        pins[idx].connectedOuts.Add(pin);
    }

    public virtual void setIn(int idx, bool val)
    {
        pins[idx].state = val;
        eval();
        propagate();
    }

    public string getType()
    {
        return this.type;
    }

    public List<IComponent> getComponents()
    {
        return new List<IComponent>();
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
        Console.WriteLine(this.getType() + "bbccff" + baseComp.getType());

        IComponent? last = null;
        while (baseComp.getBaseComp() != null && baseComp != last)
        {
            last = baseComp;
            baseComp = baseComp.getBaseComp();

        }
        return baseComp;
    }

    public List<IComponent> getConnectedHighestOuts()
    {
        var map = new HashSet<IComponent>();
        foreach (var x in getPins())
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
}