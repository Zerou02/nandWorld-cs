using System.Numerics;
using Raylib_cs;

public class Flexbox
{
    public static List<Rectangle> alignLeft(Rectangle bounds, List<Rectangle> items)
    {
        var retList = new List<Rectangle>();
        if (items.Count == 0) { return retList; }
        var size = items[0].Size;
        var width = size.X;
        if (bounds.Size.X < size.X * items.Count)
        {
            width = (bounds.Size.X - size.X) / (items.Count - 1);
        }
        for (int i = 0; i < items.Count; i++)
        {
            retList.Add(new Rectangle(new Vector2(bounds.Position.X + i * width, bounds.Position.Y), items[i].Size));
        }
        return retList;
    }

    public static List<Rectangle> alignTop(Rectangle bounds, List<Rectangle> items)
    {
        var retList = new List<Rectangle>();
        if (items.Count == 0) { return retList; }
        var size = items[0].Size;
        var height = size.Y;
        if (bounds.Size.Y < size.Y * items.Count)
        {
            height = (bounds.Size.Y - size.Y) / (items.Count - 1);
        }
        for (int i = 0; i < items.Count; i++)
        {
            retList.Add(new Rectangle(new Vector2(bounds.Position.X, bounds.Position.Y + i * height), items[i].Size));
        }
        return retList;
    }

    /* 
        public static void centerLabel(Rect2 bounds, Label label)
        {
            var spacing = 6;
            var letterWidth = 8;
            var sizeX = label.Text.Length * letterWidth + (label.Text.Length - 1) * spacing;
        } */
}