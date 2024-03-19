using System.Numerics;
using Raylib_cs;
public class Cable : OctoComp
{
    public List<Pin> ins = new List<Pin>();
    public List<Pin> outs = new List<Pin>();
    public HashSet<Vector2> path = new HashSet<Vector2>();
    public Grid grid;
    public OctoSpriteSheet tex;
    public int cableType = 0;
    public Cable(Grid grid, OctoSpriteSheet tex, int cableType)
    {
        this.grid = grid;
        this.tex = tex;
        this.cableType = cableType;
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

    public int addToBinIndex(int idx, Vector2 currField, Vector2 nextField)
    {
        var offset = new Vector2(currField.X - nextField.X, currField.Y - nextField.Y);
        int retIdx = idx;
        if (offset.X == 1)
        {
            retIdx |= 0b100;
        }
        else if (offset.X == -1)
        {
            retIdx |= 0b01;
        }
        if (offset.Y == -1)
        {
            retIdx |= 0b10;
        }
        else if (offset.Y == 1)
        {
            retIdx |= 0b1000;
        }
        return retIdx;
    }

    public Vector2[] getNeighbours(Vector2 pos)
    {
        var vectorOffsets = new Vector2[] { pos - new Vector2(1, 0), pos + new Vector2(1, 0), pos + new Vector2(0, 1), pos - new Vector2(0, 1) };
        var retArr = new Vector2[4];
        for (int i = 0; i < retArr.Length; i++)
        {
            if (!path.TryGetValue(vectorOffsets[i], out retArr[i]))
            { retArr[i] = pos; }
        }
        return retArr;
    }

    public void add(Vector2 vector)
    {
        if (!path.Contains(vector))
        {
            path.Add(vector);
        }
    }
    public bool isAdjacentTo(Vector2 vector)
    {
        bool retVal = false;
        foreach (var x in getNeighbours(vector))
        {
            if (path.Contains(x))
            {
                retVal = true;
            }
        }
        return retVal;
    }
    public void draw(OctoState state)
    {
        foreach (var x in path)
        {
            var sum = 0;
            var neighbours = getNeighbours(x);
            foreach (var y in neighbours)
            {
                sum = addToBinIndex(sum, x, y);
            }
            Drawing.drawSpriteFromSheet(tex, new Rectangle(x.X * grid.gridSize, x.Y * grid.gridSize, grid.gridSize, grid.gridSize), sum + tex.columns * (cableType * 2));
        }
    }
}