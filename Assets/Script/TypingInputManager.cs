using UnityEngine;

public class TypingInputManager : MonoBehaviour
{
    public KeyboardMahjongSystem mahjong;
    public TypingGameManager gameManager;

    void Update()
    {
        if (mahjong == null || gameManager == null)
            return;

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

        char rawChar = input[0];

        if (rawChar == ' ')
        {
            gameManager.ReceiveResolvedChar(' ');
            return;
        }

        if (!char.IsLetter(rawChar))
            return;

        char resolved = mahjong.Resolve(char.ToLower(rawChar));
        if (resolved != '\0')
            gameManager.ReceiveResolvedChar(resolved);
    }
}
