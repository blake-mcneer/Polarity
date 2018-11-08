using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {
    public float rotationSpeed = 0.0f;
    public SkinnedMeshRenderer renderer;
    GameManager manager;
    void Start () {
        manager = FindObjectOfType<GameManager>();
    }

    void Spin()
    {
        float rotateAmount = rotationSpeed * Time.deltaTime;
        transform.Rotate(0.0f, 0.0f, rotateAmount);
    }
	void Update () {
        Spin();
	}
    IEnumerator AnimateGoal()
    {
        yield return new WaitForSeconds(1.0f);
        renderer.SetBlendShapeWeight(0, 50.0f);
    }
    public void HitByMagneticObject(MagneticObject mag)
    {
         
       Destroy(mag.gameObject);
    } 

}
