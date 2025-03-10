using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        LoadGame.Instance.wasLoaded = false;
        PlayerPrefs.DeleteAll(); // apagar todas as referencias salvas 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitGame()
    {   
        Application.Quit();
    }
}
