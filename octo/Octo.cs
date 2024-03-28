using Raylib_cs;
using System.Numerics;
public class Octo
{
    public OctoState state = new OctoState();
    public List<OctoComp> comps = new List<OctoComp>();
    public Audio audio;
    public TextureLoader textureLoader;
    public Octo()
    {
        Raylib.InitWindow(800, 600, "Definitiv Octodot");

        Raylib.SetTargetFPS(60);
        Raylib.InitAudioDevice();
        state.screenSize = new Vector2(800, 600);
        audio = new Audio();
        textureLoader = new TextureLoader();
        textureLoader.loadTex("assets/sprites/wires.png");
        //comps.Add(audio);
    }

    public void addComp(params OctoComp[] comps)
    {
        foreach (var x in comps)
        {
            this.comps.Add(x);
        }
    }

    public void setScene(OctoScene scene)
    {
        this.comps = new List<OctoComp>() { scene };
    }

    public void start(CallbackFn callbackFn)
    {
        Raylib.SetMousePosition((int)state.screenSize.X / 2, (int)state.screenSize.Y / 2);
        //this.comps.Add(new Nand());
        while (!Raylib.WindowShouldClose())
        {
            callbackFn();
            state.process();
            comps.ForEach(x => x.process(state));
            Raylib.BeginDrawing();

            Raylib.ClearBackground(Color.Beige);
            comps.ForEach(x => { x.draw(state); });
            Raylib.DrawFPS(0, 0);
            Raylib.EndDrawing();
        }
    }
}