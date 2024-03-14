using Raylib_cs;

public class TextureLoader
{

    public Dictionary<string, SpriteTex> sprites = new Dictionary<string, SpriteTex>();
    public TextureLoader()
    {
        loadTex("assets/sprites/wires.png");
    }

    public bool loadTex(string path)
    {
        Image? img = Raylib.LoadImage(path);
        if (img == null)
        {
            return false;
        }
        var tex = Raylib.LoadTextureFromImage((Image)img);
        sprites.Add(path, new SpriteTex((Image)img, tex));
        return true;
    }
}