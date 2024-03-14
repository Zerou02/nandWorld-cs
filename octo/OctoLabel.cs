using Raylib_cs;
public class OctoLabel : OctoComp
{
    public string text;
    public Rectangle rectangle;
    public Color color = Color.White;
    public OctoLabel(Rectangle rectangle, string text)
    {
        this.rectangle = rectangle;
        this.text = text;
    }

    public void draw(OctoState state)
    {
        Drawing.drawRectangle(rectangle, Color.Gray);
        Drawing.fillTextInRectangle(text, rectangle, color);
    }

}