using System.Collections;
using System.Net;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using Raylib_cs;

class Main
{
    float scrollSpeed = 2f;
    IComponent? draggedComponent = null;
    IComponent? selectedComp = null;
    Pin? startPin = null;
    List<IComponent> components = new List<IComponent>();
    List<Button> buttons = new List<Button>();

    Vector2 mousePos = new Vector2();
    Vector2 mouseDelta = new Vector2();
    bool leftDown = false;
    bool shiftPressd = false;
    bool capsLockPressed = false;
    bool uiSelected = false;
    TextInput textInput;
    List<List<IComponent>> views = new List<List<IComponent>>();
    public Main()
    {
        Raylib.InitWindow(800, 480, "Definitiv Godot");
        Raylib.SetTargetFPS(60);

        var inputBox = new Rectangle(700, 150, 100, 20);
        textInput = new TextInput(inputBox);
        textInput.text = "and";

        var nand = new Nand();
        var nand2 = new Nand();
        components.Add(nand);
        components.Add(nand2);
        nand.connectPin(2, nand2.getPins()[0]);

        var btn = new Button(new Rectangle(700, 0, 100, 20), "create Nand", Color.Gray, Color.White, () => { components.Add(new Nand()); });
        btn.hoverColor = Color.Blue;
        var btn2 = new Button(new Rectangle(700, 30, 100, 20), "create Input ", Color.Gray, Color.White, () => { components.Add(new Input()); });
        var btn3 = new Button(new Rectangle(700, 60, 100, 20), "create Output", Color.Gray, Color.White, () => { components.Add(new Output()); });
        var btn4 = new Button(new Rectangle(700, 90, 100, 20), "save         ", Color.Gray, Color.White, () => { save(); });
        var btn5 = new Button(new Rectangle(700, 120, 100, 20), "load        ", Color.Gray, Color.White, () => { spawnComp(Utils.Load(textInput.text)); });

        btn2.hoverColor = Color.Blue;
        btn3.hoverColor = Color.Blue;
        btn4.hoverColor = Color.Blue;
        btn5.hoverColor = Color.Blue;


        buttons.Add(btn);
        buttons.Add(btn2);
        buttons.Add(btn3);
        buttons.Add(btn4);
        buttons.Add(btn5);


        Console.WriteLine(Utils.Load("and").getPins()[0].baseComponent);
        while (!Raylib.WindowShouldClose())
        {
            shiftPressd = Raylib.IsKeyDown(KeyboardKey.LeftShift) || Raylib.IsKeyDown(KeyboardKey.RightShift);
            if (Raylib.IsKeyPressed(KeyboardKey.CapsLock))
            {
                capsLockPressed = !capsLockPressed;
            }

            handleInput();
            mousePos = Raylib.GetMousePosition();
            mouseDelta = Raylib.GetMouseDelta();
            leftDown = Raylib.IsMouseButtonDown(MouseButton.Left);

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Beige);
            var camVec = getCameraMoveVec();
            components.ForEach(x =>
            {
                if (!uiSelected)
                {
                    x.move(camVec);
                }
                drawComp(x);
            });
            drawPinMenu(selectedComp);
            if (startPin != null) { Raylib.DrawLineV(getCentreOfCircle(startPin.bounds), mousePos, Color.Black); }
            buttons.ForEach(x => { drawButton(x); });

            drawRectangle(inputBox, Color.White);
            drawTextInRectangle(textInput.text, inputBox, Color.Black, 12);
            if (isPointInRec(mousePos, inputBox))
            {
                Raylib.SetMouseCursor(MouseCursor.IBeam);
            }
            else
            {
                Raylib.SetMouseCursor(MouseCursor.Default);
            }
            if (leftDown)
            {
                textInput.selected = isPointInRec(mousePos, textInput.rectangle);
                uiSelected = textInput.selected;
            }
            if (textInput.selected)
            {
                var pressed = Raylib.GetKeyPressed();
                if (pressed != 0)
                {
                    Console.WriteLine(pressed);
                    var sign = keyCodeToAscii(pressed);
                    if (isAlphaNumeric(sign))
                    {
                        textInput.text += keyCodeToAscii(pressed);
                    }
                }
                if (Raylib.IsKeyPressed(KeyboardKey.Backspace) || Raylib.IsKeyPressedRepeat(KeyboardKey.Backspace))
                {
                    if (textInput.text.Count() > 0)
                    {
                        textInput.text = textInput.text.Remove(textInput.text.Count() - 1);
                    }
                }
            }
            if (isPointInRec(mousePos, textInput.rectangle))
            {
                Raylib.SetMouseCursor(MouseCursor.IBeam);
            }
            else
            {
                Raylib.SetMouseCursor(MouseCursor.Default);
            }

            var back = new Rectangle(775, 200, 25, 25);
            drawRectangle(back, views.Count == 0 ? Color.Gray : Color.Maroon);
            if (buttonPressed(back) && views.Count > 0)
            {
                components = views[views.Count - 1];
                views.RemoveAt(views.Count - 1);
            }
            Raylib.DrawFPS(0, 0);


            Raylib.EndDrawing();

        }
        Raylib.CloseWindow();
    }

    void save()
    {
        var newComp = new Component(textInput.text);
        this.components.ForEach(x =>
        {
            newComp.addComp(x);
        });
        Utils.save(newComp);
    }


    void spawnComp(IComponent component)
    {
        alignPins(component);
        components.Add(component);
    }
    void drawButton(Button button)
    {
        drawRectangle(button.rectangle, button.isHover ? button.hoverColor : button.btnColour);
        drawTextInRectangle(button.text, button.rectangle, button.textColour, 25);
    }

    void drawPinMenu(IComponent component)
    {
        if (selectedComp == null) { return; }

        var box = new Rectangle(0, 400, 800, 80);
        var exit = new Rectangle(775, 400, 25, 25);
        var inspect = new Rectangle(750, 400, 25, 25);
        var isBaseComp = component.getType() == "nand" || component.getType() == "input" || component.getType() == "output";
        drawRectangle(box, Color.Purple);

        drawRectangle(exit, Color.Red);
        if (!isBaseComp)
        {
            drawRectangle(inspect, Color.Blue);
        }

        var pins = component.getPins();
        var rects = new List<Rectangle>();
        foreach (var x in pins)
        {
            rects.Add(x.bounds);
        }
        rects = Flexbox.alignLeft(new Rectangle(50, 430, 700, 50), rects);
        for (int i = 0; i < (component.getType() == "input" ? 1 : component.getType() == "output" ? 0 : component.getInputPins().Length); i++)
        {

            var newRect = new Rectangle(rects[i].Position + i * new Vector2(20, 0), rects[i].Size);
            if (Raylib.IsMouseButtonPressed(MouseButton.Left) && isPointInRec(Raylib.GetMousePosition(), newRect))
            {
                pins[i].setState(!pins[i].state);
            }
            drawCircle(newRect, pins[i].state ? Color.Green : Color.Red);
        }
        if (Raylib.IsMouseButtonPressed(MouseButton.Left) && isPointInRec(Raylib.GetMousePosition(), exit))
        {
            selectedComp = null;
        }
        if (buttonPressed(inspect) && !isBaseComp)
        {
            var comp = Utils.Load(component.getType());
            views.Add(components);
            components = new List<IComponent>();
            var nodeMap = new Dictionary<IComponent, int>();

            foreach (var x in comp.getComponents())
            {
                x.setHighestComp(x);
                nodeMap.Add(x, x.getType() == "input" ? 1 : -1);
                foreach (var y in x.getPins()) { y.isInner = false; }
                components.Add(x);
            }
            for (int i = 1; i < comp.getComponents().Count + 1; i++)
            {
                var list = new List<IComponent>();
                foreach (var x in nodeMap.Keys)
                {
                    if (nodeMap[x] == i)
                    {
                        list.Add(x);
                    }
                }
                var map = new HashSet<IComponent>();
                list.ForEach(x =>
                {
                    foreach (var y in x.getConnectedHighestOuts())
                    {
                        map.Add(y);
                    }
                });
                foreach (var x in map)
                {
                    nodeMap[x] = i + 1;
                }
            }
            foreach (var x in nodeMap.Keys)
            {
                Console.WriteLine(x.GetType() + "; " + nodeMap[x]);
            }
            for (int i = 1; i < component.getComponents().Count + 1; i++)
            {
                var list = new List<IComponent>();
                foreach (var x in nodeMap.Keys)
                {
                    if (nodeMap[x] == i)
                    {
                        list.Add(x);
                    }
                }
                for (int j = 0; j < list.Count; j++)
                {
                    list[j].setPosition(new Vector2(i * 75, (j + 1) * 50));
                }
            }

            selectedComp = null;
        }
    }

    bool buttonPressed(Rectangle rectangle)
    {
        return Raylib.IsMouseButtonPressed(MouseButton.Left) && isPointInRec(Raylib.GetMousePosition(), rectangle);
    }
    bool isAlphaNumeric(char c)
    {
        return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9');
    }
    char keyCodeToAscii(int keycode)
    {
        // keycode 65 = a = Ascii A; 
        var retChar = (char)keycode;
        Console.WriteLine(capsLockPressed || shiftPressd);

        if (!(capsLockPressed || shiftPressd) && keycode >= 65)
        {
            retChar = (char)(keycode - ('A' - 'a'));
        }
        Console.WriteLine(retChar);
        return retChar;
    }
    void handleInput()
    {

        var mousePos = Raylib.GetMousePosition();
        var mouseDelta = Raylib.GetMouseDelta();
        buttons.ForEach(x =>
        {
            if (isPointInRec(mousePos, x.rectangle))
            {
                x.isHover = true;
                if (Raylib.IsMouseButtonPressed(MouseButton.Left))
                {
                    x.callbackFn();
                }
            }
            else
            {
                x.isHover = false;
            }
        });
        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            foreach (var x in components)
            {
                if (isPointInRec(mousePos, x.getBounds()))
                {
                    draggedComponent = x;
                    selectedComp = draggedComponent;
                    break;
                }
            }
        }
        if (Raylib.IsMouseButtonPressed(MouseButton.Right))
        {
            foreach (var x in components)
            {
                foreach (var y in x.getPins())
                {
                    if (Raylib.CheckCollisionPointCircle(mousePos, getCentreOfCircle(y.bounds), y.bounds.Width / 2))
                    {
                        startPin = y;
                        break;
                    }
                }
            }
        }
        if (Raylib.IsMouseButtonReleased(MouseButton.Right))
        {
            Pin? foundPin = null;
            foreach (var x in components)
            {
                foreach (var y in x.getPins())
                {
                    if (Raylib.CheckCollisionPointCircle(mousePos, getCentreOfCircle(y.bounds), y.bounds.Width / 2))
                    {
                        foundPin = y;
                        break;
                    }
                }
            }
            if (startPin != null && foundPin != null)
            {
                Pin? outPin = null;
                Pin? inPin = null;
                if ((startPin.isComposite && !startPin.isOut) || (!startPin.isComposite && startPin.isOut))
                {
                    outPin = startPin;
                }
                else
                {
                    inPin = startPin;
                }
                if ((foundPin.isComposite && !foundPin.isOut) || (!foundPin.isComposite && foundPin.isOut))
                {
                    outPin = foundPin;
                }
                else
                {
                    inPin = foundPin;
                }
                if (outPin != null && inPin != null)
                {
                    if (!wouldBeCyclic(outPin.baseComponent, inPin.baseComponent))
                    {
                        outPin.connectedOuts.Add(inPin);
                        outPin.setOuts();
                    }
                }
            }
            startPin = null;
        }
        if (Raylib.IsMouseButtonDown(MouseButton.Left))
        {
            if (draggedComponent != null) { draggedComponent.move(mouseDelta); }
        }
        if (Raylib.IsMouseButtonReleased(MouseButton.Left))
        {
            draggedComponent = null;
        }
    }

    bool queueContains(Queue<IComponent> queue, IComponent e)
    {
        var found = false;
        foreach (var x in queue)
        {
            if (x == e)
            {
                found = true;
                break;
            }
        }
        return found;
    }
    bool wouldBeCyclic(IComponent component, IComponent newComponent)
    {
        if (component == newComponent) { return true; }
        var found = false;
        var openList = new Queue<IComponent>() { };
        var closedList = new Queue<IComponent>() { };

        foreach (var x in newComponent.getOutputPins())
        {
            x.connectedOuts.ForEach(y =>
            {
                if (!queueContains(openList, y.baseComponent.getHighestComp()) && !queueContains(closedList, y.baseComponent.getHighestComp()))
                {
                    openList.Enqueue(y.baseComponent.getHighestComp());
                }
            });
        }
        while (openList.Count > 0 && !found)
        {
            var e = openList.Dequeue();
            if (e == component.getHighestComp())
            {
                found = true;
            }
            foreach (var x in e.getOutputPins())
            {
                x.connectedOuts.ForEach(y =>
                {
                    if (!queueContains(openList, y.baseComponent.getHighestComp()) && !queueContains(closedList, y.baseComponent.getHighestComp()))
                    {
                        openList.Enqueue(y.baseComponent.getHighestComp());
                    }
                });
            }
            closedList.Enqueue(e);
        }
        Console.WriteLine(found);
        return found;
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

    Vector2 getCameraMoveVec()
    {
        var cameraMovVec = new Vector2();
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
        return cameraMovVec;
    }

    static void alignPins(IComponent component)
    {
        var pin = component.getPins()[0];
        var tmpList = new List<Rectangle>();
        var inputs = component.getInputPins();
        var biggestPinSize = Math.Max(inputs.Length, component.getOutputPins().Length);
        component.setSize(new Vector2(pin.bounds.Size.X * 2, biggestPinSize * pin.bounds.Size.Y + 10));
        foreach (var x in inputs)
        {
            tmpList.Add(x.bounds);
        }
        var newRect = component.getBounds();
        newRect.X -= pin.bounds.Width / 2;
        var ordered = Flexbox.alignTop(newRect, tmpList);
        for (int i = 0; i < ordered.Count; i++)
        {
            inputs[i].bounds = ordered[i];
        }

        var outputs = component.getOutputPins();
        tmpList = new List<Rectangle>();
        foreach (var x in outputs)
        {
            tmpList.Add(x.bounds);
        }
        newRect = component.getBounds();
        newRect.X += newRect.Width - pin.bounds.Width / 2;
        ordered = Flexbox.alignTop(newRect, tmpList);
        for (int i = 0; i < ordered.Count; i++)
        {
            outputs[i].bounds = ordered[i];
        }

    }
    void drawComp(IComponent comp)
    {
        alignPins(comp);
        var bounds = comp.getBounds();
        var textBounds = bounds;
        textBounds.Y += textBounds.Height / 2;

        var inputs = comp.getPins();

        if (selectedComp != null && comp == selectedComp)
        {
            drawRectangleWithOutline(bounds, Color.Blue, Color.Black);
        }
        else
        {
            drawRectangle(bounds, Color.Blue);
        }
        for (int i = 0; i < inputs.Length; i++)
        {
            drawPin(inputs[i]);
        }
        drawTextInRectangle(comp.getType().ToString(), textBounds, Color.White);
    }

    void drawCircle(Rectangle rect, Color color)
    {
        Raylib.DrawCircleV(getCentreOfCircle(rect), rect.Width / 2, color);
    }
    void drawPin(Pin pin)
    {
        var bounds = pin.bounds;
        drawCircle(pin.bounds, pin.state ? Color.Green : Color.Red);
        pin.connectedOuts.ForEach(x =>
        {
            if (!(pin.isInner || x.isInner))
            {
                Raylib.DrawLineV(getCentreOfCircle(pin.bounds), getCentreOfCircle(x.bounds), Color.Black);
            }
        });
    }

    static Vector2 getCentreOfCircle(Rectangle rectangle)
    {
        return new Vector2(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2);
    }

    static void drawTextInRectangle(string text, Rectangle rectangle, Color color, int textSize = -1)
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
    static void drawHairCross()
    {
        var width = Raylib.GetScreenWidth();
        var height = Raylib.GetScreenHeight();
        var size = 100;
        var startPos = new Vector2(width / 2, height / 2);
        Raylib.DrawLine((int)startPos.X - size, (int)startPos.Y, (int)startPos.X + size, (int)startPos.Y, Color.White);
        Raylib.DrawLine((int)startPos.X, (int)startPos.Y - size, (int)startPos.X, (int)startPos.Y + size, Color.White);
    }

    static bool isPointInRec(Vector2 point, Rectangle rectangle)
    {
        return Raylib.CheckCollisionPointRec(point, rectangle);
    }
}