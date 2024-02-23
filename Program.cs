using System.Numerics;
using System.Threading.Tasks.Dataflow;
using Raylib_cs;

namespace HelloWorld;

class Program
{
    public static void Main()
    {
        Raylib.InitWindow(800, 480, "Definitiv Godot");
        var nand = new Nand();
        var scrollSpeed = 0.05f;
        Tester.test(nand, new bool[][] { new bool[] { false, false, true }, new bool[] { false, true, true }, new bool[] { true, false, true }, new bool[] { true, true, false } });
        var rectangle = new Rectangle(0, 0, 100, 100);
        var blending = BlendMode.Alpha;
        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.BeginBlendMode(blending);
            Raylib.ClearBackground(Color.Beige);
            Raylib.DrawText("Hello, world!", 12, 12, 20, Color.Black);
            var cameraMovVec = new Vector2();
            drawNand(nand);
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
            if (Raylib.IsKeyDown(KeyboardKey.E))
            {
                rectangle.Size = Raymath.Vector2Add(rectangle.Size, new Vector2(10 * scrollSpeed, 10 * scrollSpeed));
            }
            if (Raylib.IsKeyDown(KeyboardKey.Q))
            {
                rectangle.Size = Raymath.Vector2Add(rectangle.Size, new Vector2(-10 * scrollSpeed, -10 * scrollSpeed));
            }
            cameraMovVec = Raymath.Vector2Scale(Raymath.Vector2Normalize(cameraMovVec), scrollSpeed);
            nand.position = Raymath.Vector2Add(nand.position, cameraMovVec);
            drawHairCross();
            drawRectangle(rectangle, Color.Purple);
            drawTextInRectangle("123", rectangle, Color.Green);
            Raylib.EndBlendMode();
            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }

    static void drawRectangle(Rectangle rectangle, Color color)
    {
        Raylib.DrawRectangle((int)rectangle.X, (int)rectangle.Y, (int)rectangle.Width, (int)rectangle.Height, Color.Blue);
    }
    static void drawNand(Nand nand)
    {
        Raylib.DrawRectangle((int)nand.position.X, (int)nand.position.Y, 300, 300, Color.Red);
    }

    static void drawTextInRectangle(string text, Rectangle rectangle, Color color)
    {
        var width = rectangle.Width;
        var fontSize = 1;
        while (Raylib.MeasureText(text, fontSize) < width)
        {
            fontSize++;
        }
        fontSize--;
        Raylib.DrawText(text, (int)rectangle.X, (int)rectangle.Y, fontSize, color);
    }
    static void drawHairCross()
    {
        var width = Raylib.GetScreenWidth();
        var height = Raylib.GetScreenHeight();
        var size = 100;
        var startPos = new Vector2(width / 2, height / 2);
        Raylib.DrawLine((int)startPos.X - size, (int)startPos.Y, (int)startPos.X + size, (int)startPos.Y, Color.White);
        Raylib.DrawLine((int)startPos.X, (int)startPos.Y - size, (int)startPos.X, (int)startPos.Y + size, Color.White);

    }
}