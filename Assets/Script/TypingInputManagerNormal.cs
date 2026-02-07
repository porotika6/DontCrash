using System;
using UnityEngine;

public class TypingInputManager : MonoBehaviour
{
    public TypingGameManagerNormal gameManager;

    void Update()
    {
        if (gameManager == null)
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

        char c = input[0];

        // TERIMA HURUF & SPASI
        if (char.IsLetter(c) || c == ' ')
        {
            gameManager.ReceiveChar(c);
        }
    }
}

public class TypingGameManagerNormal
{
    internal void HandleBackspace()
    {
        throw new NotImplementedException();
    }

    internal void ReceiveChar(char c)
    {
        throw new NotImplementedException();
    }
}