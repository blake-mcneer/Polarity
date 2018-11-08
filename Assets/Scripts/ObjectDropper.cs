using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDropper : MonoBehaviour {

    public Transform leftDropLimit;
    public Transform rightDropLimit;

    public GameObject[] prefabs;
    public float dropFrequency = 1.0f;
    public float[] dropPercentages;
    public bool randomSequence = false;
	void Start () {
        if (randomSequence)
        {
            float xPos = Random.Range(leftDropLimit.position.x, rightDropLimit.position.x);
            StartCoroutine(DropObjectAfterDelay(prefabs[0],xPos, 0.0f));
        }
        else
        {
            RunSequence(LoadSequence());
        }
    }
    List<DropSequenceItem> LoadSequence()
    {
        List<DropSequenceItem> sequence = new List<DropSequenceItem>();

        //sequence.Add(new DropSequenceItem(Random.Range(leftDropLimit.position.x, rightDropLimit.position.x), 1, 1.0f, MagneticType.Medium));
        //sequence.Add(new DropSequenceItem(Random.Range(leftDropLimit.position.x, rightDropLimit.position.x), 2, 3.0f, MagneticType.High));
        //sequence.Add(new DropSequenceItem(Random.Range(leftDropLimit.position.x, rightDropLimit.position.x), 3, 4.2f, MagneticType.Medium));
        //sequence.Add(new DropSequenceItem(Random.Range(leftDropLimit.position.x, rightDropLimit.position.x), 4, 8.5f, MagneticType.Low));
        //sequence.Add(new DropSequenceItem(Random.Range(leftDropLimit.position.x, rightDropLimit.position.x), 5, 10.0f, MagneticType.Medium));
        sequence.Add(new DropSequenceItem(10.0f, 1, 1.0f, MagneticType.M2));
        sequence.Add(new DropSequenceItem(20.0f, 2, 3.0f, MagneticType.M3));
        sequence.Add(new DropSequenceItem(30.0f, 3, 4.2f, MagneticType.M2));
        sequence.Add(new DropSequenceItem(40.0f, 4, 8.5f, MagneticType.M1));
        sequence.Add(new DropSequenceItem(50.0f, 5, 10.0f, MagneticType.M2));

        return sequence;
    }
    void RunSequence(List<DropSequenceItem> sequence)
    {
        foreach(DropSequenceItem item in sequence)
        {
            GameObject prefab = prefabs[(int) item.magneticType];
            StartCoroutine(DropObjectAfterDelay(prefab, item.xPosition, item.sequenceTimimg));            
        }
    }
	    
    IEnumerator DropObjectAfterDelay(GameObject prefab,float xPos, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Vector3 dropPosition = new Vector3(xPos, leftDropLimit.position.y, leftDropLimit.position.z);
        Instantiate(prefab,dropPosition, Quaternion.Euler(0.0f, 0.0f, 0.0f), transform.parent);
        if (randomSequence)
        {
            int nextObject = Random.Range(0, 3);
            float nextX = Random.Range(leftDropLimit.position.x, rightDropLimit.position.x);
            StartCoroutine(DropObjectAfterDelay(prefabs[nextObject],nextX, Random.Range(dropFrequency * 0.75f, dropFrequency * 1.5f)));
        }
    }

	void Update () {
		
	}
}
