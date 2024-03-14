using System.Numerics;
using Raylib_cs;

public class OctoState : OctoComp
{
    public bool leftPressed = false;
    public bool shiftPressed = false;
    public bool capsLockPressed = false;
    public bool leftDown = false;
    public bool rightDown = false;
    public bool uiSelected = false;
    public Vector2 mousePos = new Vector2(0, 0);
    public Vector2 screenSize = new Vector2(0, 0);
    public int currFrame = 0;
    public void process()
    {
        leftPressed = Raylib.IsMouseButtonPressed(MouseButton.Left);
        shiftPressed = Raylib.IsKeyDown(KeyboardKey.LeftShift) || Raylib.IsKeyDown(KeyboardKey.RightShift);
        leftDown = Raylib.IsMouseButtonDown(MouseButton.Left);
        rightDown = Raylib.IsMouseButtonDown(MouseButton.Right);
        mousePos = Raylib.GetMousePosition();
        currFrame++;
        if (Raylib.IsKeyPressed(KeyboardKey.CapsLock))
        {
            capsLockPressed = !capsLockPressed;
        }
    }

}