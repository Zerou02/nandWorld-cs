using Raylib_cs;
public class Audio : OctoComp
{
    public List<Sound> sounds = new List<Sound>();
    public Dictionary<string, Sound> soundMap = new Dictionary<string, Sound>();
    public List<string> soundQueue = new List<string>();
    public Audio()
    {
        foreach (var x in Directory.EnumerateFiles("./octo/sounds"))
        {
            var fileName = x.Split("/").Last().Replace(".mp3", "");
            soundMap.Add(fileName, Raylib.LoadSound(x));
        }
    }

    public void process(OctoState octoState)
    {
        /* var newList = new List<string>();
        for (int i = 0; i < soundQueue.Count; i++)
        {
            if (!playSound(soundQueue[i]))
            {
                newList.Add(soundQueue[i]);
            };
        }
        soundQueue = newList; */
    }

    public bool playSound(string name)
    {
        if (!Raylib.IsSoundPlaying(soundMap[name]))
        {
            Raylib.PlaySound(soundMap[name]);
            return true;
        }
        else
        {
            //       enqueueSound(name);
            return false;
        }
    }

    public void enqueueSound(string name)
    {
        soundQueue.Add(name);
    }
}