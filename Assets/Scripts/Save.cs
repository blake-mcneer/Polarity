using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save {
    public List<MagneticObjectData> magneticObjectElements = new List<MagneticObjectData>();
    public List<GoalData> goalElements = new List<GoalData>();
    public List<AttractorData> attractorElements = new List<AttractorData>();
}

