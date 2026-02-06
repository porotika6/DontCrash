using UnityEngine;
using System.Collections.Generic;

public class KeyboardMahjong : MonoBehaviour
{
    public List<KeyCode> keyboardOrder = new List<KeyCode>()
    {
        KeyCode.Q, KeyCode.A, KeyCode.Z,
        KeyCode.W, KeyCode.S, KeyCode.X,
        KeyCode.E, KeyCode.D, KeyCode.C,
        KeyCode.R, KeyCode.F, KeyCode.V,
        KeyCode.T, KeyCode.G, KeyCode.B,
        KeyCode.Y, KeyCode.H, KeyCode.N,
        KeyCode.U, KeyCode.J, KeyCode.M,
        KeyCode.I, KeyCode.K,
        KeyCode.O, KeyCode.L,
        KeyCode.P
    };

    public int visibleCount = 3;

    void Start()
    {
        Shuffle();
    }

    void Shuffle()
    {
        for (int i = 0; i < keyboardOrder.Count; i++)
        {
            int r = Random.Range(i, keyboardOrder.Count);
            var temp = keyboardOrder[i];
            keyboardOrder[i] = keyboardOrder[r];
            keyboardOrder[r] = temp;
        }
    }

    public KeyCode GetExpectedKey()
    {
        return keyboardOrder[0];
    }

    public List<KeyCode> GetVisibleKeys()
    {
        return keyboardOrder.GetRange(0, visibleCount);
    }

    public void ShiftKey(KeyCode key)
    {
        if (!keyboardOrder.Contains(key)) return;
        keyboardOrder.Remove(key);
        keyboardOrder.Add(key);
    }
}
