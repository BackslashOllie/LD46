using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoysGhost : MonoBehaviour
{
    public float verticalVelocity = 4f;


    // Start is called before the first frame update
    void Start()
    {
        
        FindObjectOfType<CameraFollow>().followTransform = this.transform;
        transform.parent = null;  // Split off from the player
        GetComponent<Rigidbody>().velocity = new Vector3(0f, verticalVelocity, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
