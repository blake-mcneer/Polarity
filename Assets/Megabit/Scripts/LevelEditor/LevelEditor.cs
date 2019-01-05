using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditor : MonoBehaviour
{
    public AttractorEditPanel attractorPanel;
    public GoalEditPanel goalPanel;
    public MagneticBallEditPanel magneticBallPanel;
    public GeneralEditor editorPanel;
    public BorderPieceEditPanel borderPiecePanel;
    ScreenLimits limits;

    int mLevel = 0;
    public int Level
    {
        get
        {
            return mLevel;
        }
        set
        {
            mLevel = value;
            LevelManager lm = FindObjectOfType<LevelManager>();
            lm.LoadLevel(mLevel);
        }
    }

    GameObject objectHeld;
    Vector3 positionStarted;
    Vector3 inputStarted;

    private void Start()
    {
        HideAllPanels();
        ConfigureScreenLimits();
    }
    void ConfigureScreenLimits()
    {
        Vector3 lowerLeftCorner = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 upperRightCorner = Camera.main.ViewportToWorldPoint(Vector3.one);
        Vector3 upperRightScreenPoint = Camera.main.ViewportToScreenPoint(Vector3.one);
        upperRightScreenPoint.y = Screen.height * (1.0f - GameSingleton.Instance.bannerPercentage);
        upperRightCorner = Camera.main.ScreenToWorldPoint(upperRightScreenPoint);

        limits.leftLimit = lowerLeftCorner.x + transform.localScale.x / 2.0f;
        limits.rightLimit = upperRightCorner.x - transform.localScale.x / 2.0f;
        limits.bottomLimit = lowerLeftCorner.y + transform.localScale.x / 2.0f;
        limits.topLimit = upperRightCorner.y - transform.localScale.x / 2.0f;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckForGrab();
        }else if (Input.GetMouseButton(0)){
            MoveObject();
        }
        if (Input.GetMouseButtonUp(0))
        {
            objectHeld = null;
        }
    }
    void HideAllPanels()
    {
        attractorPanel.gameObject.SetActive(false);
        goalPanel.gameObject.SetActive(false);
        magneticBallPanel.gameObject.SetActive(false);
    }

    void CheckForGrab()
    {
        Vector2 loc = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 tapLocation = new Vector3(loc.x, loc.y, 10.0f);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            string tagHit = hit.collider.gameObject.tag;
            if (tagHit == "MagneticBall")
            {
                GameObject magneticObjectObject = hit.collider.transform.gameObject;
                MagneticBall magneticObject = magneticObjectObject.GetComponent<MagneticBall>();
                GrabObject(magneticObjectObject);
                HideAllPanels();
                magneticBallPanel.gameObject.SetActive(true);
                magneticBallPanel.SetMagneticObject(magneticObject);

            }
            else if (tagHit == "BorderPiece")
            {
                GameObject borderPieceObject = hit.collider.transform.parent.gameObject;
                BorderPiece borderPiece = borderPieceObject.GetComponent<BorderPiece>();
                GrabObject(borderPieceObject);
                HideAllPanels();
                borderPiecePanel.gameObject.SetActive(true);
                borderPiecePanel.SetBorderPiece(borderPiece);
            }
            else if (tagHit == "Barrier")
            {
                GameObject attractorObject = hit.collider.transform.parent.gameObject;
                Attractor attractor = attractorObject.GetComponent<Attractor>();
                GrabObject(attractorObject);
                HideAllPanels();
                attractorPanel.gameObject.SetActive(true);
                attractorPanel.SetAttractor(attractor);
            }
            else if (tagHit == "Finish")
            {
                GameObject goalObject = hit.collider.transform.parent.gameObject;
                Goal goal = goalObject.GetComponent<Goal>();
                GrabObject(goalObject);
                HideAllPanels();
                goalPanel.gameObject.SetActive(true);
                goalPanel.SetGoal(goal);
            }
        }
    }

    void GrabObject(GameObject obj)
    {
        objectHeld = obj;
        positionStarted = obj.transform.position;
        Vector2 loc = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        inputStarted = new Vector3(loc.x, loc.y, 0.0f);
    }
    float RoundedPosition(int intVal, float decVal)
    {
        float returnVal = (float)intVal;
        if (decVal > 0.75f){
            returnVal += 1.0f;
        }else if (decVal > 0.25f){
            returnVal += 0.5f;
        }
        //if (decVal > 0.95f){
        //    returnVal += 1.0f;
        //}else if (decVal > 0.85f){
        //    returnVal += 0.9f;
        //}else if (decVal > 0.75f){
        //    returnVal += 0.8f;
        //}else if (decVal > 0.65f){
        //    returnVal += 0.7f;
        //}else if (decVal > 0.55f){
        //    returnVal += 0.6f;
        //}else if (decVal > 0.45f){
        //    returnVal += 0.5f;
        //}else if (decVal > 0.35f){
        //    returnVal += 0.4f;
        //}else if (decVal > 0.25f){
        //    returnVal += 0.3f;
        //}else if (decVal > 0.15f){
        //    returnVal += 0.2f;
        //}else if (decVal > 0.05){
        //    returnVal += 0.1f;
        //}

        return returnVal;
    }

    void MoveObject()
    {
        if (objectHeld == null) return;
        Vector2 loc = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 currentInput = new Vector3(loc.x, loc.y, 0.0f);

        Vector3 newPos = positionStarted - (inputStarted - currentInput);
        Vector3 intVals = new Vector3((int)newPos.x, (int)newPos.y, (int)newPos.z);
        Vector3 remVals = new Vector3(newPos.x % intVals.x, newPos.y % intVals.y, 0.0f);

        newPos = new Vector3(RoundedPosition((int)intVals.x, remVals.x), RoundedPosition((int)intVals.y, remVals.y), newPos.z);

        newPos.x = Mathf.Max(newPos.x, limits.leftLimit); 
        newPos.x = Mathf.Min(newPos.x, limits.rightLimit);
        newPos.y = Mathf.Max(newPos.y, limits.bottomLimit);
        newPos.y = Mathf.Min(newPos.y, limits.topLimit);

        objectHeld.transform.position = newPos;

    }

}
