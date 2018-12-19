using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameCompleteMenu : MonoBehaviour {

    public Text timeText;
    public Text tapText;
    public Text scoreText;
    public RectTransform scoreFillBar;
    public RectTransform tick1;
    public RectTransform tick2;
    public RectTransform tick3;
    bool animatingScore = false;
    float animatingDuration = 0.0f;
    float fullAnimationDuration = 1.5f;
    float scoreAnimationTarget;
    GameManager manager;

    // Use this for initialization
    void Start () {
        manager = FindObjectOfType<GameManager>();
        PlaceScoreTicks();
        StartCoroutine(AnimateAfterDelay());
        SetStatistics();
	}
    void SetStatistics()
    {
        tapText.text = "TAPS: " + manager.tapCount;
        timeText.text = "TIME: " + manager.seconds.ToString("0.00");
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
            scoreText.text = showScore.ToString("0");
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
    IEnumerator AnimateAfterDelay(float delayTime = 0.5f)
    {
        Debug.Log("Waiting");
        yield return new WaitForSeconds(delayTime);
        UpdateScoreBar();
        Debug.Log("Animating");
    }
}
