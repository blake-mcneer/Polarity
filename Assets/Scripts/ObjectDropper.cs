using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDropper : MonoBehaviour {
    public Transform leftDropLimit;
    public Transform rightDropLimit;

    public GameObject[] prefabs;
    public float dropFrequency = 1.0f;
    public float[] dropPercentages;
	void Start () {
        StartCoroutine(DropObjectAfterDelay(prefabs[0], 0.0f));
	}
	    
    IEnumerator DropObjectAfterDelay(GameObject prefab, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        float xPos = Random.Range(leftDropLimit.position.x, rightDropLimit.position.x);
        Vector3 dropPosition = new Vector3(xPos, leftDropLimit.position.y, leftDropLimit.position.z);
        Instantiate(prefab,dropPosition, Quaternion.Euler(0.0f, 0.0f, 0.0f), transform.parent);
        int nextObject = Random.Range(0, 3);
        StartCoroutine(DropObjectAfterDelay(prefabs[nextObject], Random.Range(dropFrequency * 0.75f, dropFrequency * 1.5f)));
    }

	void Update () {
		
	}
}
