public class Utils
{
    /*     public static void save(IComponent component)
        {
            var idMap = new Dictionary<Pin, string>();
            var type = component.getType();
            var components = component.getComponents();
            var savString = "name:" + type + "\ncomponents:";
            for (int i = 0; i < components.Count; i++)
            {
                savString += components[i].getType();
                if (i != components.Count - 1)
                {
                    savString += ",";
                }
                var pins = components[i].getPins();
                for (int j = 0; j < pins.Length; j++)
                {
                    var key = i.ToString() + "/" + j.ToString();
                    idMap.Add(pins[j], key);
                }
            }
            savString += "\nconnections:";
            foreach (var comp in components)
            {
                foreach (var pin in comp.getPins())
                {
                    pin.connectedOuts.ForEach(x =>
                    {
                        if (idMap.ContainsKey(pin) && idMap.ContainsKey(x))
                        {
                            savString += idMap[pin] + "," + idMap[x] + ";";
                        }
                    });
                }
            }
            var writer = new StreamWriter("assets/saves/" + type + ".comp");
            writer.Write(savString);
            writer.Flush();
            writer.Close();
        }

        public static IComponent Load(string name)
        {
            var path = "assets/saves/" + name + ".comp";
            var data = new StreamReader(path).ReadToEnd();
            var retComp = new Component(name);
            var nameComp = data.Split("components:");
            var compConn = nameComp[1].Split("connections:");
            var comps = compConn[0].Trim();
            var conns = compConn[1].Trim();
            var idMap = new Dictionary<string, Pin>();
            var compsSplit = comps.Split(",");
            for (int i = 0; i < compsSplit.Length; i++)
            {
                IComponent comp = null;
                if (compsSplit[i] == "input")
                {
                    comp = new Input();
                    foreach (var x in comp.getPins()) { x.isComposite = true; }
                }
                else if (compsSplit[i] == "nand")
                {
                    comp = new Nand();
                    foreach (var x in comp.getPins()) { x.isInner = true; }

                }
                else if (compsSplit[i] == "output")
                {
                    comp = new Output();
                    foreach (var x in comp.getPins()) { x.isComposite = true; }
                }
                else
                {
                    comp = Utils.Load(compsSplit[i]);
                    foreach (var x in comp.getPins()) { x.isInner = true; }
                }
                comp.setBaseComp(retComp);
                retComp.addComp(comp);
                var pins = comp.getPins();
                for (int j = 0; j < pins.Length; j++)
                {
                    var key = i.ToString() + "/" + j.ToString();
                    idMap.Add(key, pins[j]);
                }
            }
            foreach (var entry in conns.Split(";"))
            {
                if (entry != "")
                {
                    var entrySplit = entry.Split(",");
                    idMap[entrySplit[0]].connectedOuts.Add(idMap[entrySplit[1]]);
                }
            }
            return retComp;
        } */
}