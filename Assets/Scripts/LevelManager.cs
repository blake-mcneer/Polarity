using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

public class LevelManager : MonoBehaviour {

    public GameObject BallPrefab;
    public GameObject AttractorPrefab;
    public GameObject GoalPrefab;
    public GameObject GameManagerPrefab;
    Save saveFile;

    public void SaveLevel(int levelNumber)
    {
        MagneticObject[] magneticObjects = FindObjectsOfType<MagneticObject>();
        Attractor[] attractors = FindObjectsOfType<Attractor>();
        Goal[] goals = FindObjectsOfType<Goal>();

        Save save = new Save();

        foreach (MagneticObject obj in magneticObjects)
        {
            save.magneticObjectElements.Add(new MagneticObjectData(obj.transform.position, obj.type, obj.index));
        }
        foreach(Goal g in goals)
        {
            save.goalElements.Add(new GoalData(g.transform.position, g.type,g.barrier, g.rotationSpeed));
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/Level" + levelNumber.ToString() + ".save");
        bf.Serialize(file, save);
        file.Close();
    }
    public void LoadLevel(int level)
    {
        string filename = "/Level" + level.ToString() + ".save";
        if (File.Exists(Application.persistentDataPath+filename))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + filename, FileMode.Open);

            Save save = (Save)bf.Deserialize(file);
            file.Close();

            foreach(MagneticObjectData obj in save.magneticObjectElements)
            {
                Debug.Log(obj.index);
                Debug.Log(obj.posX);
                Debug.Log(obj.posY);
                Debug.Log(obj.posZ);
                Debug.Log(obj.type);             
            }
            foreach(GoalData g in save.goalElements)
            {
                Debug.Log(g.posX);
                Debug.Log(g.posY);
                Debug.Log(g.posZ);
                Debug.Log(g.type);
                Debug.Log(g.barrier);
            }

        }
        else
        {
            Debug.Log("Could not find save file:" + filename);
        }
    }
}
