using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour {
    public Vector3 rotationSpeed = Vector3.zero;
    public float strength = 2.0f;
    public GameObject positiveGameObject;
    public GameObject negativeGameObject;

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
