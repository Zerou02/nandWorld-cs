using System.Numerics;
using Raylib_cs;

public interface IComponent
{
    public string getType();
    public List<IComponent> getComponents();
    public void eval();

    public Pin[] getInputPins();
    public Pin[] getOutputPins();
    public Pin[] getPins();
    public void print();
    public void connectPin(int idx, Pin pin);
    Rectangle getBounds();
    void move(Vector2 vec);
    void setPosition(Vector2 vec);
    // void alignPins();
}