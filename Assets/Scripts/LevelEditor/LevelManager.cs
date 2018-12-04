﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    public GameObject BallPrefab;
    public GameObject AttractorPrefab;
    public GameObject GoalPrefab;
    public GameObject SaveMenu;
    public SaveConfig config;
    GameManager manager;
    Save saveFile;

    private void Start()
    {
        manager = FindObjectOfType<GameManager>();
        LoadLevel(GameSingleton.Instance.currentLevel);
    }
    public void ShowSave()
    {
        SaveMenu.SetActive(true);
    }


    public void SaveLevel()
    {
        int levelNumber = config.currentLevel;
        levelNumber = GameSingleton.Instance.currentLevel;
        MagneticObject[] magneticObjects = FindObjectsOfType<MagneticObject>();
        Attractor[] attractors = FindObjectsOfType<Attractor>();
        Goal[] goals = FindObjectsOfType<Goal>();

        Save save = new Save();

        foreach (MagneticObject obj in magneticObjects)
        {
            Vector3 pos = obj.transform.position;
            pos = Camera.main.WorldToViewportPoint(pos);
            save.magneticObjectElements.Add(new MagneticObjectData(pos, obj.type, obj.index));
        }
        foreach(Goal g in goals)
        {
            Vector3 pos = g.transform.position;
            pos = Camera.main.WorldToViewportPoint(pos);
            save.goalElements.Add(new GoalData(pos, g.type,g.barrier, g.rotationSpeed));
        }
        foreach (Attractor a in attractors)
        {
            Vector3 pos = a.transform.position;
            pos = Camera.main.WorldToViewportPoint(pos);
            save.attractorElements.Add(new AttractorData(pos, a.transform.rotation, a.strength));
        }
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/Level" + levelNumber.ToString() + ".save");
        bf.Serialize(file, save);
        file.Close();
        SaveMenu.SetActive(false);
    }
    public void DeleteAll()
    {
        MagneticObject[] magneticObjects = FindObjectsOfType<MagneticObject>();
        Attractor[] attractors = FindObjectsOfType<Attractor>();
        Goal[] goals = FindObjectsOfType<Goal>();

        for (int i = 0; i < magneticObjects.Length; i++){
            Destroy(magneticObjects[i].gameObject);
        }
        for (int i = 0; i < goals.Length; i++)
        {
            Destroy(goals[i].gameObject);
        }
        for (int i = 0; i < attractors.Length; i++)
        {
            Destroy(attractors[i].gameObject);
        }
    }
    public void LoadLevel(int level = 0)
    {
        int levelNumber = level == 0 ? config.currentLevel : level;
        string filename = "/Level" + levelNumber.ToString() + ".save";
        if (File.Exists(Application.persistentDataPath+filename))
        {
            DeleteAll();
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + filename, FileMode.Open);

            Save save = (Save)bf.Deserialize(file);
            file.Close();

            foreach(MagneticObjectData obj in save.magneticObjectElements)
            {
                LoadMagneticBall(obj);
            }
            foreach(GoalData g in save.goalElements)
            {
                LoadGoal(g);
            }
            foreach (AttractorData a in save.attractorElements)
            {
                LoadAttractor(a);
            }
            manager.SetupManager();
            PlayerPrefs.SetInt("CurrentLevel",levelNumber);
        }
        else
        {
            Debug.Log("Could not find save file:" + filename);
        }
        SaveMenu.SetActive(false);
    }
    void LoadMagneticBall(MagneticObjectData d)
    {
        Vector3 pos = new Vector3(d.posX, d.posY, d.posZ);
        pos = Camera.main.ViewportToWorldPoint(pos);
        Quaternion rot = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
        GameObject obj = Instantiate(BallPrefab, pos, rot, transform.parent);
        MagneticObject magObj = obj.GetComponent<MagneticObject>();
        magObj.index = d.index;
        magObj.type = d.type;
        magObj.SetMaterial();
    }
    void LoadGoal(GoalData d)
    {
        Vector3 pos = new Vector3(d.posX, d.posY, d.posZ);
        pos = Camera.main.ViewportToWorldPoint(pos);
        Quaternion rot = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
        GameObject obj = Instantiate(GoalPrefab, pos, rot, transform.parent);
        Goal goalObj = obj.GetComponent<Goal>();
        goalObj.rotationSpeed = d.rotationSpeed;
        goalObj.type = d.type;
        goalObj.barrier = d.barrier;
        goalObj.ConfigureSettings();
    }
    void LoadAttractor(AttractorData d)
    {
        Vector3 pos = new Vector3(d.posX, d.posY, d.posZ);
        pos = Camera.main.ViewportToWorldPoint(pos);
        Quaternion rot = new Quaternion(d.rotX, d.rotY, d.rotZ, d.rotW);
        GameObject obj = Instantiate(AttractorPrefab, pos, rot, transform.parent);
        Attractor attractorObj = obj.GetComponent<Attractor>();
        attractorObj.strength = d.strength;
        attractorObj.ConfigureAttractor();

    }
}
