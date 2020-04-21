using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDeath : MonoBehaviour
{
    torchlight _torchlight;
    // Start is called before the first frame update
    void Start()
    {
        _torchlight = FindObjectOfType<torchlight>();
    }

    private void OnTriggerEnter(Collider player)
    {
        if (player.tag == "Player")
        {
            _torchlight.torchLife = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
