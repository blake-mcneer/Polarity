﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour {

    public Text timeText;
    public Text tapText;
    public Text scoreText;
    public Text pulseStrengthText;
    public Text pulseDurationText;
    public GameObject PauseMenu;
    GameManager manager;
    private void Start()
    {
        manager = FindObjectOfType<GameManager>();
        LoadManagerSettings();
        HidePauseMenu();
        SetManagerLabels();
    }
    void SetManagerLabels()
    {
        pulseStrengthText.text = manager.pulseStrength.ToString("0.0") + " M";
        pulseDurationText.text = manager.pulseDurationSetting.ToString("0.0") +" S";
    }
    public void AdjustDuration(float adjustment)
    {
        manager.pulseDurationSetting += adjustment;
        SaveGameManagerSettings();
        SetManagerLabels();
    }
    public void AdjustStrength(float adjustment)
    {
        manager.pulseStrength += adjustment;
        SaveGameManagerSettings();
        SetManagerLabels();
    }
    public void LoadManagerSettings()
    {
        if (PlayerPrefs.GetFloat("GameManagerStrength") > 0)
        {
            manager.pulseStrength = PlayerPrefs.GetFloat("GameManagerStrength");
        }
        if (PlayerPrefs.GetFloat("GameManagerDuration") > 0)
        {
            manager.pulseDurationSetting = PlayerPrefs.GetFloat("GameManagerDuration");
        }
    }
    public void SaveGameManagerSettings()
    {
        PlayerPrefs.SetFloat("GameManagerStrength", manager.pulseStrength);
        PlayerPrefs.SetFloat("GameManagerDuration", manager.pulseDurationSetting);
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
    public void Home()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
