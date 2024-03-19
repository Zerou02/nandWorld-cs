public class OctoSpriteSheet
{
    public SpriteTex tex;
    public int columns;
    public int rows;

    public OctoSpriteSheet(SpriteTex sprite, int columns, int rows)
    {
        this.columns = columns;
        this.rows = rows;
        this.tex = sprite;
    }
}