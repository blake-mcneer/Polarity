using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum MagneticType
{
    M1 = 0, M2, M3
}

[System.Serializable]
public enum GoalBarrier
{
    BarrierNone = 0, Barrier25, Barrier50, Barrier75
}


[System.Serializable]
public struct GoalData
{
    public float rotationSpeed;
    public float posX;
    public float posY;
    public float posZ;
    public MagneticType type;
    public GoalBarrier barrier;
    public GoalData(Vector3 pos, MagneticType t, GoalBarrier b, float rotSpeed)
    {
        rotationSpeed = rotSpeed;
        posX = pos.x;
        posY = pos.y;
        posZ = pos.z;
        type = t;
        barrier = b;
    }
}
[System.Serializable]
public struct BorderPieceData
{
    public BorderPieceType type;
    public RotationDegrees rot;
    public float posX;
    public float posY;
    public float posZ;
    public BorderPieceData(Vector3 pos, BorderPieceType t, RotationDegrees r)
    {
        rot = r;
        type = t;
        posX = pos.x;
        posY = pos.y;
        posZ = pos.z;
    }
}
[System.Serializable]
public struct AttractorData
{
    public float rotX;
    public float rotY;
    public float rotZ;
    public float rotW;
    public float posX;
    public float posY;
    public float posZ;
    public float strength;
    public AttractorData(Vector3 pos, Quaternion rot, float attractorStrength)
    {
        rotX = rot.x;
        rotY = rot.y;
        rotZ = rot.z;
        rotW = rot.w;

        posX = pos.x;
        posY = pos.y;
        posZ = pos.z;

        strength = attractorStrength;
    }
}
[System.Serializable]
public enum BorderPieceType
{
    BorderPieceStraight,
    BorderPieceCorner    
}
[System.Serializable]
public enum RotationDegrees
{
    None,
    S1_FortyFive,
    S2_Ninety,
    S3_OneThirtyFive,
    S4_OneEighty,
    S5_TwoTwentyFive,
    S6_TwoSeventy,
    S7_ThreeFifteen
}

[System.Serializable]
public struct MagneticObjectData
{
    public int index;
    public MagneticType type;
    public float posX;
    public float posY;
    public float posZ;


    public MagneticObjectData(Vector3 pos, MagneticType t, int ind)
    {
        index = ind;
        type = t;
        posX = pos.x;
        posY = pos.y;
        posZ = pos.z;
    }
}
[System.Serializable]
public struct GameManagerData
{
    float pulseDuration;
    float pulseStrength;
}

public struct LevelScoring
{
    public float tier1;
    public float tier2;
    public float tier3;
    public float topScore;
    public LevelScoring(float t1, float t2, float t3, float top)
    {
        if (t1 == 0.0f) t1 = 50.0f;
        if (t2 == 0.0f) t2 = 100.0f;
        if (t3 == 0.0f) t3 = 500.0f;
        if (top == 0.0f) top = 1000.0f;
        tier1 = t1;
        tier2 = t2;
        tier3 = t3;
        topScore = top;
    }
}

public struct CustomRange
{
    int min;
    int max;
    public CustomRange(int minimum, int maximum)
    {
        min = minimum;
        max = maximum;
    }
}
public enum LevelButtonMode
{
    LoadLevelMode, EditLevelMode
}

public struct DropSequenceItem
{
    public float xPosition;
    public int index;
    public float sequenceTimimg;
    public MagneticType magneticType;

    public DropSequenceItem(float pos, int ind, float timing, MagneticType type)
    {
        xPosition = pos;
        index = ind;
        sequenceTimimg = timing;
        magneticType = type;
    }
}
public class Constants{
}
