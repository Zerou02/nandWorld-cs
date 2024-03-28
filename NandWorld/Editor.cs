using System.Numerics;
using Raylib_cs;

public class Editor : OctoScene
{
    public List<BaseComponent> nandWorldComps = new List<BaseComponent>();
    BaseComponent draggedComponent;
    BaseComponent selectedComp;

    Grid grid;
    Pin startPin;
    List<List<BaseComponent>> views = new List<List<BaseComponent>>();
    List<Cable> cables = new List<Cable>();
    OctoButton backBtn;
    int currCableType = 0;
    OctoSpriteSheet wireSheet;
    public Editor(Octo octo) : base(octo)
    {
        var wireTex = octo.textureLoader.getTex("wires");
        wireSheet = new OctoSpriteSheet(wireTex, 16, 8);
        //  var btn = new OctoButton(new Rectangle(700, 0, 100, 20), "create Nand", () => { spawnComp(new Nand()); });
        //  var btn2 = new OctoButton(new Rectangle(700, 30, 100, 20), "create Input ", () => { spawnComp(new Input()); });
        //  var btn3 = new OctoButton(new Rectangle(700, 60, 100, 20), "create Output", () => { spawnComp(new Output()); });
        var btn4 = new OctoButton(new Rectangle(700, 90, 100, 20), "save         ", () => { save(); });
        //var btn5 = new OctoButton(new Rectangle(700, 120, 100, 20), "load        ", () => { spawnComp(Utils.Load("and")); });
        var cable1Btn = new TextureBtn(new Rectangle(775, 250, 25, 25), wireSheet, 0, () => { currCableType = 0; });
        var cable2Btn = new TextureBtn(new Rectangle(775, 275, 25, 25), wireSheet, 1 * 2 * 16, () => { currCableType = 1; });
        var cable3Btn = new TextureBtn(new Rectangle(775, 300, 25, 25), wireSheet, 2 * 2 * 16, () => { currCableType = 2; });
        var cable4Btn = new TextureBtn(new Rectangle(775, 325, 25, 25), wireSheet, 3 * 2 * 16, () => { currCableType = 3; });


        backBtn = new OctoButton(new Rectangle(775, 200, 25, 25), "", () =>
        {
            /* if (views.Count > 0)
            {
                nandWorldComps = views[views.Count - 1];
                views.RemoveAt(views.Count - 1);
            } */
        }
        );

        var textInput = new TextInput(new Rectangle(700, 150, 100, 20));
        this.octo = octo;
        textInput.text = "and";
        grid = new Grid(octo.state);
        //spawnComp(new Nand());
        //addComp(btn, btn2, btn3, btn4, btn5, backBtn, cable1Btn, cable2Btn, cable3Btn, cable4Btn);
        //addComp(textInput);
        var c1 = new Cable(grid, wireSheet, 0);
        foreach (var x in c1.pins)
        {
            Console.WriteLine(x.GetHashCode());
        }
        var nand = new Nand();
        nandWorldComps.Add(nand);
        nandWorldComps.Add(new Nand());
    }

    public override void process(OctoState state)
    {
        handleInput();
        state.leftClickHandled = draggedComponent != null;
        base.process(state);
        grid.process(state);
        if (grid.currSelectedField != null)
        {
            var adjacentSameCables = new List<Cable>();
            var contains = false;
            var field = (Vector2)grid.currSelectedField;
            foreach (var x in cables)
            {
                if (x.path.Contains(field))
                {
                    contains = true;
                    break;
                }
                if (x.cableType == currCableType && x.isAdjacentTo(field))
                {
                    adjacentSameCables.Add(x);
                }
            };
            if (contains) { return; }
            if (adjacentSameCables.Count == 0)
            {
                var x = new Cable(grid, wireSheet, currCableType);
                x.add(field);
                cables.Add(x);
                addComp(x);
                foreach (var y in nandWorldComps)
                {
                    foreach (var z in y.pins)
                    {
                        var pos = grid.getGridPos((int)z.bounds.X, (int)z.bounds.Y);
                        if (x.contains(pos))
                        {
                            x.addPin(z);
                        }
                    }
                }
            }
            else if (adjacentSameCables.Count == 1)
            {
                adjacentSameCables[0].add(field);
                var x = adjacentSameCables[0];
                foreach (var y in nandWorldComps)
                {
                    foreach (var z in y.pins)
                    {
                        var pos = grid.getGridPos((int)z.bounds.X, (int)z.bounds.Y);
                        if (x.contains(pos))
                        {
                            x.addPin(z);
                        }
                    }
                }
            }
            else
            {
                var mainCable = adjacentSameCables[0];
                mainCable.add(field);
                for (int i = 1; i < adjacentSameCables.Count; i++)
                {
                    foreach (var x in adjacentSameCables[i].path)
                    {
                        mainCable.add(x);
                    }
                    removeComponent(adjacentSameCables[i]);
                    cables.Remove(adjacentSameCables[i]);
                }
                var xx = mainCable;
                foreach (var y in nandWorldComps)
                {
                    foreach (var z in y.pins)
                    {
                        var pos = grid.getGridPos((int)z.bounds.X, (int)z.bounds.Y);
                        if (xx.contains(pos))
                        {
                            xx.addPin(z);
                        }
                    }
                }
            }
        }
        // backBtn.btnColour = views.Count == 0 ? Color.Gray : Color.Maroon;
        // nandWorldComps.ForEach(x => x.process(state));
    }

    public override void draw(OctoState state)
    {
        grid.draw(state);
        base.draw(state);
        nandWorldComps.ForEach(x => x.draw(state));
        if (startPin != null) { Raylib.DrawLineV(OctoMath.getCentreOfCircle(startPin.bounds), octo.state.mousePos, Color.Black); }
        drawPinMenu(selectedComp);
    }

    void save()
    {
        // var newComp = new Component("and");
        //this.nandWorldComps.ForEach(x => newComp.addComp(x));
        //Utils.save(newComp);
    }


    /* void spawnComp(IComponent component)
    {
        component.alignSizeToGrid(grid);
        component.alignPins();
        this.nandWorldComps.Add(component);
    } */

    void handleInput()
    {
        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            foreach (var x in nandWorldComps)
            {
                if (OctoMath.isPointInRec(octo.state.mousePos, x.bounds))
                {
                    draggedComponent = x;
                    x.isSelected = true;
                    selectedComp = draggedComponent;
                    foreach (var y in draggedComponent.pins)
                    {
                        y.removeFromCables();
                    }
                    foreach (var y in draggedComponent.getInputPins())
                    {
                        y.setState(false);
                    }

                    break;
                }
            }
        }
        if (Raylib.IsMouseButtonPressed(MouseButton.Right))
        {
            foreach (var x in nandWorldComps)
            {
                foreach (var y in x.getPins())
                {
                    if (Raylib.CheckCollisionPointCircle(octo.state.mousePos, OctoMath.getCentreOfCircle(y.bounds), y.bounds.Width / 2))
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
            foreach (var x in nandWorldComps)
            {
                foreach (var y in x.getPins())
                {
                    if (Raylib.CheckCollisionPointCircle(octo.state.mousePos, OctoMath.getCentreOfCircle(y.bounds), y.bounds.Width / 2))
                    {
                        foundPin = y;
                        break;
                    }
                }
            }
            /* if (startPin != null && foundPin != null)
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
            startPin = null; */
        }
        if (Raylib.IsMouseButtonDown(MouseButton.Left))
        {
            if (draggedComponent != null && Raymath.Vector2Length(octo.state.mouseDelta) > 1)
            {
                draggedComponent.setPosition(octo.state.mousePos);
                draggedComponent.alignToGrid(grid);
            }
        }
        if (Raylib.IsMouseButtonReleased(MouseButton.Left))
        {
            if (draggedComponent != null)
            {
                foreach (var y in draggedComponent.pins)
                {
                    var pos = grid.getGridPos((int)y.bounds.Position.X, (int)y.bounds.Position.Y);
                    foreach (var x in cables)
                    {
                        if (x.contains(pos))
                        {
                            x.addPin(y);
                        }
                    }
                }
            }
            draggedComponent = null;
        }
    }

    void drawPinMenu(BaseComponent component)
    {
        if (selectedComp == null) { return; }

        var box = new Rectangle(0, octo.state.screenSize.Y - 80, octo.state.screenSize.X, 80);
        var exit = new Rectangle(775, box.Position.Y, 25, 25);
        var inspect = new Rectangle(750, box.Position.Y, 25, 25);
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
        rects = Flexbox.alignLeft(new Rectangle(50, box.Position.Y + 30, 700, 50), rects);
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
        if (octo.state.leftPressed && OctoMath.mouseInRec(inspect) && !isBaseComp)
        {
            //var comp = Utils.Load(component.getType());
            views.Add(nandWorldComps);
            nandWorldComps = new List<BaseComponent>();
            var nodeMap = new Dictionary<BaseComponent, int>();

            /*    foreach (var x in comp.getComponents())
               {
                   x.setHighestComp(x);
                   nodeMap.Add(x, x.getType() == "input" ? 1 : -1);
                   foreach (var y in x.getPins()) { y.isInner = false; }
                   nandWorldComps.Add(x);
               }
               for (int i = 1; i < comp.getComponents().Count + 1; i++)
               {
                   var list = new List<BaseComponent>();
                   foreach (var x in nodeMap.Keys)
                   {
                       if (nodeMap[x] == i)
                       {
                           list.Add(x);
                       }
                   }
                   var map = new HashSet<BaseComponent>();
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
           } */
        }
    }
}