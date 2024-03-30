public class Output : BaseComponent
{
    public Output() : base(1, 1)
    {
        this.type = "output";
        this.pins[0].isInputPin = false;
        this.bounds = new Raylib_cs.Rectangle(100, 100, 30, 40);

    }

    public override void eval()
    {
        //    this.pins[0].setOuts();
    }

    public override void setIn(int idx, bool val)
    {
        pins[idx].state = val;
    }
}