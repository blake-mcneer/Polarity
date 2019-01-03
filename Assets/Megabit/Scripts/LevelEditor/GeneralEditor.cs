using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneralEditor : MonoBehaviour {

    public GameObject BallPrefab;
    public GameObject AttractorPrefab;
    public GameObject GoalPrefab;
    public GameObject BorderPiecePrefab;
    public Text levelText;
    Vector3 spawnPosition = new Vector3(6.0f, 0.0f, 0.0f);

    EditorTool editorTool;
    private void Start()
    {
        editorTool = FindObjectOfType<EditorTool>();
        levelText.text = GameSingleton.Instance.currentLevel.ToString();
    }
    public void CreateBall()
    {
        Instantiate(BallPrefab,spawnPosition, Quaternion.Euler(0.0f,0.0f,0.0f));   
    }
    public void CreateAttractor()
    {
        Instantiate(AttractorPrefab,spawnPosition, Quaternion.Euler(0.0f,0.0f,0.0f));   
    }
    public void CreateGoal()
    {
        Instantiate(GoalPrefab,spawnPosition, Quaternion.Euler(0.0f,0.0f,0.0f));   
    }
    public void CreateBorderPiece()
    {
        Instantiate(BorderPiecePrefab, spawnPosition, Quaternion.Euler(0.0f, 0.0f, 0.0f));
    }
    public void TestMode()
    {
        editorTool.ToggleEditMode();
        gameObject.SetActive(false);
    }


}
