using Raylib_cs;

public class Nand : BaseComponent
{
    public Rectangle bounds = new Rectangle(100, 100, 30, 60);
    public Nand() : base(3, 2)
    {
        type = "nand";
    }

    public override void eval()
    {
        pins[2].state = !(pins[0].state && pins[1].state);
    }

}