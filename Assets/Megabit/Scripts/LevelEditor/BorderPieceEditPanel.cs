﻿using System.Collections;
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
        borderPieceRotationText.text = "0";
        borderPieceTypeText.text = "Straight";
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
                degRot = RotationDegrees.S1_FortyFive;
                rotString = "45";
                break;
            case RotationDegrees.S1_FortyFive:
                degRot = RotationDegrees.S2_Ninety;
                rotString = "90";
                break;
            case RotationDegrees.S2_Ninety:
                degRot = RotationDegrees.S3_OneThirtyFive;
                rotString = "135";
                break;
            case RotationDegrees.S3_OneThirtyFive:
                degRot = RotationDegrees.S4_OneEighty;
                rotString = "180";
                break;
            case RotationDegrees.S4_OneEighty:
                degRot = RotationDegrees.S5_TwoTwentyFive;
                rotString = "225";
                break;
            case RotationDegrees.S5_TwoTwentyFive:
                degRot = RotationDegrees.S6_TwoSeventy;
                rotString = "270";
                break;
            case RotationDegrees.S6_TwoSeventy:
                degRot = RotationDegrees.S7_ThreeFifteen;
                rotString = "315";
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
