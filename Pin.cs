using Microsoft.VisualBasic;
using Raylib_cs;

public class Pin : OctoComp
{
    public Rectangle bounds = new Rectangle(0, 0, 20, 20);
    public BaseComponent baseComponent;
    public bool state = false;
    public bool isOut = false;
    public bool isInner = false;
    public bool isComposite = false;
    public List<Pin> connectedOuts = new List<Pin>();
    public Pin(BaseComponent baseComponent, bool isOut)
    {
        this.baseComponent = baseComponent;
        this.isOut = isOut;
    }

    public void setState(bool x)
    {
        this.state = x;
        this.baseComponent.eval();
    }

    public void setOuts()
    {
        connectedOuts.ForEach(x =>
        {
            x.state = this.state;
            x.baseComponent.eval();
            x.baseComponent.propagate();
        });
    }

    public void draw()
    {
        Drawing.drawCircle(bounds, state ? Color.Green : Color.Red);
        connectedOuts.ForEach(x =>
        {
            if (!(isInner || x.isInner))
            {
                Raylib.DrawLineV(OctoMath.getCentreOfCircle(bounds), OctoMath.getCentreOfCircle(x.bounds), Color.Black);
            }
        });
    }
}