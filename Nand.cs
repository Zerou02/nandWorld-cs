using System.Numerics;

public class Nand : BaseComponent
{
    public Vector2 position = new Vector2(100, 100);
    public Nand() : base(3, 2)
    {
        type = "nand";
    }

    public override void eval()
    {
        pins[2].state = !(pins[0].state && pins[1].state);
    }

}