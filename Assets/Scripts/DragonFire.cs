using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonFire : MonoBehaviour
{
    public float fireLifeSpan = 3f;
    public float fireVelocity = 10f; //units per second travel speed
    Rigidbody fireShotRigidBody;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FireShotSelfDestruct());
        fireShotRigidBody = GetComponent<Rigidbody>();
        transform.position = transform.parent.position;
        var locVel = transform.InverseTransformDirection(fireShotRigidBody.velocity);
        locVel.z = fireVelocity;
        fireShotRigidBody.velocity = transform.TransformDirection(locVel);
        transform.parent = null;
    }

    private IEnumerator FireShotSelfDestruct()
    {
        yield return new WaitForSeconds(fireLifeSpan);
        Destroy(gameObject);
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
