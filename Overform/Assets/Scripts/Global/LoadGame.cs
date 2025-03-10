using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class LoadGame : MonoBehaviour
{
    public static LoadGame Instance;
    public bool wasLoaded;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Loading() // Methode pour sauve le jeux
    {
        if (PlayerPrefs.HasKey("LevelSaved"))
        {
            wasLoaded = true;
            PlayerPrefs.SetInt("wasLoaded", 1);

            string levelToLoad = PlayerPrefs.GetString("LevelSaved");
            SceneManager.LoadScene(levelToLoad);
        }
    }
}
