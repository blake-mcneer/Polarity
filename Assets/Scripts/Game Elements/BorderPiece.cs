using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderPiece : MonoBehaviour {
    public enum RotationDegrees{
        None,
        Quarter,
        Half,
        ThreeQuarter,
        Full
    }
    public BorderPieceType type = BorderPieceType.BorderPieceStraight;
    public RotationDegrees rot = RotationDegrees.None;
    public GameObject cornerPrefab;
    public GameObject straightPrefab;

    private void Start()
    {
        ActivateSelectedPrefab();
        SetRotation();
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
        switch(rot){
            case RotationDegrees.None:
                break;
            case RotationDegrees.Full:
                break;
            case RotationDegrees.Quarter:
                amt = 90.0f;
                break;
            case RotationDegrees.Half:
                amt = 180.0f;
                break;
            case RotationDegrees.ThreeQuarter:
                amt = 270.0f;
                break;
                
        }
        Vector3 rotVec3 = new Vector3(0.0f, 0.0f, amt);
        Quaternion rotQuat = Quaternion.Euler(rotVec3);
        transform.rotation = rotQuat;
    }
    private void OnDrawGizmosSelected()
    {
        ActivateSelectedPrefab();
        SetRotation();
    }
}
