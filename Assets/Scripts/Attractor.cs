using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour {
    public Vector3 rotationSpeed = Vector3.zero;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Spin();
	}


    void Spin()
    {
        Vector3 rotateAmount = rotationSpeed * Time.deltaTime;
        transform.Rotate(rotateAmount);
    }
}
