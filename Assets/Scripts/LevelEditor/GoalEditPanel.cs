using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalEditPanel : MonoBehaviour {

    public Goal goalBeingModified;
    public Text goalTypeText;
    public Text barrierTypeText;
    //public Button typeButton;

    public void SetGoal(Goal newGoal)
    {
        goalBeingModified = newGoal;
        goalTypeText.text = newGoal.type.ToString();
        barrierTypeText.text = newGoal.barrier.ToString();
    }
    public void ModificationsComplete()
    {
        goalBeingModified = null;
        gameObject.SetActive(false);
    }
    public void ChangeType()
    {
        if (goalBeingModified == null) return;
        MagneticType magType;
        switch (goalBeingModified.type){
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
        goalBeingModified.type = magType;
        goalBeingModified.ConfigureSettings();
        goalTypeText.text = magType.ToString();
    }
    public void ChangeBarrier()
    {
        if (goalBeingModified == null) return;
        GoalBarrier barrier;
        switch (goalBeingModified.barrier){
            case GoalBarrier.BarrierNone:
                barrier = GoalBarrier.Barrier25;
                break;
            case GoalBarrier.Barrier25:
                barrier = GoalBarrier.Barrier50;
                break;
            case GoalBarrier.Barrier50:
                barrier = GoalBarrier.Barrier75;
                break;
            case GoalBarrier.Barrier75:
                barrier = GoalBarrier.BarrierNone;
                break;
            default:
                barrier = GoalBarrier.BarrierNone;
                break;
        }
        goalBeingModified.barrier = barrier;
        goalBeingModified.ConfigureSettings();
        barrierTypeText.text = barrier.ToString();
    }



}
