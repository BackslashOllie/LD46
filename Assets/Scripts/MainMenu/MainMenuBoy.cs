using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuBoy : MonoBehaviour
{

    public Animator anim;

    private float lastScaredTime;
    // Start is called before the first frame update
    void Start()
    {
        lastScaredTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastScaredTime > 10f)
        {
            lastScaredTime = Time.time;
            anim.SetTrigger("Scared");
        }
    }
}
