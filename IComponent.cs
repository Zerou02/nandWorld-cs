using System.Data;
using System.Numerics;
using Raylib_cs;

public interface IComponent : OctoComp
{
    public string getType();
    public List<IComponent> getComponents();
    public void eval();

    public Pin[] getInputPins();
    public Pin[] getOutputPins();
    public Pin[] getPins();
    public void print();
    public void connectPin(int idx, Pin pin);
    Rectangle getBounds();
    void move(Vector2 vec);
    void setPosition(Vector2 vec);
    void setSize(Vector2 vec);
    void setBaseComp(IComponent component);
    IComponent? getBaseComp();
    IComponent getHighestComp();
    List<IComponent> getConnectedHighestOuts();
    void setHighestComp(IComponent component);
    void alignSizeToGrid(Grid grid)
    {
        var biggestPinSize = Math.Max(getInputPins().Length, getOutputPins().Length);
        var xPos = getInputPins()[0].bounds.Size.X * 2;
        var yPos = biggestPinSize * getInputPins()[0].bounds.Size.Y + 10;
        setSize(new Vector2((int)(xPos / grid.gridSize) * grid.gridSize, (int)(yPos / grid.gridSize) * grid.gridSize));
    }
    bool getIsSelected();
    void setIsSelected(bool val);

    public void process(OctoState state)
    {
        if (state.leftPressed)
        {
            setIsSelected(OctoMath.mouseInRec(getBounds()));
        }
    }
    void alignPins()
    {
        var pin = getPins()[0];
        var tmpList = new List<Rectangle>();
        var inputs = getInputPins();
        foreach (var x in inputs)
        {
            tmpList.Add(x.bounds);
        }
        var newRect = getBounds();
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
        newRect = getBounds();
        newRect.X += newRect.Width - pin.bounds.Width / 2;
        ordered = Flexbox.alignTop(newRect, tmpList);
        for (int i = 0; i < ordered.Count; i++)
        {
            outputs[i].bounds = ordered[i];
        }

    }
    void draw()
    {
        alignPins();
        var bounds = getBounds();
        var textBounds = bounds;
        textBounds.Y += textBounds.Height / 2;

        var inputs = getPins();

        if (getIsSelected())
        {
            Drawing.drawRectangleWithOutline(bounds, Color.Blue, Color.Black);
        }
        else
        {
            Drawing.drawRectangle(bounds, Color.Blue);
        }
        for (int i = 0; i < inputs.Length; i++)
        {
            inputs[i].draw();
        }
        Drawing.fillTextInRectangle(getType().ToString(), textBounds, Color.White);
    }

    void alignToCamera(Camera camera)
    {
        this.move(camera.lastMov);
    }
    bool wouldBeCyclic(IComponent newComponent)
    {
        if (this == newComponent) { return true; }
        var found = false;
        var openList = new Queue<IComponent>() { };
        var closedList = new Queue<IComponent>() { };

        foreach (var x in newComponent.getOutputPins())
        {
            x.connectedOuts.ForEach(y =>
            {
                if (!OctoUtils.queueContains(openList, y.baseComponent.getHighestComp()) && !OctoUtils.queueContains(closedList, y.baseComponent.getHighestComp()))
                {
                    openList.Enqueue(y.baseComponent.getHighestComp());
                }
            });
        }
        while (openList.Count > 0 && !found)
        {
            var e = openList.Dequeue();
            if (e == getHighestComp())
            {
                found = true;
            }
            foreach (var x in e.getOutputPins())
            {
                x.connectedOuts.ForEach(y =>
                {
                    if (!OctoUtils.queueContains(openList, y.baseComponent.getHighestComp()) && !OctoUtils.queueContains(closedList, y.baseComponent.getHighestComp()))
                    {
                        openList.Enqueue(y.baseComponent.getHighestComp());
                    }
                });
            }
            closedList.Enqueue(e);
        }
        return found;
    }

    void alignToGrid(Grid grid)
    {
        var pos = getBounds().Position;
        setPosition(new Vector2((int)(pos.X / grid.gridSize) * grid.gridSize, (int)(pos.Y / grid.gridSize) * grid.gridSize));
    }
}