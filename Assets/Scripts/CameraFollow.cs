using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform followTransform;
    public float moveSpeed;
    Vector3 moveVelocity;
    
    public void LateUpdate()
    {
        Vector3 desiredPos = new Vector3(followTransform.position.x , followTransform.position.y , followTransform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, desiredPos, ref moveVelocity, moveSpeed);
    }
}

