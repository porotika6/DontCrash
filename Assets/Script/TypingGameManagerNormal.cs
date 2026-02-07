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

    int sentenceIndex = 0;
    int cursorIndex = 0;

    List<InputChar> buffer = new();

    int currentWordIndex = 0;
    HashSet<int> completedWords = new HashSet<int>();

    void Start()
    {
        LoadSentence(0);
    }

    void LoadSentence(int index)
    {
        sentenceIndex = index;
        cursorIndex = 0;

        buffer.Clear();
        completedWords.Clear();
        currentWordIndex = 0;

        typingBufferText.text = "";
        RefreshTargetText();
    }

    // ===== INPUT =====
    public void ReceiveChar(char input)
    {
        if (cursorIndex >= sentences[sentenceIndex].Length)
            return;

        char target = sentences[sentenceIndex][cursorIndex];

        bool correct =
            char.ToLower(input) == char.ToLower(target);

        char displayChar = correct ? target : input;
        buffer.Add(new InputChar(displayChar, correct));

        if (correct)
            cursorIndex++;

        bool wordFinished =
            correct &&
            (target == ' ' || cursorIndex == sentences[sentenceIndex].Length);

        if (wordFinished)
        {
            completedWords.Add(currentWordIndex);
            currentWordIndex++;
        }

        RefreshTypingBuffer();
        RefreshTargetText();
    }

    // ===== BACKSPACE =====
    public void HandleBackspace()
    {
        if (buffer.Count == 0)
            return;

        InputChar last = buffer[^1];
        buffer.RemoveAt(buffer.Count - 1);

        if (last.correct)
        {
            cursorIndex--;
            currentWordIndex = GetWordIndex(cursorIndex);
            completedWords.RemoveWhere(w => w >= currentWordIndex);
        }

        RefreshTypingBuffer();
        RefreshTargetText();
    }

    // ===== INPUT TEXT =====
    void RefreshTypingBuffer()
    {
        typingBufferText.text = "";

        foreach (var c in buffer)
        {
            if (c.correct)
                typingBufferText.text += $"<color=white>{c.ch}</color>";
            else
                typingBufferText.text += $"<color=red>{c.ch}</color>";
        }
    }

    // ===== TARGET TEXT =====
    void RefreshTargetText()
    {
        string sentence = sentences[sentenceIndex];
        string result = "";

        int wordIndex = 0;

        for (int i = 0; i < sentence.Length; i++)
        {
            char c = sentence[i];

            if (completedWords.Contains(wordIndex))
                result += $"<color=#83BCA9>{c}</color>";
            else
                result += $"<color=#282B28>{c}</color>";

            if (c == ' ')
                wordIndex++;
        }

        targetText.text = result;
    }

    int GetWordIndex(int charIndex)
    {
        string s = sentences[sentenceIndex];
        int word = 0;

        for (int i = 0; i < charIndex && i < s.Length; i++)
        {
            if (s[i] == ' ')
                word++;
        }

        return word;
    }
}

public struct InputChar
{
    public char ch;
    public bool correct;

    public InputChar(char c, bool ok)
    {
        ch = c;
        correct = ok;
    }
}
