using UnityEngine;

public class InputManager : MonoBehaviour
{
    public KeyboardMahjong keyboard;   // drag KeyboardSystem ke sini

    void Update()
    {
        // kalau tidak ada input, stop
        if (!Input.anyKeyDown) return;

        // cek SATU PER SATU key yang ada di sistem keyboard
        foreach (KeyCode key in keyboard.keyboardOrder)
        {
            if (Input.GetKeyDown(key))
            {
                HandleKeyPress(key);
                break; // PENTING: stop setelah ketemu
            }
        }
    }

    void HandleKeyPress(KeyCode pressedKey)
    {
        KeyCode expectedKey = keyboard.GetExpectedKey();

        if (pressedKey == expectedKey)
        {
            Debug.Log("BENAR: " + pressedKey);
            // nanti di sini: HP jadi hitam
        }
        else
        {
            Debug.Log("SALAH: " + pressedKey);
        }

        // SELALU geser key yang ditekan (mahjong behavior)
        keyboard.ShiftKey(pressedKey);
    }
}
