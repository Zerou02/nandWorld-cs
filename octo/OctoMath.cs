using System.Numerics;
using Raylib_cs;

public class OctoMath
{
    public static bool isPointInRec(Vector2 point, Rectangle rectangle)
    {
        return Raylib.CheckCollisionPointRec(point, rectangle);
    }


    public static Vector2 getCentreOfCircle(Rectangle rectangle)
    {
        return new Vector2(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2);
    }

    public static bool mouseInRec(Rectangle rectangle)
    {
        return isPointInRec(Raylib.GetMousePosition(), rectangle);
    }
}