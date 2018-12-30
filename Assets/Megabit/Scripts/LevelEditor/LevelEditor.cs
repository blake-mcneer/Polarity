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
    void MoveObject()
    {
        if (objectHeld == null) return;
        Vector2 loc = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 currentInput = new Vector3(loc.x, loc.y, 0.0f);

        Vector3 newPos = positionStarted - (inputStarted - currentInput);
        newPos = new Vector3((int)newPos.x, (int)newPos.y, (int)newPos.z);
        objectHeld.transform.position = newPos;
        //Debug.Log(objectHeld.transform.position);

    }

}
