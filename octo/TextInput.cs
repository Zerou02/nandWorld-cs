using System.Runtime.InteropServices;
using Raylib_cs;
public class TextInput : OctoComp
{
    public Rectangle rectangle;
    public string text = "";
    public string drawnText = "";
    public Color inputColour = Color.White;
    public Color textColour = Color.Black;
    public CallbackFn onHover;
    public CallbackFn onSubmit;
    public bool isFocused = false;
    bool showSeparator = false;
    int capacityExceededAt = int.MaxValue;
    public TextInput(Rectangle rectangle)
    {
        this.rectangle = rectangle;
    }

    public virtual void process(OctoState state)
    {

        Raylib.SetMouseCursor(OctoMath.mouseInRec(rectangle) ? MouseCursor.IBeam : MouseCursor.Default);
        if (state.leftDown)
        {
            isFocused = OctoMath.mouseInRec(rectangle);
        }
        if (isFocused)
        {
            if (Raylib.IsKeyPressed(KeyboardKey.Backspace) || Raylib.IsKeyPressedRepeat(KeyboardKey.Backspace))
            {
                if (text.Count() > 0)
                {
                    text = text.Remove(text.Count() - 1);
                }
            }
            var currKey = Raylib.GetKeyPressed();
            if (currKey != 0 && OctoUtils.isSaneAscii((char)currKey))
            {
                if (!(state.shiftPressed || state.leftPressed) && (currKey >= 'A' && currKey <= 'Z'))
                {
                    currKey -= ('A' - 'a');
                }
                text += (char)currKey;
            }
        }
        if (state.currFrame % 30 == 0 || !isFocused)
        {
            showSeparator = false;
        }
        else if (state.currFrame % 15 == 0)
        {
            showSeparator = true;
        }
        if (showSeparator)
        {
            drawnText = text + "|";
        }
        else
        {
            drawnText = text;
        }
        if (Raylib.IsKeyPressed(KeyboardKey.Enter))
        {
            onSubmit();
        }
    }

    public void draw(OctoState state)
    {
        Drawing.drawRectangle(rectangle, Color.White);
        Drawing.fillTextInRectangle(drawnText, rectangle, Color.Black, 36);
    }
}