using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticObject : MonoBehaviour {

    public float magneticConstant = 1.0f;
    public float dropSpeed = 1.0f;
    public float placeholderMagneticField = 0.0f;
    public MagneticType type;
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
        Vector2 pulseAffect = manager.AffectOnPosition(transform.position);
        float xTarget = transform.position.x + pulseAffect.x * magneticConstant * Time.deltaTime;
        float yTarget = transform.position.y - pulseAffect.y * magneticConstant * Time.deltaTime;
        yTarget -= dropSpeed * Time.deltaTime;
        Vector3 targetPos = new Vector3(xTarget, yTarget, 0.0f);
        if (targetPos.x > manager.rightLimit.position.x) targetPos.x = manager.rightLimit.position.x;
        if (targetPos.x < manager.leftLimit.position.x) targetPos.x = manager.leftLimit.position.x;
        rb.MovePosition(targetPos);
        //rb.AddForce(new Vector3(pulseAffect.x, -pulseAffect.y, 0.0f),ForceMode.VelocityChange);
    }
}
