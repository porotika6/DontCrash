using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class KeyboardUI : MonoBehaviour
{
    public KeyboardMahjong keyboard;
    public TextMeshProUGUI[] keyTexts;

    void Update()
    {
        List<KeyCode> visibleKeys = keyboard.GetVisibleKeys();

        for (int i = 0; i < keyTexts.Length; i++)
        {
            keyTexts[i].text = visibleKeys[i].ToString();

            // visual arcade: yang atas lebih terang
            if (i == 0)
                keyTexts[i].color = Color.white;
            else
                keyTexts[i].color = Color.gray;
        }
    }
}
