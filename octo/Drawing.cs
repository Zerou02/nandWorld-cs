using System.Numerics;
using Raylib_cs;

public class Drawing
{

    public static void drawRectangle(Rectangle rectangle, Color color)
    {
        Raylib.DrawRectangle((int)rectangle.X, (int)rectangle.Y, (int)rectangle.Width, (int)rectangle.Height, color);
    }

    public static void drawRectangleSC(int x, int y, int w, int h, Color color)
    {
        Raylib.DrawRectangle(x, y, w, h, color);
    }

    public static void drawRectangleWithOutline(Rectangle rectangle, Color color, Color outlineColor)
    {
        Raylib.DrawRectangle((int)rectangle.X, (int)rectangle.Y, (int)rectangle.Width, (int)rectangle.Height, color);
        Raylib.DrawRectangleLines((int)rectangle.X, (int)rectangle.Y, (int)rectangle.Width, (int)rectangle.Height, outlineColor);
    }

    public static void fillTextInRectangle(string text, Rectangle rectangle, Color color, int textSize = -1)
    {
        var fontSize = textSize;
        if (textSize == -1)
        {
            fontSize = 1;
            var width = rectangle.Width;
            var size = Raylib.MeasureTextEx(Raylib.GetFontDefault(), text, fontSize, 5);
            while (size.X < width && size.Y < rectangle.Height)
            {
                fontSize++;
                size = Raylib.MeasureTextEx(Raylib.GetFontDefault(), text, fontSize, 5);
            }
            fontSize--;
        }
        Raylib.DrawText(text, (int)rectangle.X, (int)rectangle.Y, fontSize, color);
    }

    public static void drawCircle(Rectangle rect, Color color)
    {
        Raylib.DrawCircleV(OctoMath.getCentreOfCircle(rect), rect.Width / 2, color);
    }

    public static void drawImage(SpriteTex tex, Rectangle src, Rectangle dest)
    {
        Raylib.DrawTexturePro(tex.texture2D, src, dest, new Vector2(0, 0), 0, Color.White);
    }

    public static void drawSpriteFromSheet(OctoSpriteSheet sheet, Rectangle dest, int spriteNr)
    {
        var width = sheet.tex.width;
        var spriteSize = width / sheet.columns;
        var row = spriteNr / sheet.columns;
        var column = spriteNr % sheet.columns;
        var src = new Rectangle(column * spriteSize, row * spriteSize, spriteSize, spriteSize);
        Raylib.DrawTexturePro(sheet.tex.texture2D, src, dest, new Vector2(0, 0), 0, Color.White);
    }
}