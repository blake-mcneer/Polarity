using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BorderPieceEditPanel : MonoBehaviour {

    
    public BorderPiece borderPieceBeingModified;
    public Text borderPieceTypeText;
    public Text borderPieceRotationText;

    public void SetBorderPiece(BorderPiece newBorderPiece)
    {
        borderPieceBeingModified = newBorderPiece;
        borderPieceTypeText.text = newBorderPiece.type.ToString();
    }
    public void ModificationsComplete()
    {
        borderPieceBeingModified = null;
        gameObject.SetActive(false);
    }
    public void ChangeType()
    {
        if (borderPieceBeingModified == null) return;
        BorderPieceType borderType;
        string borderString;
        switch (borderPieceBeingModified.type)
        {
            case BorderPieceType.BorderPieceCorner:
                borderType = BorderPieceType.BorderPieceStraight;
                borderString = "Corner";
                break;
            case BorderPieceType.BorderPieceStraight:
                borderType = BorderPieceType.BorderPieceCorner;
                borderString = "Straight";
                break;
            default:
                borderType = BorderPieceType.BorderPieceCorner;
                borderString = "Corner";
                break;
        }
        borderPieceBeingModified.type = borderType;
        borderPieceBeingModified.ForceUpdate();
        borderPieceTypeText.text = borderString;
    }
    public void ApplyRotation()
    {
        if (borderPieceBeingModified == null) return;
        RotationDegrees degRot;
        string rotString;
        switch (borderPieceBeingModified.rot)
        {
            case RotationDegrees.None:
                degRot = RotationDegrees.Quarter;
                rotString = "90";
                break;
            case RotationDegrees.Quarter:
                degRot = RotationDegrees.Half;
                rotString = "180";
                break;
            case RotationDegrees.Half:
                degRot = RotationDegrees.ThreeQuarter;
                rotString = "270";
                break;
            case RotationDegrees.ThreeQuarter:
                degRot = RotationDegrees.None;
                rotString = "0";
                break;
            default:
                degRot = RotationDegrees.None;
                rotString = "0";
                break;
        }
        borderPieceBeingModified.rot = degRot;
        borderPieceBeingModified.ForceUpdate();
        borderPieceRotationText.text = rotString;
    }
    public void Delete()
    {
        Destroy(borderPieceBeingModified.gameObject);
        ModificationsComplete();
    }
}
