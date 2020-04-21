using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lantern : MonoBehaviour
{
    UIManager uiManager; //all things UI :)
    Player playerObject; //reference to player object
    LanternGuide lanternGuide; // reference to the path of lanterns
    public float lanternFuelAmount; //How much fuel left
    bool lanternUsed = false; //track if used for UI prompt

    private void Start()
    {
        playerObject = FindObjectOfType<Player>();
        uiManager = FindObjectOfType<UIManager>();
        lanternGuide = FindObjectOfType<LanternGuide>();
    }

    private void OnTriggerEnter(Collider player)
    {
        if (player.tag.Contains("Player") && !lanternUsed && this == lanternGuide.CurrentLantern)
        {
            uiManager.EnableFillTorchMessage();
            playerObject.InRangeOfLantern(true, lanternFuelAmount, this);
        }
    }

    private void OnTriggerExit(Collider player)
    {
        if (player.tag.Contains("Player"))
        {
            uiManager.DisableFillTorchMessage();
            playerObject.InRangeOfLantern(false, lanternFuelAmount, this);
        }
    }

    public bool LanternUsed() // called from player
    {
        if (!lanternUsed)
        {
            lanternUsed = true;
            uiManager.DisableFillTorchMessage();
            lanternFuelAmount = 0;
            lanternGuide.NextLantern();
            return true;
        }
        return false;
    }

    
    public void ToggleLightsOff() //called from lantern path
    {
        transform.Find("LanternFX").gameObject.SetActive(false);
    }

    public void ToggleLightsOn() //called from lantern path
    {
        transform.Find("LanternFX").gameObject.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
