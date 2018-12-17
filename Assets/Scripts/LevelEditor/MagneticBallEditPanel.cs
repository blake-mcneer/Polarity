using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagneticBallEditPanel : MonoBehaviour {

    public MagneticBall magneticObjectBeingModified;
    public Text objectTypeText;
    //public Button typeButton;

    public void SetMagneticObject(MagneticBall newMagneticObject)
    {
        magneticObjectBeingModified = newMagneticObject;
        objectTypeText.text = newMagneticObject.type.ToString();
    }
    public void ModificationsComplete()
    {
        magneticObjectBeingModified = null;
        gameObject.SetActive(false);
    }
    public void ChangeType()
    {
        if (magneticObjectBeingModified == null) return;
        MagneticType magType;
        switch (magneticObjectBeingModified.type)
        {
            case MagneticType.M1:
                magType = MagneticType.M2;
                break;
            case MagneticType.M2:
                magType = MagneticType.M3;
                break;
            case MagneticType.M3:
                magType = MagneticType.M1;
                break;
            default:
                magType = MagneticType.M1;
                break;
        }
        magneticObjectBeingModified.type = magType;
        magneticObjectBeingModified.SetMaterial();
        objectTypeText.text = magType.ToString();
    }
    public void Delete()
    {
        Destroy(magneticObjectBeingModified.gameObject);
        ModificationsComplete();
    }

}
