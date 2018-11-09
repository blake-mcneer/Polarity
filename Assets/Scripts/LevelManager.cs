using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public void SaveLevel()
    {
        MagneticObject[] magneticObjects = FindObjectsOfType<MagneticObject>();
        Attractor[] attractors = FindObjectsOfType<Attractor>();
        Goal[] goals = FindObjectsOfType<Goal>();
    }
}
