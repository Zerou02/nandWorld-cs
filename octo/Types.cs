using Raylib_cs;

public struct SpriteTex
{
    public int width;
    public int height;
    public Texture2D texture2D;
    public SpriteTex(Image image, Texture2D texture2D)
    {
        this.width = image.Width;
        this.height = image.Height;
        this.texture2D = texture2D;
    }

}
