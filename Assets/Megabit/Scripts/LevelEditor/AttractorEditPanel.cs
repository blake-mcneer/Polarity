using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttractorEditPanel : MonoBehaviour {

    public Attractor attractorBeingModified;
    public Text strengthText;
    //public Button typeButton;

    public void SetAttractor(Attractor newAttractor)
    {
        attractorBeingModified = newAttractor;
        strengthText.text = "Strength:" + newAttractor.strength.ToString("0.0");
    }
    public void ModificationsComplete()
    {
        attractorBeingModified = null;
        gameObject.SetActive(false);
        //Animate panel
    }
    void ModifyStrength(float amount)
    {
        attractorBeingModified.strength += amount;
        strengthText.text = "Strength:" + attractorBeingModified.strength.ToString("0.0");  
    }

    public void AddStrength(float strengthAmount)
    {
        if (attractorBeingModified == null) return;
        ModifyStrength(strengthAmount);
    }
    public void SubtractStrength(float strengthAmount)
    {
        if (attractorBeingModified == null) return;
        ModifyStrength(-strengthAmount);
    }
    public void Delete()
    {
        Destroy(attractorBeingModified.gameObject);
        ModificationsComplete();
    }

}
