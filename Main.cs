class Main
{
    public Main()
    {
        var octo = new Octo();
        var ed = new Editor(octo);
        octo.setScene(ed);
        octo.start(() =>
        {
        });
    }

}