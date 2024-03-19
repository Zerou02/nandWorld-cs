using Microsoft.VisualBasic;
using Raylib_cs;

public class TextureBtn : OctoButton
{
    OctoSpriteSheet spriteSheet;
    int sprite = 0;
    public TextureBtn(Rectangle rectangle, Color btnColour, Color textColour, CallbackFn callbackFn, OctoSpriteSheet sprite) : base(rectangle, "", btnColour, textColour, callbackFn)
    {
        this.spriteSheet = sprite;
    }
    public TextureBtn(Rectangle rectangle, OctoSpriteSheet sprite, int spriteNr, CallbackFn callbackFn) : base(rectangle, "", callbackFn)
    {
        this.spriteSheet = sprite;
        this.sprite = spriteNr;
    }

    public override void draw(OctoState state)
    {
        base.draw(state);
        Drawing.drawSpriteFromSheet(spriteSheet, this.rectangle, sprite);
    }
}