using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveConfig : MonoBehaviour {

    public int currentLevel = 1;
    public Text levelText;
    public void SubtractLevel()
    {
        if (currentLevel > 2){
            currentLevel--;
            levelText.text = currentLevel.ToString();
        }
    }
    public void AddLevel()
    {
        currentLevel++;
        levelText.text = currentLevel.ToString();
    }


}
