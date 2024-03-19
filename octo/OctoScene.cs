public class OctoScene : OctoComp
{
    public Octo octo;
    public List<OctoComp> comps = new List<OctoComp>();
    public OctoScene(Octo octo, Octo octo1)
    {
        this.octo = octo;
    }

    public OctoScene(Octo octo)
    {
        this.octo = octo;
    }

    public void addComp(params OctoComp[] comps)
    {
        foreach (var x in comps)
        {
            this.comps.Add(x);
        }
    }

    public void removeComponent(OctoComp comp)
    {
        comps.Remove(comp);
    }

    public virtual void process(OctoState state)
    {
        comps.ForEach(x => x.process(state));
    }

    public virtual void draw(OctoState state)
    {
        comps.ForEach(x => x.draw(state));
    }
}