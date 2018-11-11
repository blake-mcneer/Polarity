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
