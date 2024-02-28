using Raylib_cs;

public delegate void CallbackFn();
public class Button
{
    public Rectangle rectangle;
    public string text;
    public Color btnColour;
    public Color textColour;
    public CallbackFn callbackFn;
    public Color hoverColor;
    public bool isHover = false;
    public Button(Rectangle rectangle, string text, Color btnColour, Color textColour, CallbackFn callbackFn)
    {
        this.rectangle = rectangle;
        this.text = text;
        this.btnColour = btnColour;
        this.textColour = textColour;
        this.callbackFn = callbackFn;
        hoverColor = btnColour;
    }
}