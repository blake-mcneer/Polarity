﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour {

    public Text timeText;
    public Text tapText;
    public Text scoreText;
    public Text finishedGameScoreText;
    public Text pulseStrengthText;
    public Text pulseDurationText;
    public GameObject PauseMenu;
    public GameObject UIBanner;
    public GameObject GameCompleteMenu;
    public RectTransform scoreFillBar;
    public RectTransform tick1;
    public RectTransform tick2;
    public RectTransform tick3;
    bool animatingScore = false;
    float animatingDuration = 0.0f;
    float fullAnimationDuration = 1.5f;
    float scoreAnimationTarget;
    GameManager manager;
    private void Start()
    {
        ResizeBanner();
        manager = FindObjectOfType<GameManager>();
        PlaceScoreTicks();
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
        tapText.text = "TAPS: " + count;
    }
    public void SetTime(float seconds)
    {
        timeText.text = "TIME: " + seconds.ToString("0.00");
    }
    public void SetScore(int score)
    {
        scoreText.text = score.ToString();   
        //if (finishedGameScoreText)
        //{
        //    finishedGameScoreText.text = score.ToString();
        //}
    }
    IEnumerator CompleteAfterDelay(float delayTime = 1.0f)
    {
        yield return new WaitForSeconds(delayTime);
        GameCompleteMenu.SetActive(true);
        StartCoroutine(AnimateAfterDelay());
    }
    IEnumerator AnimateAfterDelay(float delayTime = 0.5f)
    {
        yield return new WaitForSeconds(delayTime);
        UpdateScoreBar();
    }

    public void ShowGameCompleteMenu()
    {
        StartCoroutine(CompleteAfterDelay());
    }
    public void HideGameCompleteMenu()
    {
        GameCompleteMenu.SetActive(false);
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
    private void Update()
    {
        if (animatingScore)
        {
            animatingDuration += Time.deltaTime;
            animatingDuration = Mathf.Min(fullAnimationDuration, animatingDuration);
            float targetScore = (float)manager.score;
            float percentage = animatingDuration / fullAnimationDuration;
            percentage = Mathf.Min(1.0f, percentage);
            float showScore = targetScore * percentage;
            finishedGameScoreText.text = showScore.ToString("0");
            float fillBarCur = scoreAnimationTarget * percentage;
            scoreFillBar.localScale = new Vector3(fillBarCur, 1.0f);

        }
    }
    void UpdateScoreBar()
    {
        if (!manager) return;
        animatingScore = true;
        float score = (float)manager.score;
        LevelScoring scoring = manager.scoring;
        float fullWidth = 474.0f;
        float fillBarTarget = score / scoring.topScore;
        scoreAnimationTarget = fillBarTarget;
        //scoreFillBar.localScale = new Vector3(fillBarTarget, 1.0f);
    }
    void PlaceScoreTicks()
    {
        LevelScoring scoring = manager.scoring;
        float fullWidth = 474.0f;
        float t1Loc = (scoring.tier1 / scoring.topScore) * fullWidth;
        float t2Loc = (scoring.tier2 / scoring.topScore) * fullWidth;
        float t3Loc = (scoring.tier3 / scoring.topScore) * fullWidth;
        Debug.Log("T3Loc:" + t3Loc + " when scoringt3:" + scoring.tier3 + " on fullScore:" + scoring.topScore);
        tick1.localPosition = new Vector3(t1Loc - fullWidth / 2.0f, tick1.localPosition.y);
        tick2.localPosition = new Vector3(t2Loc - fullWidth / 2.0f, tick2.localPosition.y);
        tick3.localPosition = new Vector3(t3Loc - fullWidth / 2.0f, tick3.localPosition.y);
    }
}
