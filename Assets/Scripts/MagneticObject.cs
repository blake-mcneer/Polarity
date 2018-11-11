using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticObject : MonoBehaviour {

    public Material M1_Material;
    public Material M2_Material;
    public Material M3_Material;

    public float magneticConstant = 1.0f;
    public float dropSpeed = 1.0f;
    public float placeholderMagneticField = 0.0f;
    public MagneticType type;
    public int totalCount = 1;
    public TextMesh tMesh;
    float scale = 1.0f;
    float targetScale = 1.0f;
    float scaleSpeed = 4.0f;
    bool shrinkingAway = false;
    [HideInInspector] public int index;
    Vector3 previousVelocity;
    Rigidbody rb;
    GameManager manager;
    private void Start()
    {
        ConfigureBall();
    }
    public void ConfigureBall()
    {
        rb = GetComponent<Rigidbody>();
        manager = FindObjectOfType<GameManager>();
        SetMaterial();
    }

    public void SetMaterial()
    {
        Material mat = GetComponent<Renderer>().sharedMaterial;
        switch (type)
        {
            case MagneticType.M1:
                mat = M1_Material;
                break;
            case MagneticType.M2:
                mat = M2_Material;
                break;
            case MagneticType.M3:
                mat = M3_Material;
                break;
        }
        Renderer rend = GetComponent<Renderer>();
        rend.sharedMaterial = mat;
        //renderer.sharedMaterials = mats;
    }

    void UpdateScale()
    {
        float direction = shrinkingAway ? -1.0f : 1.0f;
        scale += Time.deltaTime * scaleSpeed * direction;
        if (shrinkingAway){
            scale = Mathf.Max(0.0f, scale);
        }else{
            scale = Mathf.Min(scale, targetScale);
        }
        transform.localScale = Vector3.one * scale;
    }
    private void Update()
    {
        if (Mathf.Abs((targetScale - scale)) > 0.05f)
        {
            UpdateScale();
        }else if (shrinkingAway){
            Destroy(gameObject);    
        }


        rb.velocity = Vector3.zero;
        Vector2 pulseAffect = manager.AffectOnPosition(transform.position, index);
        float xTarget = transform.position.x + pulseAffect.x * magneticConstant * Time.deltaTime;
        float yTarget = transform.position.y - pulseAffect.y * magneticConstant * Time.deltaTime;
        yTarget -= dropSpeed * Time.deltaTime;
        Vector3 targetPos = new Vector3(xTarget, yTarget, 0.0f);
        if (targetPos.x > manager.rightLimit.position.x) targetPos.x = manager.rightLimit.position.x;
        if (targetPos.x < manager.leftLimit.position.x) targetPos.x = manager.leftLimit.position.x;
        rb.MovePosition(targetPos);
        //rb.AddForce(new Vector3(pulseAffect.x, -pulseAffect.y, 0.0f),ForceMode.VelocityChange);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "MagneticBall")
        {
            MagneticObject mag = collision.gameObject.GetComponent<MagneticObject>();
            if (mag.type == type)
            {
                if (index > mag.index)
                {
                    AbsorbObject(mag);
                }
            }
            else
            {
                int[] indecesAffected = { index, mag.index };
                manager.AddRepulsion(transform.position, indecesAffected);
            }
        }else if (collision.gameObject.tag == "Barrier")
        {
            int[] indecesAffected = { index};
            Vector3 pos = (collision.transform.position - transform.position).normalized;
            pos = transform.position + pos;
            manager.AddRepulsion(pos, indecesAffected);

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Finish")
        {
            Goal g = other.transform.parent.GetComponent<Goal>();
            Debug.Log(g);
            g.HitByMagneticObject(gameObject.GetComponent<MagneticObject>());
        }
    }
    public void AbsorbObject(MagneticObject obj)
    {
        totalCount+= obj.totalCount;
        tMesh.text = totalCount.ToString();
        targetScale = (float)(totalCount -1) * 0.25f + 1.0f;
        obj.ShrinkAway();
//        Destroy(obj.gameObject);
        
    }
    public void ShrinkAway()
    {
        targetScale = 0.0f;
        shrinkingAway = true;
    }
    private void OnDrawGizmosSelected()
    {
        SetMaterial();
    }
}
