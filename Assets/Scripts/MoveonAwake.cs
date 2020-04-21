using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveonAwake : MonoBehaviour
{
    public float exitSpeed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        
        
    }

    public void FireExitObject()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(0f, exitSpeed, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
