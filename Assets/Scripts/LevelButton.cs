//using System.Collections;
//using System.Collections.Generic;
//using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class LevelButton : MonoBehaviour
{

    LevelButtonMode mMode;
    public LevelButtonMode Mode{
        get{
            return mMode;
        }
        set{
            mMode = value;
        }
    }
    int mLevel = 0;

    public int Level{
        get{ 
            return mLevel; 
        }
        set{
            mLevel = value;
            SetLabels();
        }
    }
    public Text text;
    public Button button;
    public void ButtonAction()
    {
        if (mMode == LevelButtonMode.EditLevelMode){
            Debug.Log("Attempting an edit");
            //EditLevel();
        }else{
            Debug.Log("Attempting a load");
            //LoadLevel();
        }
        
    }

    void EditLevel()
    {
        if (mLevel == 0) return;
        string filename = "/Level" + mLevel.ToString() + ".save";
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + filename, FileMode.OpenOrCreate);
        Save save = (Save)bf.Deserialize(file);
        file.Close();            
    }
    void LoadLevel()
    {
        if (mLevel == 0) return;
        string filename = "/Level" + mLevel.ToString() + ".save";

        if (File.Exists(Application.persistentDataPath + filename))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + filename, FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();
        }
        else
        {
            Debug.Log("Could not find save file:" + filename);
        }
    }
    private void Start()
    {
        SetLabels();
    }
    void SetLabels()
    {
        text.text = mLevel.ToString();
    }

}
