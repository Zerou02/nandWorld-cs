using Raylib_cs;

public struct SpriteTex
{
    public Image image;
    public Texture2D texture2D;
    public SpriteTex(Image image, Texture2D texture2D)
    {
        this.image = image;
        this.texture2D = texture2D;
    }

}
