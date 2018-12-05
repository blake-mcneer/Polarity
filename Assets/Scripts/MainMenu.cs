using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class MainMenu : MonoBehaviour {

    public void PlayGame()
    {
        GameSingleton.Instance.currentMode = LevelButtonMode.LoadLevelMode;
        SceneManager.LoadScene("LevelSelect");
    }
    public void LevelEditor()
    {
        GameSingleton.Instance.currentMode = LevelButtonMode.EditLevelMode;
        SceneManager.LoadScene("LevelSelect");
    }
    public void Home()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void ConvertCoordinates()
    {
        for (int i = 1; i <= 30; i++){
            CreateNewSave(i);
        }
    }
    public void CreateNewSave(int level)
    {
        string filename = "/Level" + level.ToString() + ".save";
        BinaryFormatter newBF = new BinaryFormatter();
        FileStream newFile = File.Create(Application.persistentDataPath + filename);
        Save newSave = new Save();
        newBF.Serialize(newFile, newSave);
        newFile.Close();            
    }

    //Delete all of this after kelly does his conversions
    public void UpdateLevel(int level = 0)
    {        
        string filename = "/Level" + level.ToString() + ".save";
        if (File.Exists(Application.persistentDataPath + filename))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + filename, FileMode.Open);

            Save oldSave = (Save)bf.Deserialize(file);
            file.Close();

            Save newSave = new Save();
            foreach (MagneticObjectData obj in oldSave.magneticObjectElements)
            {
                Vector3 oldCoord = new Vector3(obj.posX, obj.posY, obj.posZ);
                Vector3 newCoord = Camera.main.ScreenToWorldPoint(oldCoord);
                Debug.Log("Converting magnetic obj from: " + oldCoord + " to: " + newCoord);
                newSave.magneticObjectElements.Add(new MagneticObjectData(newCoord, obj.type, obj.index));
            }
            foreach (GoalData g in oldSave.goalElements)
            {
                Vector3 oldCoord = new Vector3(g.posX, g.posY, g.posZ);
                Vector3 newCoord = Camera.main.ScreenToWorldPoint(oldCoord);
                newSave.goalElements.Add(new GoalData(newCoord, g.type, g.barrier, g.rotationSpeed));

            }
            foreach (AttractorData a in oldSave.attractorElements)
            {
                Vector3 oldCoord = new Vector3(a.posX, a.posY, a.posZ);
                Vector3 newCoord = Camera.main.ScreenToWorldPoint(oldCoord);
                Quaternion newRot = new Quaternion(a.rotX, a.rotY, a.rotZ, a.rotW);
                newSave.attractorElements.Add(new AttractorData(newCoord, newRot, a.strength));
            }

            BinaryFormatter newBF = new BinaryFormatter();
            FileStream newFile = File.Create(Application.persistentDataPath + filename);
            //Save save = (Save)bf.Deserialize(file);
            newBF.Serialize(newFile, newSave);
            newFile.Close();            

        }
        else
        {
            Debug.Log("Could not find save file:" + filename);
        }
    }

}
