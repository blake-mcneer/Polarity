using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public Transform leftPosition;
    public Transform rightPosition;
    public float moveSpeed = 5.0f;
    bool movingRight = true;
    bool hasReachedTarget = false;
    private void Update()
    {
        float moveAmount = moveSpeed * Time.deltaTime;
        if (!movingRight) moveAmount *= -1.0f;
        Vector3 targetPos = Vector3.right * moveAmount + transform.position;
        if (targetPos.x > rightPosition.position.x)
        {
            targetPos.x = rightPosition.position.x;
            movingRight = false;
        }
        if (targetPos.x < leftPosition.position.x)
        {
            targetPos.x = leftPosition.position.x;
            movingRight = true;
        }
        transform.position = targetPos;
    }
    private void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
    }
}
