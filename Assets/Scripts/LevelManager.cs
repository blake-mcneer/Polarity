using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public void SaveLevel()
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


    }
    public void LoadLevel(int level)
    {

    }
}
