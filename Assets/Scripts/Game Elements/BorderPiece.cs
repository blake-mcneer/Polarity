using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderPiece : MonoBehaviour {
    public BorderPieceType type = BorderPieceType.BorderPieceStraight;
    public RotationDegrees rot = RotationDegrees.None;
    public GameObject cornerPrefab;
    public GameObject straightPrefab;

    private void Start()
    {
        ForceUpdate();
    }
    void ActivateSelectedPrefab()
    {
        switch (type){
            case BorderPieceType.BorderPieceStraight:
                cornerPrefab.SetActive(false);
                straightPrefab.SetActive(true);
                break;
            case BorderPieceType.BorderPieceCorner:
                straightPrefab.SetActive(false);
                cornerPrefab.SetActive(true);
                break;
        }
    }
    void SetRotation()
    {
        float amt = 0.0f;
        switch (rot)
        {
            case RotationDegrees.None:
                amt = 45.0f;
                break;
            case RotationDegrees.S1_FortyFive:
                amt = 90.0f;
                break;
            case RotationDegrees.S2_Ninety:
                amt = 135.0f;
                break;
            case RotationDegrees.S3_OneThirtyFive:
                amt = 180.0f;
                break;
            case RotationDegrees.S4_OneEighty:
                amt = 225.0f;
                break;
            case RotationDegrees.S5_TwoTwentyFive:
                amt = 270.0f;
                break;
            case RotationDegrees.S6_TwoSeventy:
                amt = 315.0f;
                break;
            default:
                amt = 0.0f;
                break;
        }
        Vector3 rotVec3 = new Vector3(0.0f, 0.0f, amt);
        Quaternion rotQuat = Quaternion.Euler(rotVec3);
        transform.rotation = rotQuat;
    }
    public void ForceUpdate()
    {
        ActivateSelectedPrefab();
        SetRotation();
    }
    private void OnDrawGizmosSelected()
    {
        ForceUpdate();
    }
}
