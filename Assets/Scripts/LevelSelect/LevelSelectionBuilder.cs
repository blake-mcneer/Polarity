using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionBuilder : MonoBehaviour {
    public GameObject LevelButtonPrefab;
    public LevelButtonMode buttonMode = LevelButtonMode.LoadLevelMode;
    public int levelCount = 30;
    void Start () {
        for (int i = 1; i <= levelCount; i++)
        {
            GameObject levelButtonObject = Instantiate(LevelButtonPrefab,transform);
            levelButtonObject.GetComponent<LevelButton>().Level = i;
            levelButtonObject.GetComponent<LevelButton>().Mode = GameSingleton.Instance.currentMode;
        }
	}
}
