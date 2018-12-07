using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ScreenLimits
{
    public float leftLimit;
    public float rightLimit;
    public float topLimit;
    public float bottomLimit;
}
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
    ScreenLimits limits;
    [HideInInspector] public int index;
    Vector3 previousVelocity;
    Rigidbody rb;
    GameManager manager;
    private void Start()
    {
        ConfigureScreenLimits();
        ConfigureBall();
    }
    void ConfigureScreenLimits()
    {
        Vector3 lowerLeftCorner = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 upperRightCorner = Camera.main.ViewportToWorldPoint(Vector3.one);
        limits.leftLimit = lowerLeftCorner.x + transform.localScale.x /2.0f;
        limits.rightLimit = upperRightCorner.x - transform.localScale.x /2.0f;
        limits.bottomLimit = lowerLeftCorner.y + transform.localScale.x/2.0f;
        limits.topLimit = upperRightCorner.y - transform.localScale.x/2.0f;
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
    Vector3 LimitPosition(Vector3 rawPosition)
    {
        float x = Mathf.Max(rawPosition.x, limits.leftLimit);
        x = Mathf.Min(x, limits.rightLimit);
        float y = Mathf.Max(rawPosition.y, limits.bottomLimit);
        y = Mathf.Min(y, limits.topLimit);

        Vector3 limitedPosition = new Vector3(x, y, rawPosition.z);
        return limitedPosition;
    }
    private void Update()
    {
        if (manager == null) ConfigureBall();

        if (manager == null) return;

        if (Mathf.Abs((targetScale - scale)) > 0.05f)
        {
            UpdateScale();
        }else if (shrinkingAway){
            Destroy(gameObject);    
        }

        if (shrinkingAway) return;

        rb.velocity = Vector3.zero;
        Vector2 pulseAffect = manager.AffectOnPosition(transform.position, index);
        float xTarget = transform.position.x + pulseAffect.x * magneticConstant * Time.deltaTime;
        float yTarget = transform.position.y - pulseAffect.y * magneticConstant * Time.deltaTime;
        yTarget -= dropSpeed * Time.deltaTime;
        Vector3 targetPos = LimitPosition(new Vector3(xTarget, yTarget, 0.0f));
        //Vector3 targetPos = new Vector3(xTarget, yTarget, 0.0f);
        //if (targetPos.x > limits.rightLimit) targetPos.x = limits.rightLimit;
        //if (targetPos.x < limits.leftLimit) targetPos.x = limits.leftLimit;
        rb.MovePosition(targetPos);
        //Vector2 testLoc = Camera.main.WorldToViewportPoint(transform.position);
        //Debug.Log(testLoc.x.ToString("0.000") + " : " + testLoc.y.ToString("0.000"));
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
                manager.RemoveFromCurrentAttractionPulses(index);
            }
        }else if (collision.gameObject.tag == "Barrier" || collision.gameObject.tag == "BorderPiece")
        {
            int[] indecesAffected = { index};
            Vector3 pos = (collision.transform.position - transform.position).normalized;
            pos = transform.position + pos;
            manager.AddRepulsion(pos, indecesAffected);
            manager.RemoveFromCurrentAttractionPulses(index);

        }
    }
    void HandleCollisionWithGoal(Goal g)
    {
        if (g.type == type)
        {
            g.HitByMagneticObject(gameObject.GetComponent<MagneticObject>());
            ShrinkAway();
        }
        else
        {
            int[] indecesAffected = { index };
            Vector3 pos = (g.transform.position - transform.position).normalized;
            pos = transform.position + pos;
            manager.AddRepulsion(pos, indecesAffected);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Finish")
        {
            Goal g = other.transform.parent.GetComponent<Goal>();
            HandleCollisionWithGoal(g);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Finish")
        {
            Goal g = other.transform.parent.GetComponent<Goal>();
            HandleCollisionWithGoal(g);
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
