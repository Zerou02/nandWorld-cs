public class Input : BaseComponent
{
    public Input() : base(1, 0)
    {
        this.type = "input";
    }

    public override void eval()
    {
    }

    public override void setIn(int idx, bool val)
    {
        pins[idx].state = val;
        propagate();
    }
}