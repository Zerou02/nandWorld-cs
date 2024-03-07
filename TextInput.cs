
using System.Buffers.Binary;
using System.Diagnostics;
using Raylib_cs;

class TextInput
{
    public Rectangle rectangle;
    public string text = "";
    public Color inputColour = Color.White;
    public Color textColour = Color.Black;
    public CallbackFn onHover;
    public bool selected;
    public TextInput(Rectangle rectangle)
    {
        this.rectangle = rectangle;
    }

}