using UnityEngine;

public class PhoneUIManager : MonoBehaviour
{
    [Header("Referensi UI")]
    public GameObject typingGamePanel; // Tarik 'TypingGame_Panel' ke sini

    void Start()
    {
        // Pastikan saat game mulai, panel ketikan tertutup
      
    }

    // Fungsi ini dipasang di OnClick() tombol Ikon HP
    public void OpenPhone()
    {
        if (typingGamePanel != null)
        {
            typingGamePanel.SetActive(true);
            
            // Opsional: Jika game kamu ada sistem gerak player, 
            // di sini kamu bisa matikan script gerak player agar tidak gerak saat ngetik
        }
    }

    // Fungsi ini dipasang di OnClick() tombol Close (X) di dalam HP
    public void ClosePhone()
    {
        if (typingGamePanel != null)
        {
            typingGamePanel.SetActive(false);
        }
    }
}