using System.Numerics;
using Raylib_cs;
public class Cable : OctoComp
{
    public List<Pin> ins = new List<Pin>();
    public List<Pin> outs = new List<Pin>();
    public List<Vector2> path = new List<Vector2>() { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1) };
    public Grid grid;
    public Cable()
    {
    }

    public void addPin(bool inPin, Pin pin)
    {
        if (inPin)
        {
            ins.Add(pin);
        }
        else
        {
            outs.Add(pin);
        }
    }

    public void draw()
    {
        path.ForEach(x =>
        {
            Drawing.drawRectangleSC((int)x.X * grid.gridSize, (int)x.Y * grid.gridSize, grid.gridSize, grid.gridSize, Color.Yellow);
        });
    }
}