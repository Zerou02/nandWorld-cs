using System.Numerics;
using Raylib_cs;

class Main
{
    float scrollSpeed = 2f;
    IComponent? draggedComponent = null;
    IComponent? selectedComp = null;
    Pin? startPin = null;
    List<IComponent> components = new List<IComponent>();
    List<Button> buttons = new List<Button>();

    public Main()
    {
        Raylib.InitWindow(800, 480, "Definitiv Godot");
        Raylib.SetTargetFPS(60);

        var nand = new Nand();
        var nand2 = new Nand();
        components.Add(nand);
        components.Add(nand2);
        nand.connectPin(2, nand2.getPins()[0]);

        var btn = new Button(new Rectangle(700, 0, 100, 20), "create Nand", Color.Gray, Color.White, () => { components.Add(new Nand()); });
        btn.hoverColor = Color.Blue;
        var btn2 = new Button(new Rectangle(700, 30, 100, 20), "create Input ", Color.Gray, Color.White, () => { components.Add(new Input()); });
        var btn3 = new Button(new Rectangle(700, 60, 100, 20), "create Output", Color.Gray, Color.White, () => { components.Add(new Output()); });
        var btn4 = new Button(new Rectangle(700, 90, 100, 20), "save         ", Color.Gray, Color.White, () => { components.Add(new Input()); });
        btn2.hoverColor = Color.Blue;
        btn3.hoverColor = Color.Blue;
        btn4.hoverColor = Color.Blue;

        buttons.Add(btn);
        buttons.Add(btn2);
        buttons.Add(btn3);
        buttons.Add(btn4);

        while (!Raylib.WindowShouldClose())
        {
            handleInput();
            var mousePos = Raylib.GetMousePosition();
            var mouseDelta = Raylib.GetMouseDelta();

            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Beige);
            var camVec = getCameraMoveVec();
            components.ForEach(x =>
            {
                x.move(camVec);
                drawComp(x);
            });
            drawPinMenu(selectedComp);
            if (startPin != null) { Raylib.DrawLineV(getCentreOfCircle(startPin.bounds), mousePos, Color.Black); }
            buttons.ForEach(x => { drawButton(x); });

            Raylib.DrawFPS(0, 0);
            Raylib.EndDrawing();
        }
        Raylib.CloseWindow();
    }

    void drawButton(Button button)
    {
        drawRectangle(button.rectangle, button.isHover ? button.hoverColor : button.btnColour);
        drawTextInRectangle(button.text, button.rectangle, button.textColour);
    }

    void drawPinMenu(IComponent component)
    {
        if (selectedComp == null) { return; }

        var box = new Rectangle(0, 400, 800, 80);
        var exit = new Rectangle(775, 400, 25, 25);
        drawRectangle(box, Color.Purple);
        drawRectangle(exit, Color.Red);

        var pins = component.getPins();
        var rects = new List<Rectangle>();
        foreach (var x in pins)
        {
            rects.Add(x.bounds);
        }
        rects = Flexbox.alignLeft(new Rectangle(50, 430, 700, 50), rects);
        for (int i = 0; i < rects.Count; i++)
        {
            var newRect = new Rectangle(rects[i].Position + i * new Vector2(20, 0), rects[i].Size);
            if (Raylib.IsMouseButtonPressed(MouseButton.Left) && !pins[i].isOut && Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), newRect))
            {
                pins[i].setState(!pins[i].state);
            }
            drawCircle(newRect, pins[i].state ? Color.Green : Color.Red);
        }
        if (Raylib.IsMouseButtonPressed(MouseButton.Left) && Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), exit))
        {
            selectedComp = null;
        }
    }
    void handleInput()
    {

        var mousePos = Raylib.GetMousePosition();
        var mouseDelta = Raylib.GetMouseDelta();
        buttons.ForEach(x =>
        {
            if (Raylib.CheckCollisionPointRec(mousePos, x.rectangle))
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
                if (Raylib.CheckCollisionPointRec(mousePos, x.getBounds()))
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
            if (startPin != null)
            {
                if (foundPin != null && startPin.isOut != foundPin.isOut)
                {
                    var outPin = startPin.isOut ? startPin : foundPin;
                    var inPin = startPin.isOut ? foundPin : startPin;
                    outPin.connectedOuts.Add(inPin);
                }
                startPin = null;
            }
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
        drawRectangle(bounds, Color.Blue);
        var textBounds = bounds;
        textBounds.Y += textBounds.Height / 2;

        var inputs = comp.getPins();
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
            Raylib.DrawLineV(getCentreOfCircle(pin.bounds), getCentreOfCircle(x.bounds), Color.Black);
        });
    }

    static Vector2 getCentreOfCircle(Rectangle rectangle)
    {
        return new Vector2(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2);
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