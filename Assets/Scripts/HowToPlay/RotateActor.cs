using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateActor : MonoBehaviour
{
    public bool IsBoy, IsRunning, IsHit, IsLose, IsAttack;
    public Animator anim;
    public float animWaitTime;
    private float animStartTime;

    // Start is called before the first frame update
    void Start()
    {
        if (IsBoy && IsRunning)
        {
            anim.SetFloat("Speed", 7.8f);
        }
        if (IsBoy && IsLose)
        {
            anim.SetTrigger("Lose");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 10 * Time.deltaTime, 0);
        if (IsBoy && IsHit)
        {
            if (Time.time - animStartTime > animWaitTime)
            {
                animStartTime = Time.time;
                anim.SetTrigger("Hit");
            }
                
        }
        if (IsBoy && IsAttack)
        {
            if (Time.time - animStartTime > animWaitTime)
            {
                animStartTime = Time.time;
                anim.SetTrigger("Attack");
            }
        }
            
    }
}
