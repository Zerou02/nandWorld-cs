public class OctoUtils
{
    public static bool queueContains(Queue<BaseComponent> queue, BaseComponent e)
    {
        var found = false;
        foreach (var x in queue)
        {
            if (x == e)
            {
                found = true;
                break;
            }
        }
        return found;
    }


    public static bool isSaneAscii(char c)
    {
        // space through ~
        return c >= 32 && c <= 126;
    }

    public static bool isAlphaNumeric(char c)
    {
        return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9');
    }
    public static char keyCodeToChar(OctoState state, int keycode)
    {
        // keycode 65 = a = Ascii A; 
        var retChar = (char)keycode;
        if (isSaneAscii((char)keycode))
        {

        }
        //if (!(state.capsLockPressed || state.shiftPressed) && keycode >= 65)
        //{
        //   retChar = (char)(keycode - ('A' - 'a'));
        // }
        return retChar;
    }
}