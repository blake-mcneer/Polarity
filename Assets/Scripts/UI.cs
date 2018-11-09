using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour {

    public Text timeText;
    public Text tapText;
    public Text scoreText;
    public GameObject PauseMenu;
    GameManager manager;
    private void Start()
    {
        manager = FindObjectOfType<GameManager>();
        HidePauseMenu();
    }
    public void SetTapCount(int count)
    {
        tapText.text = "TAPS: " + count;
    }
    public void SetTime(float seconds)
    {
        timeText.text = "TIME: " + seconds.ToString("0.00");
    }
    public void SetScore(int score)
    {
        scoreText.text = score.ToString();   
    }
    public void ShowPauseMenu()
    {
        Time.timeScale = 0.0f;
        PauseMenu.SetActive(true);
    }
    public void HidePauseMenu()
    {
        Time.timeScale = 1.0f;
        PauseMenu.SetActive(false);
    }
    public void Shuffle()
    {
        Debug.Log("Shuffle");
    }
    public void Reload()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
