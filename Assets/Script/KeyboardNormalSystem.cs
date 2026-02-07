using UnityEngine;

public class KeyboardSystemNormal : MonoBehaviour
{
    private char bufferedChar;
    private bool hasChar;

    public void Push(char c)
    {
        bufferedChar = c;
        hasChar = true;
    }

    public bool TryGet(out char c)
    {
        if (!hasChar)
        {
            c = '\0';
            return false;
        }

        c = bufferedChar;
        hasChar = false;
        return true;
    }
}
