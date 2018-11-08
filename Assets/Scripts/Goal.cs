using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {
    public float rotationSpeed = 0.0f;
    public SkinnedMeshRenderer renderer;
    public MagneticType type;
    public GoalBarrier barrier = GoalBarrier.BarrierNone;
    public Material M1_Material;
    public Material M2_Material;
    public Material M3_Material;
    public GameObject Barrier25;
    public GameObject Barrier50;
    public GameObject Barrier75;
    int positiveScore = 100;
    int negativeScore = -50;
    GameManager manager;
    void Start () {
        manager = FindObjectOfType<GameManager>();
        SetBarrier();
        SetMaterial();
    }

    void SetMaterial()
    {
        Material[] mats = renderer.materials;
        switch (type)
        {
            case MagneticType.M1:
                mats[0] = M1_Material;
                break;
            case MagneticType.M2:
                mats[0] = M2_Material;
                break;
            case MagneticType.M3:
                mats[0] = M3_Material;
                break;
        }
        renderer.materials = mats;
    }
    void SetBarrier()
    {
        switch (barrier)
        {
            case GoalBarrier.BarrierNone:
                Barrier25.SetActive(false);
                Barrier50.SetActive(false);
                Barrier75.SetActive(false);
                break;
            case GoalBarrier.Barrier25:
                Barrier25.SetActive(true);
                Barrier50.SetActive(false);
                Barrier75.SetActive(false);
                break;
            case GoalBarrier.Barrier50:
                Barrier25.SetActive(false);
                Barrier50.SetActive(true);
                Barrier75.SetActive(false);
                break;
            case GoalBarrier.Barrier75:
                Barrier25.SetActive(false);
                Barrier50.SetActive(false);
                Barrier75.SetActive(true);
                break;
        }
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
        if (mag.type == type){
            manager.AddScore(positiveScore * mag.totalCount);
        }else{
            manager.AddScore(negativeScore * mag.totalCount);
        }
        Destroy(mag.gameObject);
    }
    private void OnDrawGizmosSelected()
    {
        SetBarrier();
        SetMaterial();        
    }
}
