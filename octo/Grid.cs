using System.Numerics;
using Raylib_cs;

public class Grid : OctoComp
{
    public int gridSize = 20;
    public List<List<int>> grid = new List<List<int>>();

    public Grid(OctoState state)
    {
        for (int y = 0; y < (int)(state.screenSize.Y / gridSize) + 1; y++)
        {
            var innerList = new List<int>();
            for (int x = 0; x < (int)(state.screenSize.X / gridSize) + 1; x++)
            {
                innerList.Add(0);
            }
            grid.Add(innerList);
        }

    }
    public void draw(OctoState state)
    {
        for (int y = 0; y < (int)(state.screenSize.Y / gridSize) + 1; y++)
        {
            for (int x = 0; x < (int)(state.screenSize.X / gridSize) + 1; x++)
            {
                if (grid[y][x] == 0)
                {
                    Raylib.DrawLine(gridSize * x, gridSize * y, gridSize * x + gridSize, gridSize * y, Color.Gray);
                    Raylib.DrawLine(gridSize * x, gridSize * y, gridSize * x, gridSize * y + gridSize, Color.Gray);
                    Raylib.DrawLine(gridSize * x, gridSize * y + gridSize, gridSize * x + gridSize, gridSize * y + gridSize, Color.Gray);
                    Raylib.DrawLine(gridSize * x + gridSize, gridSize * y, gridSize * x + gridSize, gridSize * y + gridSize, Color.Gray);
                }
                else
                {
                    Drawing.drawRectangleSC(x * gridSize, y * gridSize, gridSize, gridSize, Color.Yellow);
                }
            }
        }

    }

    public Vector2 getGridPos(int x, int y)
    {
        return new Vector2((int)(x / gridSize), (int)(y / gridSize));
    }

    public void process(OctoState state)
    {
        var pos = getGridPos((int)state.mousePos.X, (int)state.mousePos.Y);
        if (state.leftDown)
        {
            grid[(int)pos.Y][(int)pos.X] = 1;
        }
        else if (state.rightDown)
        {
            grid[(int)pos.Y][(int)pos.X] = 0;

        }
    }
}