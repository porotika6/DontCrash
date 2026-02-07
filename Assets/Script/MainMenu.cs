using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    
     public GameObject TutorialScreen;  
     public void StartGame()
    {
    SceneManager.LoadSceneAsync(1);
    }
    public void Tutorial()
    {
        if(TutorialScreen != null)
        {
           TutorialScreen.SetActive(true);
        }

    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void CloseBTN()
    {
        if(TutorialScreen != null)
        {
            TutorialScreen.SetActive(false);
        }
    }
}
