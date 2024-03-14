using Raylib_cs;
public delegate void CallbackFn();
public class OctoButton : OctoComp
{
    public Rectangle rectangle = new Rectangle(0, 0, 0, 0);
    public string text = "";
    public Color btnColour = Color.Gray;
    public Color textColour = Color.White;
    public CallbackFn callbackFn = () => { };
    public Color hoverColor = Color.Blue;
    public bool isHover = false;
    public bool isPressed = false;
    public VisualTextMode textMode = VisualTextMode.LeftAligned;

    public OctoButton(Rectangle rectangle, string text, Color btnColour, Color textColour, CallbackFn callbackFn)
    {
        this.rectangle = rectangle;
        this.text = text;
        this.btnColour = btnColour;
        this.textColour = textColour;
        this.callbackFn = callbackFn;
        hoverColor = btnColour;
    }
    public OctoButton(Rectangle rectangle, string text, CallbackFn callbackFn)
    {
        this.rectangle = rectangle;
        this.text = text;
        this.callbackFn = callbackFn;
    }

    public void draw(OctoState state)
    {
        Drawing.drawRectangle(rectangle, isHover ? hoverColor : btnColour);
        Drawing.fillTextInRectangle(text, rectangle, textColour, 25);
    }

    public void process(OctoState state)
    {
        isHover = OctoMath.mouseInRec(rectangle);
        if (state.leftPressed && isHover)
        {
            isPressed = true;
            callbackFn();
        }
        else
        {
            isPressed = false;
        }
    }
}