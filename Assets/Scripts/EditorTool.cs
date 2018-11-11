using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorTool : MonoBehaviour {

    public enum PlayMode
    {
        Play, Edit
    }
    public GameObject GameManager;
    public GameObject LevelEditor;

    PlayMode currentMode = PlayMode.Edit;

    private void Start()
    {
        SetMode(currentMode);
    }
    public void ToggleEditMode()
    {
        if (currentMode == PlayMode.Play){
            SetMode(PlayMode.Edit);
        }else if (currentMode == PlayMode.Edit){
            SetMode(PlayMode.Play);
        }

    }

    public void SetMode(PlayMode mode)
    {
        currentMode = mode;
        if (currentMode == PlayMode.Play)
        {
            GameManager.SetActive(true);
            LevelEditor.SetActive(false);
            GameManager.GetComponent<GameManager>().SetupManager();
        }
        else
        {
            GameManager.SetActive(false);
            LevelEditor.SetActive(true);
        }
    }
}
