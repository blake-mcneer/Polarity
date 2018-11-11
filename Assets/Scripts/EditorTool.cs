using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorTool : MonoBehaviour {

    public GameObject GameManager;
    public GameObject LevelEditor;
    public enum PlayMode
    {
        Play,Edit
    }
    private void Start()
    {
        SetMode(PlayMode.Edit);
    }
    public void SetMode(PlayMode mode)
    {
        if (mode == PlayMode.Play)
        {
            GameManager.SetActive(true);
            LevelEditor.SetActive(false);
        }
        else
        {
            GameManager.SetActive(false);
            LevelEditor.SetActive(true);
        }
    }
}
