using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MagneticType
{
    M1 = 0, M2, M3
}
public enum GoalBarrier
{
    BarrierNone = 0, Barrier25, Barrier50, Barrier75
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
