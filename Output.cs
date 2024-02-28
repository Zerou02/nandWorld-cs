public class Output : BaseComponent
{
    public Output() : base(1, 1)
    {
        this.type = "output";
        this.bounds = new Raylib_cs.Rectangle(100, 100, 30, 40);

    }

    public override void eval()
    {
    }

    public override void setIn(int idx, bool val)
    {
        pins[idx].state = val;
        propagate();
    }

    public override void propagate()
    {
        pins[0].setOuts();
    }
}