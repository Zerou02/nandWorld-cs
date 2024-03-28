using System.Numerics;
using Raylib_cs;

public abstract class BaseComponent : OctoComp
{
    public Pin[] pins;
    public int inputPins;
    public string type = "";
    public Rectangle bounds = new Rectangle(0, 0, 0, 0);
    public BaseComponent parentComp;
    public List<BaseComponent> inputs = new List<BaseComponent>();
    public List<BaseComponent> outputs = new List<BaseComponent>();
    public List<BaseComponent> childComponents = new List<BaseComponent>();
    public bool isSelected = false;
    public BaseComponent(int amountPins, int amountInputPins)
    {
        this.pins = new Pin[amountPins];
        this.inputPins = amountInputPins;
        for (int i = 0; i < amountPins; i++)
        {
            this.pins[i] = new Pin(this, i < amountInputPins);
        }
        parentComp = this;
        eval();
    }



    public virtual void eval()
    {
        foreach (var x in inputs)
        {
            x.eval();
        }
    }

    public virtual void propagate()
    {/* 
        for (int i = inputPins; i < pins.Length; i++)
        {
            pins[i].setOuts();
        } */
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
        //pins[idx].connectedOuts.Add(pin);
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

    public List<BaseComponent> getComponents()
    {
        return new List<BaseComponent>();
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


    /* public void setBaseComp(IComponent component)
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
 */
    public bool getIsSelected()
    {
        return isSelected;
    }
    public void setIsSelected(bool val)
    {
        this.isSelected = val;
    }

    public void draw(OctoState state)
    {
        alignPins();
        var textBounds = bounds;
        textBounds.Y += textBounds.Height / 2;

        if (isSelected)
        {
            Drawing.drawRectangleWithOutline(bounds, Color.Blue, Color.Black);
        }
        else
        {
            Drawing.drawRectangle(bounds, Color.Blue);
        }
        foreach (var x in pins)
        {
            x.draw(state);
        }
        Drawing.fillTextInRectangle(getType().ToString(), textBounds, Color.White);
    }

    public void alignToGrid(Grid grid)
    {
        var pos = bounds.Position;
        setPosition(new Vector2((int)(pos.X / grid.gridSize) * grid.gridSize, (int)(pos.Y / grid.gridSize) * grid.gridSize));
    }

    void alignPins()
    {
        if (pins.Length == 0) { return; }
        var pin = pins[0];
        var tmpList = new List<Rectangle>();
        var inputs = getInputPins();
        foreach (var x in inputs)
        {
            tmpList.Add(x.bounds);
        }
        var newRect = bounds;
        newRect.X -= pin.bounds.Width / 2;
        var ordered = Flexbox.alignTop(newRect, tmpList);
        for (int i = 0; i < ordered.Count; i++)
        {
            inputs[i].bounds = ordered[i];
        }

        var outputs = getOutputPins();
        tmpList = new List<Rectangle>();
        foreach (var x in outputs)
        {
            tmpList.Add(x.bounds);
        }
        newRect = bounds;
        newRect.X += newRect.Width - pin.bounds.Width / 2;
        ordered = Flexbox.alignTop(newRect, tmpList);
        for (int i = 0; i < ordered.Count; i++)
        {
            outputs[i].bounds = ordered[i];
        }

    }
}