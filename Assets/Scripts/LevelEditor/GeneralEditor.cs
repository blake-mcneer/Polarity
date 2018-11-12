using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralEditor : MonoBehaviour {

    public GameObject BallPrefab;
    public GameObject AttractorPrefab;
    public GameObject GoalPrefab;
    Vector3 spawnPosition = new Vector3(6.0f, 0.0f, 0.0f);

    EditorTool editorTool;
    private void Start()
    {
        editorTool = FindObjectOfType<EditorTool>();
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
    public void TestMode()
    {
        editorTool.ToggleEditMode();
        gameObject.SetActive(false);
    }


}
