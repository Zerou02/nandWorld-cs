using Raylib_cs;
public class Pin : OctoComp
{
    public Rectangle bounds = new Rectangle(0, 0, 20, 20);
    public BaseComponent parentComp;
    public bool state = false;
    public bool isInner = false;
    public bool isComposite = false;
    public List<Cable> cables = new List<Cable>();
    public bool isReceiverPin = false;

    public Pin(BaseComponent parentComp, bool isReceiverPin)
    {
        this.isReceiverPin = isReceiverPin;
        this.parentComp = parentComp;
    }

    public void setState(bool x)
    {
        if (x == this.state) { return; }
        this.state = x;
        //propagate();
        this.parentComp.eval();
    }

    public void removeFromCables()
    {
        cables.ForEach(x => x.pins.Remove(this));
        this.cables = new List<Cable>();
    }

    /*     public void propagate()
        {
            foreach (var x in cables)
            {
                foreach (var y in x.pins)
                {
                    if (y != this)
                    {
                        y.setState(this.state);
                    }
                }
            }
        } */

    public void draw(OctoState state)
    {
        Drawing.drawCircle(bounds, this.state ? Color.Green : Color.Red);
        /* 
        connectedOuts.ForEach(x =>
        {
            if (!(isInner || x.isInner))
            {
                Raylib.DrawLineV(OctoMath.getCentreOfCircle(bounds), OctoMath.getCentreOfCircle(x.bounds), Color.Black);
            }
        }); */
    }
}