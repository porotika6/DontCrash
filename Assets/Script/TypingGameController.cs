using TMPro;
using UnityEngine;

public class TypingGameController : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text targetText;
    public TMP_InputField inputField;

    [Header("Sentences")]
    [TextArea(2, 4)]
    public string[] sentences;

    private int currentIndex = 0;

    private string baseColor = "#282B28";
    private string correctColor = "#83BCA9";

    void Start()
    {
        inputField.text = "";
        inputField.onValueChanged.AddListener(OnTyping);
        ShowSentence();
    }

    void ShowSentence()
    {
        inputField.text = "";
        inputField.ActivateInputField();

        string sentence = sentences[currentIndex];
        targetText.text = $"<color={baseColor}>{sentence}</color>";
    }

    void OnTyping(string typed)
    {
        string sentence = sentences[currentIndex];
        int correctLength = 0;

        for (int i = 0; i < typed.Length && i < sentence.Length; i++)
        {
            if (typed[i] == sentence[i])
                correctLength++;
            else
                break;
        }

        UpdateTargetText(sentence, correctLength);

        // Kalau sudah selesai satu kalimat
        if (typed == sentence)
        {
            NextSentence();
        }
    }

    void UpdateTargetText(string sentence, int correctLength)
    {
        string correctPart = sentence.Substring(0, correctLength);
        string remainingPart = sentence.Substring(correctLength);

        targetText.text =
            $"<color={correctColor}>{correctPart}</color>" +
            $"<color={baseColor}>{remainingPart}</color>";
    }

    void NextSentence()
    {
        currentIndex++;

        if (currentIndex >= sentences.Length)
        {
            targetText.text = "<color=#83BCA9>DONE!</color>";
            inputField.interactable = false;
        }
        else
        {
            ShowSentence();
        }
    }
}
