using TMPro;
using UnityEngine;

public class TypingGameController : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text targetText;
    public TMP_InputField inputField;

    [Header("Keyboard")]
    public KeyboardSystemNormal keyboard;

    [Header("Sentences (URUT)")]
    [TextArea(2, 4)]
    public string[] sentences;

    int sentenceIndex = 0;
    int charIndex = 0;
    int lastInputLength = 0;

    // WARNA PERSIS DESAIN AWAL KAMU
    const string BASE_COLOR = "#282B28";
    const string CORRECT_COLOR = "#83BCA9";

    void Start()
    {
        sentenceIndex = 0;
        charIndex = 0;
        lastInputLength = 0;

        inputField.text = "";
        inputField.onValueChanged.AddListener(OnInputChanged);
        inputField.ActivateInputField();

        LoadSentence();
    }

    void OnInputChanged(string value)
    {
        // abaikan backspace
        if (value.Length <= lastInputLength)
        {
            lastInputLength = value.Length;
            return;
        }

        char c = value[value.Length - 1];
        keyboard.Push(c);
        lastInputLength = value.Length;
    }

    void Update()
    {
        if (!keyboard.TryGet(out char c))
            return;

        HandleCharacter(c);
    }

    void HandleCharacter(char c)
    {
        string sentence = sentences[sentenceIndex];

        if (charIndex >= sentence.Length)
            return;

        if (c == sentence[charIndex])
        {
            charIndex++;
            UpdateTargetText(sentence);

            if (charIndex == sentence.Length)
            {
                sentenceIndex++;

                if (sentenceIndex < sentences.Length)
                    LoadSentence();
                else
                {
                    targetText.text = $"<color={CORRECT_COLOR}>DONE</color>";
                    enabled = false;
                }
            }
        }
    }

    void LoadSentence()
    {
        charIndex = 0;
        lastInputLength = 0;
        inputField.text = "";

        // ⬇️ AWAL KALIMAT = WARNA DASAR MURNI (NO TAG)
        targetText.text = $"<color={BASE_COLOR}>{sentences[sentenceIndex]}</color>";
    }

    void UpdateTargetText(string sentence)
    {
        int lastCompletedWordEnd = 0;

        for (int i = 0; i < charIndex; i++)
        {
            if (sentence[i] == ' ')
                lastCompletedWordEnd = i + 1;
        }

        if (charIndex == sentence.Length)
            lastCompletedWordEnd = charIndex;

        // ⬇️ BELUM ADA KATA SELESAI → JANGAN HIJAUIN APA-APA
        if (lastCompletedWordEnd == 0)
        {
            targetText.text = $"<color={BASE_COLOR}>{sentence}</color>";
            return;
        }

        string greenPart = sentence.Substring(0, lastCompletedWordEnd);
        string restPart = sentence.Substring(lastCompletedWordEnd);

        targetText.text =
            $"<color={CORRECT_COLOR}>{greenPart}</color>" +
            $"<color={BASE_COLOR}>{restPart}</color>";
    }
}
