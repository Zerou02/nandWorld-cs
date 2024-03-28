using Raylib_cs;
public class Nand : BaseComponent
{
    public Nand() : base(3, 2)
    {
        type = "nand";
        this.bounds = new Rectangle(100, 100, 30, 60);
    }

    public override void eval()
    {
        pins[2].setState(!(pins[0].state && pins[1].state));
    }
}