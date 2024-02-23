using System.Collections.Generic;

public class Pin
{
    public BaseComponent baseComponent;
    public bool state = false;
    public bool isOut = false;
    public List<Pin> connectedOuts = new List<Pin>();
    public Pin(BaseComponent baseComponent, bool isOut)
    {
        this.baseComponent = baseComponent;
        this.isOut = isOut;
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
}