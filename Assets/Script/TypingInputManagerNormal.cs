using UnityEngine;

public class TypingInputManagerNormal : MonoBehaviour
{
    public KeyboardNormalSystem keyboard;
    public TypingGameManagerNormal gameManager;

    void Update()
    {
        if (keyboard == null || gameManager == null)
            return;

        // BACKSPACE
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            gameManager.HandleBackspace();
            return;
        }

        if (!Input.anyKeyDown)
            return;

        string input = Input.inputString;
        if (string.IsNullOrEmpty(input))
            return;

        char raw = input[0];

        // TERIMA HURUF & SPASI
        if (!char.IsLetter(raw) && raw != ' ')
            return;

        char resolved = keyboard.Resolve(raw);
        gameManager.ReceiveChar(resolved);
    }
}
