using System.Numerics;
using Raylib_cs;

public class Camera : OctoComp
{
    public Vector2 offset;
    public Vector2 lastMov;
    public float scrollSpeed = 1f;

    public void process()
    {
        var cameraMovVec = new Vector2();
        if (Raylib.IsKeyDown(KeyboardKey.D))
        {
            cameraMovVec.X += -1 * scrollSpeed;
        }
        if (Raylib.IsKeyDown(KeyboardKey.W))
        {
            cameraMovVec.Y += +1 * scrollSpeed;
        }
        if (Raylib.IsKeyDown(KeyboardKey.S))
        {
            cameraMovVec.Y += -1 * scrollSpeed;
        }
        if (Raylib.IsKeyDown(KeyboardKey.A))
        {
            cameraMovVec.X += +1 * scrollSpeed;
        }
        offset += cameraMovVec;
        lastMov = cameraMovVec;
    }
}