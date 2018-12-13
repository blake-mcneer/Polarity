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
    Animator anim;
    int positiveScore = 100;
    int negativeScore = -50;
    GameManager manager;
    void Start () {
        manager = FindObjectOfType<GameManager>();
        ConfigureSettings();
        anim = GetComponentInChildren<Animator>();
    }
    public void ConfigureSettings()
    {
        SetBarrier();
        SetMaterial();
    }

    void SetMaterial()
    {
        Material[] mats = renderer.sharedMaterials;
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
        renderer.sharedMaterials = mats;
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
        int i = 1;
        while (i < 100)
        {
            yield return new WaitForSeconds(0.25f);
            renderer.SetBlendShapeWeight(0, (float)i);
            i++;
        }
        while (i > 0)
        {
            yield return new WaitForSeconds(0.25f);
            renderer.SetBlendShapeWeight(0, (float)i);
            i--;
        }
    }
    IEnumerator WaitForTime(float duration)
    {
        anim.SetBool("BallAbsorbed", true);
        yield return new WaitForSeconds(duration);
        anim.SetBool("BallAbsorbed", false);
    }
    public void AnimateGrow()
    {
        StopAllCoroutines();
        anim.SetBool("BallAbsorbed", false);
        StartCoroutine(WaitForTime(1.0f));
    }
    public void HitByMagneticObject(MagneticObject mag)
    {
        if (mag.type == type){
            if (!mag.hasBeenScored){
                mag.hasBeenScored = true;
                manager.AddScore(positiveScore * mag.totalCount);
//                manager.RemoveActivePulses();
            }
        }else{
            manager.AddScore(negativeScore * mag.totalCount);
        }
        AnimateGrow();

        //StartCoroutine(AnimateGoal());
//        Destroy(mag.gameObject);
    }
    private void OnDrawGizmosSelected()
    {
        ConfigureSettings();
    }
}
