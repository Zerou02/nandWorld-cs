public class Input : BaseComponent
{
    public Input() : base(1, 0)
    {
        this.type = "input";
        this.pins[0].isReceiverPin = true;
        this.bounds = new Raylib_cs.Rectangle(100, 100, 30, 40);
    }

    public override void eval()
    {
        //  this.pins[0].setOuts();
    }

    public override void setIn(int idx, bool val)
    {
        pins[idx].state = val;
        propagate();
    }
}