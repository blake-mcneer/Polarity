using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticObject : MonoBehaviour {

    public float magneticConstant = 1.0f;
    public float dropSpeed = 1.0f;
    public float placeholderMagneticField = 0.0f;
    public MagneticType type;
    public int totalCount = 1;
    public TextMesh tMesh;
    [HideInInspector] public int index;
    Vector3 previousVelocity;
    Rigidbody rb;
    GameManager manager;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        manager = FindObjectOfType<GameManager>();
    }
    private void Update()
    {
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
        totalCount++;
        tMesh.text = totalCount.ToString();
        Destroy(obj.gameObject);
        
    }
}
