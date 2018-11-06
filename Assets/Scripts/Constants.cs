using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MagneticType
{
    Low = 0, Medium, High
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
