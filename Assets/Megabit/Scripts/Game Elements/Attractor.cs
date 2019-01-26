using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour {
    public Vector3 rotationSpeed = Vector3.zero;
    public float strength = 2.0f;
    public GameObject positiveGameObject;
    public GameObject negativeGameObject;
    public GameObject orbitalRingGameObject;
    public GameObject orbitalRingHelperObject;
    public List<MagneticBall> lockedObjects = new List<MagneticBall>();

    void Start () {
        EnablePrefab();
	}
    public void ConfigureAttractor()
    {
        EnablePrefab();
    }
	void EnablePrefab()
    {
        positiveGameObject.SetActive(strength > 0.0f);
        negativeGameObject.SetActive(strength < 0.0f);

    }
	// Update is called once per frame
	void Update () {
        Spin();
        //UpdateLockedObjects();
	}
    void UpdateLockedObjects()
    {
        foreach (MagneticBall b in lockedObjects)
        {
            if (b != null)
            {
                orbitalRingHelperObject.transform.position = b.orbitalLockPosition;
                b.transform.position = orbitalRingHelperObject.transform.position;
            }
        }
    }

    public void CaptureBall(MagneticBall b)
    {
        //b.lockedInOrbit = true;
        //orbitalRingHelperObject.transform.position = b.gameObject.transform.position;
        //b.orbitalLockPosition = b.transform.localPosition;
        //if (!lockedObjects.Contains(b)) {
        //    b.attachedAttractor = this;
        //    lockedObjects.Add(b);
        //    b.transform.parent = transform;
        //}

    }
    public void ReleaseBall(MagneticBall b)
    {
        //b.lockedInOrbit = false;
        //if (lockedObjects.Contains(b))
        //{
        //    lockedObjects.Remove(b);
        //    b.transform.parent = b.originalParent;
        //}
    }

    void Spin()
    {
        Vector3 rotateAmount = rotationSpeed * Time.deltaTime;
        transform.Rotate(rotateAmount);
    }
    private void OnDrawGizmos()
    {
        EnablePrefab();
    }
}
