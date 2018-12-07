//using System.Collections;
//using System.Collections.Generic;
//using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

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
            EditLevel();
        }else{
            Debug.Log("Attempting a load");
            LoadLevel();
        }
        
    }

    void EditLevel()
    {
        if (mLevel == 0) return;
        string filename = "/Level" + mLevel.ToString() + ".save";
        if (!File.Exists(Application.persistentDataPath + filename))
        {
            //MagneticObject[] magneticObjects = FindObjectsOfType<MagneticObject>();
            //Attractor[] attractors = FindObjectsOfType<Attractor>();
            //Goal[] goals = FindObjectsOfType<Goal>();

            Save save = new Save();

            //foreach (MagneticObject obj in magneticObjects)
            //{
            //    save.magneticObjectElements.Add(new MagneticObjectData(obj.transform.position, obj.type, obj.index));
            //}
            //foreach (Goal g in goals)
            //{
            //    save.goalElements.Add(new GoalData(g.transform.position, g.type, g.barrier, g.rotationSpeed));
            //}
            //foreach (Attractor a in attractors)
            //{
            //    save.attractorElements.Add(new AttractorData(a.transform.position, a.transform.rotation, a.strength));
            //}

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + filename);
            //Save save = (Save)bf.Deserialize(file);
            bf.Serialize(file, save);
            file.Close();            
        }

        GameSingleton.Instance.currentLevel = mLevel;
        SceneManager.LoadScene("LevelBuilder");
    }
    void LoadLevel()
    {
        if (mLevel == 0) return;
        string filename = "/Level" + mLevel.ToString() + ".save";

        if (File.Exists(Application.persistentDataPath + filename))
        {
            //BinaryFormatter bf = new BinaryFormatter();
            //FileStream file = File.Open(Application.persistentDataPath + filename, FileMode.Open);
            //Save save = (Save)bf.Deserialize(file);
            //file.Close();
            GameSingleton.Instance.currentLevel = mLevel;
            SceneManager.LoadScene("SampleScene");
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
