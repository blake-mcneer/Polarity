using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorTool : MonoBehaviour {

    UI uiPanel;
    public enum PlayMode
    {
        Play, Edit
    }
    public GameObject GameManager;
    public GameObject LevelEditor;
    public GameObject GeneralEditorToolbar;

    PlayMode currentMode = PlayMode.Edit;

    private void Start()
    {
        uiPanel = FindObjectOfType<UI>();
        SetMode(currentMode);
    }
    public void ToggleEditMode()
    {
        if (currentMode == PlayMode.Play){
            SetMode(PlayMode.Edit);
        }else if (currentMode == PlayMode.Edit){
            SetMode(PlayMode.Play);
        }

        uiPanel.HidePauseMenu();
    }

    public void SetMode(PlayMode mode)
    {
        currentMode = mode;
        if (currentMode == PlayMode.Play)
        {
            GameManager.SetActive(true);
            GameManager.GetComponent<GameManager>().SetupManager();
            LevelEditor.SetActive(false);
            GeneralEditorToolbar.SetActive(false);
            GameManager.GetComponent<GameManager>().SetupManager();
        }
        else
        {
            GameManager.SetActive(false);
            LevelEditor.SetActive(true);
            GeneralEditorToolbar.SetActive(true);
        }
    }
}
