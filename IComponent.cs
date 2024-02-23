using System.Collections.Generic;

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
}