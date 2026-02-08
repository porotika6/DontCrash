using TMPro;
using UnityEngine;

public class TypingGameController : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text targetText;
    public TMP_InputField inputField;
    public GameObject startOverlay; // Panel "Tap to Start" dimasukkan ke sini

    [Header("Keyboard System")]
    public KeyboardSystemNormal keyboard; // Referensi ke script keyboardmu

    [Header("Sentences")]
    [TextArea(2, 4)]
    public string[] sentences;

    int sentenceIndex = 0;
    int charIndex = 0;
    int lastInputLength = 0;
    bool isGameActive = false; // Penanda apakah game sudah mulai

    const string BASE_COLOR = "#282B28";
    const string CORRECT_COLOR = "#83BCA9";

    void Awake()
    {
        inputField.onValueChanged.AddListener(OnInputChanged);
    }

    // 1. Dipanggil otomatis saat HP dibuka (via Ikon)
    void OnEnable()
    {
        // Reset status game tapi JANGAN mulai dulu
        isGameActive = false;
        
        // Bersihkan teks
        targetText.text = ""; 
        inputField.text = "";
        
        // Matikan input field agar keyboard tidak muncul dulu
        inputField.DeactivateInputField(); 

        // Munculkan tombol "Tap To Start"
        if (startOverlay != null)
            startOverlay.SetActive(true);
    }

    // 2. FUNGSI BARU: Dipanggil saat tombol "Tap to Start" diklik
    public void BeginTypingGame()
    {
        isGameActive = true;

        // Sembunyikan layar start
        if (startOverlay != null)
            startOverlay.SetActive(false);

        // Reset data
        sentenceIndex = 0;
        charIndex = 0;
        lastInputLength = 0;

        // Mulai Game & Fokus Keyboard
        inputField.text = "";
        inputField.ActivateInputField(); 
        inputField.Select();
        
        LoadSentence();
    }

    void OnInputChanged(string value)
    {
        if (!isGameActive) return; // Cegah ngetik kalau belum klik start

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
        if (!isGameActive) return; // Cegah update kalau belum start

        if (!keyboard.TryGet(out char c))
            return;

        HandleCharacter(c);
    }

    void HandleCharacter(char c)
    {
        if (sentenceIndex >= sentences.Length) return;

        string sentence = sentences[sentenceIndex];
        if (charIndex >= sentence.Length) return;

        if (c == sentence[charIndex])
        {
            charIndex++;
            UpdateTargetText(sentence);

            if (charIndex == sentence.Length)
            {
                sentenceIndex++;
                if (sentenceIndex < sentences.Length)
                {
                    LoadSentence();
                }
                else
                {
                    targetText.text = $"<color={CORRECT_COLOR}>DONE</color>";
                    inputField.text = "";
                    inputField.DeactivateInputField();
                    isGameActive = false;
                    
                    // Opsional: Tutup HP otomatis
                    // GetComponentInParent<PhoneUIManager>().ClosePhone();
                }
            }
        }
    }

    void LoadSentence()
    {
        charIndex = 0;
        lastInputLength = 0;
        inputField.text = "";
        targetText.text = $"<color={BASE_COLOR}>{sentences[sentenceIndex]}</color>";
    }

    void UpdateTargetText(string sentence)
    {
        int lastCompletedWordEnd = 0;
        for (int i = 0; i < charIndex; i++)
        {
            if (sentence[i] == ' ') lastCompletedWordEnd = i + 1;
        }

        if (charIndex == sentence.Length) lastCompletedWordEnd = charIndex;

        if (lastCompletedWordEnd == 0)
        {
            targetText.text = $"<color={BASE_COLOR}>{sentence}</color>";
            return;
        }

        string greenPart = sentence.Substring(0, lastCompletedWordEnd);
        string restPart = sentence.Substring(lastCompletedWordEnd);

        targetText.text = $"<color={CORRECT_COLOR}>{greenPart}</color>" +
                          $"<color={BASE_COLOR}>{restPart}</color>";
    }
}