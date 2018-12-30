using System.Collections;
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
    public GameObject UIBanner;
    public GameObject gameCompleteMenuPrefab;
    bool menuEnabled = true;
    GameManager manager;
    private void Start()
    {
        ResizeBanner();
        manager = FindObjectOfType<GameManager>();
        LoadManagerSettings();
        HidePauseMenu();
        SetManagerLabels();
    }
    void ResizeBanner()
    {
        
        Vector3 lowerLeftCorner = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 upperRightCorner = Camera.main.ViewportToWorldPoint(Vector3.one);
        RectTransform tForm = UIBanner.GetComponent<RectTransform>();
        float bannerSize = Screen.height * GameSingleton.Instance.bannerPercentage;
        tForm.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,bannerSize);

    }
    void SetManagerLabels()
    {
        if (pulseStrengthText == null || pulseDurationText == null) return;
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
        if (tapText == null) return;
        tapText.text = "TAPS: " + count;
    }
    public void SetTime(float seconds)
    {
        if (timeText == null) return;
        timeText.text = "TIME: " + seconds.ToString("0.00");
    }
    public void SetScore(int score)
    {
        if (scoreText == null) return;
        scoreText.text = score.ToString();   
    }
    IEnumerator CompleteAfterDelay(float delayTime = 1.0f)
    {
        yield return new WaitForSeconds(delayTime);
        GameObject menu = Instantiate(gameCompleteMenuPrefab,transform);
    }

    public void ShowGameCompleteMenu()
    {
        menuEnabled = false;
        StartCoroutine(CompleteAfterDelay());
    }
    public void ShowPauseMenu()
    {
        if (!menuEnabled) return;
        Time.timeScale = 0.0f;
        PauseMenu.SetActive(true);
    }
    public void HidePauseMenu()
    {
        Time.timeScale = 1.0f;
        PauseMenu.SetActive(false);
    }
    public void ToggleAudio()
    {
        int audioSetting = PlayerPrefs.GetInt("Audio");
        bool audioOn = audioSetting == 1 ? false : true;
        AudioManager.Instance.SetAudio(audioOn);
    }
    public void Shuffle()
    {
        Debug.Log("Shuffle");
    }
    public void Reload()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void Home()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void LevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
    }
    public void NextLevel()
    {
        GameSingleton.Instance.currentLevel = GameSingleton.Instance.currentLevel + 1;
        SceneManager.LoadScene("GameScene");

    }

}
