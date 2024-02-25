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
        var rectangles = new List<Rectangle>() { new Rectangle(0, 0, 100, 100),new Rectangle(0, 0, 100, 100),new Rectangle(0, 0, 100, 100),new Rectangle(0, 0, 100, 100),
    };
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
            nand.bounds = new Rectangle(Raymath.Vector2Add(nand.bounds.Position, cameraMovVec), nand.bounds.Size);
            /*      drawRectangle(rectangle, Color.Purple);
                 drawTextInRectangle("123", rectangle, Color.Green);
                 foreach (var x in Flexbox.alignTop(new Rectangle(0, 200, 100, 500), rectangles))
                 {
                     drawRectangleWithOutline(x, Color.Green, Color.Black);
                 }
                 drawHairCross(); */
            Raylib.DrawLine(400, 240, 400, 40, Color.Black);
            Raylib.DrawLine(400, 240, (int)Raylib.GetMousePosition().X, (int)Raylib.GetMousePosition().Y, Color.Red);
            var a = Raymath.Vector2LineAngle(new Vector2(400, 240), Raylib.GetMousePosition());
            Raymath.
            Console.WriteLine(a);
            Raylib.EndBlendMode();
            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }

    static void drawRectangle(Rectangle rectangle, Color color)
    {
        Raylib.DrawRectangle((int)rectangle.X, (int)rectangle.Y, (int)rectangle.Width, (int)rectangle.Height, color);
    }

    static void drawRectangleWithOutline(Rectangle rectangle, Color color, Color outlineColor)
    {
        Raylib.DrawRectangle((int)rectangle.X, (int)rectangle.Y, (int)rectangle.Width, (int)rectangle.Height, color);
        Raylib.DrawRectangleLines((int)rectangle.X, (int)rectangle.Y, (int)rectangle.Width, (int)rectangle.Height, outlineColor);

    }
    static void drawNand(Nand nand)
    {
        var tmpList = new List<Rectangle>();
        var inputs = nand.getInputPins();
        foreach (var x in inputs)
        {
            tmpList.Add(x.bounds);
        }
        var ordered = Flexbox.alignTop(nand.bounds, tmpList);
        for (int i = 0; i < ordered.Count; i++)
        {
            inputs[i].bounds = ordered[i];
        }
        var outputs = nand.getOutputPins();

        drawRectangle(nand.bounds, Color.Red);
        for (int i = 0; i < inputs.Length; i++)
        {
            var bounds = inputs[i].bounds;
            Console.WriteLine(bounds);
            Raylib.DrawCircle((int)(bounds.X + bounds.Width * 0), (int)(bounds.Y + bounds.Height / 2), bounds.Width / 2, Color.Blue);
        }
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