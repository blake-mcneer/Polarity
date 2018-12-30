using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save {
    public List<MagneticObjectData> magneticObjectElements = new List<MagneticObjectData>();
    public List<GoalData> goalElements = new List<GoalData>();
    public List<AttractorData> attractorElements = new List<AttractorData>();
    public List<BorderPieceData> borderPieceElements = new List<BorderPieceData>();
    public float tier1Score = 50.0f;
    public float tier2Score = 100.0f;
    public float tier3Score = 500.0f;
    public float topScore = 1000.0f;
}

