using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditor : MonoBehaviour
{
    public AttractorEditPanel attractorPanel;

    GameObject objectHeld;
    Vector3 positionStarted;
    Vector3 inputStarted;


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
                MagneticObject magneticObject = magneticObjectObject.GetComponent<MagneticObject>();
                GrabObject(magneticObjectObject);
            }
            else if (tagHit == "Barrier")
            {
                GameObject attractorObject = hit.collider.transform.parent.gameObject;
                Attractor attractor = attractorObject.GetComponent<Attractor>();
                GrabObject(attractorObject);
                attractorPanel.gameObject.SetActive(true);
                attractorPanel.SetAttractor(attractor);
            }
            else if (tagHit == "Finish")
            {
                GameObject goalObject = hit.collider.transform.parent.gameObject;
                Goal goal = goalObject.GetComponent<Goal>();
                GrabObject(goalObject);
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

        objectHeld.transform.position = positionStarted - (inputStarted - currentInput);
        
    }

}
