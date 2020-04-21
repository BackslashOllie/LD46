
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float walkSpeed, runSpeed, backSpeed, lookSpeed;
    public float jumpForce;
    public LayerMask mouseNavMask;
    public float groundCheckDistance = 0.1f;
    private Player player;

    private bool isMoving;
 
    public Animator anim;
    private Rigidbody rigidBody;
    private bool isGrounded;
    public bool IsRunning;
    private bool jumpPressed;

    //Fatigue
    public bool fatigued;
    public float fatigueRecoverRate = 2f;
    private float fatigueSpeed;
    private bool releaseSprintRequired;

    private bool CanMove
    {
        get { return !player.Recovering && !player.isDead; }
    }
 
    private void Start () 
    {
        rigidBody = GetComponent<Rigidbody>();
        player = GetComponent<Player>();
    }

    private void Update()
    {
        CheckGroundStatus();
        if (CanMove) RotateToCamera();
        if (Input.GetButtonDown("Jump"))
        {
            jumpPressed = true;
        }
        IsRunning = Input.GetButton("Fire3");
        if (releaseSprintRequired && !IsRunning) releaseSprintRequired = false;
        if (fatigueSpeed <= walkSpeed && fatigued) fatigued = false;
    }

    private void FixedUpdate()
    {
        if (CanMove)
        {
            float verticalInput = Input.GetAxis("Vertical");
            float verticalRawInput = Input.GetAxisRaw("Vertical");
            
            if (fatigueSpeed >= runSpeed)
            {
                fatigued = true;
                releaseSprintRequired = true;
            }
            float currentSpeed = IsRunning ? fatigued || releaseSprintRequired ? fatigueSpeed : runSpeed : walkSpeed;
            if (currentSpeed == runSpeed && !fatigued)
                fatigueSpeed += Time.deltaTime;
            else if (fatigueSpeed > 0 && !IsRunning)
            {
                fatigueSpeed -= (fatigueRecoverRate * 2) * Time.deltaTime;
            }
            else if (fatigued && currentSpeed > walkSpeed)
            {
                fatigueSpeed -= fatigueRecoverRate * Time.deltaTime;
            }
            else if (fatigued)
            {
                fatigueSpeed -= (fatigueRecoverRate * 4) * Time.deltaTime;
            }
            currentSpeed = verticalRawInput < 0 ? backSpeed : currentSpeed;

            Vector3 moveVect = transform.forward;
            if (verticalInput != 0f)
            {
                moveVect *= verticalRawInput;
                moveVect = moveVect.normalized * currentSpeed * Time.deltaTime;
                rigidBody.MovePosition(transform.position + moveVect);
            }

            if (jumpPressed)
            {
                jumpPressed = false;
                if (isGrounded)
                {
                    anim.SetTrigger("Jump");
                    rigidBody.AddForce(new Vector3(rigidBody.velocity.x, jumpForce, rigidBody.velocity.z));
                }
            }

            anim.SetFloat("Speed", verticalInput * currentSpeed);
        }
    }

    private void RotateToCamera()
    {
        // rotation
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mouseNavMask))
        {
            Vector3 mousePos = hit.point + (Vector3.up);

            Vector3 dir = mousePos - transform.position;

            if (dir.magnitude > 3)
            {
                Quaternion lookRot = Quaternion.LookRotation(dir);
                float xRot = Mathf.Clamp(lookRot.eulerAngles.x, 0, 15);
                float zRot = Mathf.Clamp(lookRot.eulerAngles.z, 0, 15);

                Quaternion modRot = Quaternion.Euler(0, lookRot.eulerAngles.y, 0);

                transform.rotation = Quaternion.Slerp(transform.rotation, modRot, lookSpeed * Time.deltaTime);
            }
        }
    }
    
    void CheckGroundStatus()
    {
        RaycastHit hitInfo;
#if UNITY_EDITOR
        // helper to visualise the ground check ray in the scene view
        Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * groundCheckDistance));
#endif
        // 0.1f is a small offset to start the ray from inside the character
        // it is also good to note that the transform position in the sample assets is at the base of the character
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, groundCheckDistance))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        anim.SetBool("Grounded", isGrounded);
    }
}
