using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TheWorkLight : MonoBehaviour
{
    Light workLight;
    bool toggled = false;
    float cooldown = 1f;
    // Start is called before the first frame update
    void Start()
    {
        workLight = GetComponent<Light>();
    }

    private void ToggleWorkLight()
    {
        if ((Input.GetKey(KeyCode.L)) && (toggled == false))
        {
            toggled = true;
            StartCoroutine(LightSwitchToggleCooldown());
            workLight.enabled = !workLight.enabled;
        }
    }

    private IEnumerator LightSwitchToggleCooldown()
    {
        yield return new WaitForSeconds(cooldown);
        toggled = false;
    }
    // Update is called once per frame
    void Update()
    {
        ToggleWorkLight();
    }
}
