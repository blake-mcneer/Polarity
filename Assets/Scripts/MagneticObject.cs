using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticObject : MonoBehaviour {

    public float magneticConstant = 1.0f;
    public float dropSpeed = 1.0f;
    public float placeholderMagneticField = 0.0f;
    Rigidbody rb;
    GameManager manager;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        manager = FindObjectOfType<GameManager>();
    }
    private void Update()
    {
        //transform.position += Vector3.down * dropSpeed * Time.deltaTime;
        //rb.MovePosition(transform.position + Vector3.down * dropSpeed * Time.deltaTime);
        //Vector3 targetPos = transform.position + Vector3.right * magneticConstant * Time.deltaTime * manager.magneticField;
        Vector2 pulseAffect = manager.AffectOnPosition(transform.position);
        float xTarget = transform.position.x + pulseAffect.x * magneticConstant * Time.deltaTime;
        float yTarget = transform.position.y - pulseAffect.y * magneticConstant * Time.deltaTime;
        yTarget -= dropSpeed * Time.deltaTime;
        Vector3 targetPos = new Vector3(xTarget, yTarget, 0.0f);
        //Vector3 targetPos = transform.position + Vector3.right * pulseAffect.x * magneticConstant * Time.deltaTime + Vector3.down * pulseAffect.y * magneticConstant * Time.deltaTime;
        if (targetPos.x > manager.rightLimit.position.x) targetPos.x = manager.rightLimit.position.x;
        if (targetPos.x < manager.leftLimit.position.x) targetPos.x = manager.leftLimit.position.x;
        rb.MovePosition(targetPos);
//        transform.position = targetPos;
    }
}
