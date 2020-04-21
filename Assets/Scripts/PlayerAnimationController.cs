using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    Animator playerAnimator;
    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();   
    }

    public void PlayerWalkAnimation()
    {
        playerAnimator.SetBool("walking",true);
    }

    public void PlayerStopWalking()
    {
        playerAnimator.SetBool("walking", false);
    }

    public void PlayerLightTorch()
    {
        playerAnimator.SetTrigger("torchlight");
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
