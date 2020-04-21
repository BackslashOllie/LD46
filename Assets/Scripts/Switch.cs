using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public MovingPlatform movingPlatform;
    bool playerInRange = false;
    bool monolithIsActive = false;
    public bool dontShowText = false;
    UIManager uiManager;
    // Start is called before the first frame update
    void Start()
    {
        if (movingPlatform == null)
        {
            Debug.Log("No platform attached.");
            Destroy(gameObject);
        }
        uiManager = FindObjectOfType<UIManager>();
    }

    private void OnTriggerEnter(Collider player)
    {
        if (player.tag == "Player")
            if (!monolithIsActive)
        {
            playerInRange = true;
                if (!dontShowText)
                {
                    uiManager.EnableActivateMonolithMessage();
                }
        }
    }



    private void OnTriggerExit(Collider player)
    {
        if (player.tag == "Player")
        {
            playerInRange = false;
            uiManager.DisableActivateMonolithMessage();
        }
    }

    private void CheckForActivation()
    {
        if (playerInRange&&Input.GetKeyDown(KeyCode.E))
        {
            monolithIsActive = true;
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                movingPlatform.ActivateElevator();
            }
        }
        CheckForActivation();
    }
}
