using System.Numerics;
using Raylib_cs;

class Main
{
    IComponent? draggedComponent = null;
    IComponent? selectedComp = null;
    Pin? startPin = null;
    List<IComponent> components = new List<IComponent>();
    List<Button> buttons = new List<Button>();

    Vector2 mousePos = new Vector2();
    Vector2 mouseDelta = new Vector2();

    bool uiSelected = false;
    TextInput textInput;
    List<List<IComponent>> views = new List<List<IComponent>>();
    OctoState state = new OctoState();
    Camera camera = new Camera();
    List<OctoComp> octoComps = new List<OctoComp>();
    Grid grid;
    public Main()
    {
        Raylib.InitWindow(800, 480, "Definitiv Octodot");
        Raylib.SetTargetFPS(60);
        state.screenSize = new Vector2(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
        textInput = new TextInput(new Rectangle(700, 150, 100, 20));
        textInput.text = "and";

        grid = new Grid(state);
        var nand = new Nand();
        var nand2 = new Nand();
        spawnComp(nand);
        spawnComp(nand2);
        nand.connectPin(2, nand2.getPins()[0]);

        var btn = new Button(new Rectangle(700, 0, 100, 20), "create Nand", () => { spawnComp(new Nand()); });
        var btn2 = new Button(new Rectangle(700, 30, 100, 20), "create Input ", () => { spawnComp(new Input()); });
        var btn3 = new Button(new Rectangle(700, 60, 100, 20), "create Output", () => { spawnComp(new Output()); });
        var btn4 = new Button(new Rectangle(700, 90, 100, 20), "save         ", () => { save(); });
        var btn5 = new Button(new Rectangle(700, 120, 100, 20), "load        ", () => { spawnComp(Utils.Load(textInput.text)); });

        octoComps.Add(btn);
        octoComps.Add(btn2);
        octoComps.Add(btn3);
        octoComps.Add(btn4);
        octoComps.Add(btn5);
        octoComps.Add(textInput);
        octoComps.Add(grid);


        var testCable = new Cable();
        testCable.grid = this.grid;
        var img = Raylib.LoadImage("assets/sprites/wires.png");
        var tex = Raylib.LoadTextureFromImage(img);
        while (!Raylib.WindowShouldClose())
        {
            testCable.draw();
            state.process();
            camera.process();
            grid.draw(state);
            octoComps.ForEach(x => { x.process(state); x.draw(); });
            handleInput();
            mousePos = Raylib.GetMousePosition();
            mouseDelta = Raylib.GetMouseDelta();

            Raylib.BeginDrawing();

            Raylib.ClearBackground(Color.Beige);
            components.ForEach(x =>
            {
                //      x.alignToCamera(camera);
                x.process(state);
                x.draw();
            });
            drawPinMenu(selectedComp);
            if (startPin != null) { Raylib.DrawLineV(OctoMath.getCentreOfCircle(startPin.bounds), mousePos, Color.Black); }
            buttons.ForEach(x => { x.draw(); });

            if (state.leftDown)
            {
                textInput.selected = OctoMath.isPointInRec(mousePos, textInput.rectangle);
                uiSelected = textInput.selected;
            }
            if (textInput.selected)
            {
                var pressed = Raylib.GetKeyPressed();
                if (pressed != 0)
                {
                    var sign = OctoUtils.keyCodeToAscii(state, pressed);
                    if (OctoUtils.isAlphaNumeric(sign))
                    {
                        textInput.text += sign;
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
            var back = new Rectangle(775, 200, 25, 25);
            Drawing.drawRectangle(back, views.Count == 0 ? Color.Gray : Color.Maroon);
            if (state.leftPressed && OctoMath.mouseInRec(back) && views.Count > 0)
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
        Console.WriteLine("a--");
        component.alignSizeToGrid(grid);
        component.alignPins();
        components.Add(component);
    }


    void drawPinMenu(IComponent component)
    {
        if (selectedComp == null) { return; }

        var box = new Rectangle(0, 400, 800, 80);
        var exit = new Rectangle(775, 400, 25, 25);
        var inspect = new Rectangle(750, 400, 25, 25);
        var isBaseComp = component.getType() == "nand" || component.getType() == "input" || component.getType() == "output";
        Drawing.drawRectangle(box, Color.Purple);

        Drawing.drawRectangle(exit, Color.Red);
        if (!isBaseComp)
        {
            Drawing.drawRectangle(inspect, Color.Blue);
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
            if (Raylib.IsMouseButtonPressed(MouseButton.Left) && OctoMath.isPointInRec(Raylib.GetMousePosition(), newRect))
            {
                pins[i].setState(!pins[i].state);
            }
            Drawing.drawCircle(newRect, pins[i].state ? Color.Green : Color.Red);
        }
        if (Raylib.IsMouseButtonPressed(MouseButton.Left) && OctoMath.isPointInRec(Raylib.GetMousePosition(), exit))
        {
            selectedComp = null;
        }
        if (state.leftPressed && OctoMath.mouseInRec(inspect) && !isBaseComp)
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

    void handleInput()
    {

        var mousePos = Raylib.GetMousePosition();
        var mouseDelta = Raylib.GetMouseDelta();
        buttons.ForEach(x =>
        {
            if (OctoMath.isPointInRec(mousePos, x.rectangle))
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
                if (OctoMath.isPointInRec(mousePos, x.getBounds()))
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
                    if (Raylib.CheckCollisionPointCircle(mousePos, OctoMath.getCentreOfCircle(y.bounds), y.bounds.Width / 2))
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
                    if (Raylib.CheckCollisionPointCircle(mousePos, OctoMath.getCentreOfCircle(y.bounds), y.bounds.Width / 2))
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
                    var comp = (IComponent)outPin.baseComponent;
                    if (!comp.wouldBeCyclic(inPin.baseComponent))
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
            if (draggedComponent != null && Raymath.Vector2Length(mouseDelta) > 1) { draggedComponent.setPosition(mousePos); draggedComponent.alignToGrid(grid); }
        }
        if (Raylib.IsMouseButtonReleased(MouseButton.Left))
        {
            draggedComponent = null;
        }
    }
}