using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TypingGameManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text targetText;
    public TMP_Text typingBufferText;

    [Header("Sentences")]
    [TextArea(2, 5)]
    public List<string> sentences;

    private int sentenceIndex = 0;
    private int cursorIndex = 0;

    private List<TypedChar> typedBuffer = new List<TypedChar>();

    void Start()
    {
        LoadSentence(0);
    }

    void LoadSentence(int index)
    {
        sentenceIndex = index;
        cursorIndex = 0;
        typedBuffer.Clear();

        targetText.text = sentences[sentenceIndex];
        typingBufferText.text = "";
    }

    public void ReceiveResolvedChar(char c)
    {
        if (cursorIndex >= sentences[sentenceIndex].Length)
            return;

        char targetChar = sentences[sentenceIndex][cursorIndex];

        bool correct = c == targetChar;

        typedBuffer.Add(new TypedChar(c, correct));

        if (correct)
            cursorIndex++;

        RefreshTypingBuffer();

        if (cursorIndex == sentences[sentenceIndex].Length)
        {
            NextSentence();
        }
    }

    public void HandleBackspace()
    {
        if (typedBuffer.Count == 0)
            return;

        TypedChar last = typedBuffer[^1];
        typedBuffer.RemoveAt(typedBuffer.Count - 1);

        if (last.correct)
            cursorIndex--;

        RefreshTypingBuffer();
    }

    void RefreshTypingBuffer()
    {
        typingBufferText.text = "";

        foreach (var t in typedBuffer)
        {
            if (t.correct)
                typingBufferText.text += $"<color=white>{t.character}</color>";
            else
                typingBufferText.text += $"<color=red>{t.character}</color>";
        }
    }

    void NextSentence()
    {
        if (sentenceIndex + 1 < sentences.Count)
            LoadSentence(sentenceIndex + 1);
        else
            Debug.Log("GAME SELESAI");
    }
}

public struct TypedChar
{
    public char character;
    public bool correct;

    public TypedChar(char c, bool correct)
    {
        character = c;
        this.correct = correct;
    }
}
