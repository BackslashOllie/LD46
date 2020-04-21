using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonExit : MonoBehaviour
{
    CameraFollow cameraFollowRig;
    MoveonAwake exitTrackingObject;
    bool objectInteractable;
    public float timeToExit = 5f;

    // Start is called before the first frame update
    void Start()
    {
        exitTrackingObject = FindObjectOfType<MoveonAwake>();
    }

    private void RotateSelf()
    {
        gameObject.transform.Rotate(0f, 50 * Time.deltaTime, 0f);
    }

    private void OnTriggerEnter(Collider player)
    {
        //show interface on canvas to interact with exit
        objectInteractable = true;
    }

    private void OnTriggerExit(Collider player)
    {
        objectInteractable = false;
    }

    private IEnumerator ProcessExitScene()
    {
        cameraFollowRig = FindObjectOfType<CameraFollow>();
        cameraFollowRig.followTransform = exitTrackingObject.transform;
        exitTrackingObject.FireExitObject();
        yield return new WaitForSeconds(timeToExit);
        SceneManager.LoadScene("Exit");
    }

    private void Activate()
    {
        if (objectInteractable)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!Player.Instance.hasDragon)
                {
                    UIManager.Instance.FlashText("Dont forget Bobb!", 1f);
                }
                else StartCoroutine(ProcessExitScene());
                
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Activate();
        RotateSelf();
    }
}
