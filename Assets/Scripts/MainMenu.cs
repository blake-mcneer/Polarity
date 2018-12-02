using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void PlayGame()
    {
        GameSingleton.Instance.currentMode = LevelButtonMode.LoadLevelMode;
        SceneManager.LoadScene("LevelSelect");
    }
    public void LevelEditor()
    {
        GameSingleton.Instance.currentMode = LevelButtonMode.EditLevelMode;
        SceneManager.LoadScene("LevelSelect");
    }
    public void Home()
    {
        SceneManager.LoadScene("MainMenu");
    }


}
