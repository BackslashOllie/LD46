using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtmosphericFXController : MonoBehaviour
{
    torchAtmosphericSounds atmosphericSounds;
    float fXduration;
    // Start is called before the first frame update
    void Start()
    {
        transform.parent = null;
        atmosphericSounds = FindObjectOfType<torchAtmosphericSounds>();
        fXduration = atmosphericSounds.GetVisualFXDuration();
        StartCoroutine(DestroyAfterTimeExpires());
    }

    private IEnumerator DestroyAfterTimeExpires()
    {
        yield return new WaitForSeconds(fXduration);
        Destroy(gameObject);
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
