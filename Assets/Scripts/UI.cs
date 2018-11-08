﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    public Text timeText;
    public Text tapText;
    public Text scoreText;

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
}
