using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralEditor : MonoBehaviour {

    public GameObject BallPrefab;
    public GameObject AttractorPrefab;
    public GameObject GoalPrefab;

    EditorTool editorTool;
    private void Start()
    {
        editorTool = FindObjectOfType<EditorTool>();
    }
    public void CreateBall()
    {
        Instantiate(BallPrefab,Vector3.zero, Quaternion.Euler(0.0f,0.0f,0.0f));   
    }
    public void CreateAttractor()
    {
        Instantiate(AttractorPrefab,Vector3.zero, Quaternion.Euler(0.0f,0.0f,0.0f));   
    }
    public void CreateGoal()
    {
     Instantiate(GoalPrefab,Vector3.zero, Quaternion.Euler(0.0f,0.0f,0.0f));   
    }
    public void TestMode()
    {
        editorTool.ToggleEditMode();
        gameObject.SetActive(false);
    }


}
