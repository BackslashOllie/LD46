using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{


    [Header("Note the length of these arrays should be equal.  Minimum 2 points")]
    [SerializeField] GameObject[] movePos = null; //movement positions for platform
    [SerializeField] private float[] moveTime = null; //move time for each segment
    [SerializeField] private float[] waitTime = null; //wait time at the end of each move
    [SerializeField] private bool returnTrip = true;  //does the platform go backwards?

    [Header("These Boolean values to be used if functions as elevator(player activated)")]
    [SerializeField] private bool platformIsActiveObject = false;
    [SerializeField] private bool elevatorIsActive = false;

    [Header("Can the platform be Jumped through?")]  //recommended setting to true for any horizontal platform
    [SerializeField] bool canBeJumpedThrough = false;
    PlatformEffector2D jumpThrough;

    int currentSegment = 0;
    GameObject movePosTemp;
    float moveTimeTemp;
    float waitTimeTemp;
    Vector3 pointA;
    Vector3 pointB;
    Vector3 pointTemp;


    float timeInMotion = 0f;
    float lerpTime = 0f;
    bool platformMoved = false;



    // Start is called before the first frame update
    void Start()
    {
        pointA = movePos[0].transform.position;
        pointB = movePos[1].transform.position;
        if (canBeJumpedThrough)
        {
            jumpThrough = GetComponent<PlatformEffector2D>();
            jumpThrough.enabled = true;
            // GetComponent<jumpThrough>().enabled = true;
        }


    }

    private void MovePlatform()
    {
        if ((timeInMotion < moveTime[currentSegment]) && (!platformMoved))
        {
            timeInMotion += (1.0f * Time.deltaTime);
            lerpTime += (1.0f * Time.deltaTime) / moveTime[currentSegment];
            gameObject.transform.position = Vector3.Lerp(pointA, pointB, lerpTime);

        }
        else if ((timeInMotion >= moveTime[currentSegment]) && (!platformMoved))
        {

            platformMoved = true;
            StartCoroutine(PlatformWait());
        }
    }

    private IEnumerator PlatformWait()
    {

        CheckPoints();
        yield return new WaitForSeconds(waitTime[currentSegment]);
        platformMoved = false;



    }

    private void CheckPoints()
    {
        if ((movePos.Length > currentSegment + 2) && (moveTime.Length > currentSegment + 2))
        {
            currentSegment++;
            pointA = pointB;
            pointB = movePos[currentSegment + 1].transform.position;
            timeInMotion = 0f;
            lerpTime = 0f;

        }
        else if ((movePos.Length <= currentSegment + 2) && (moveTime.Length <= currentSegment + 2) && (!returnTrip))
        {
            return;
        }
        else if ((movePos.Length <= currentSegment + 2) && (moveTime.Length <= currentSegment + 2) && (returnTrip))
        {
            ReversePoints();
        }


    }

    private void ReversePoints()  //reverse the timing and order of waypoints
    {
        for (int i = 0; i < movePos.Length / 2; i++)
        {
            print(i + movePos.Length);
            movePosTemp = movePos[i];
            movePos[i] = movePos[movePos.Length - i - 1];
            movePos[movePos.Length - i - 1] = movePosTemp;
            moveTimeTemp = moveTime[i];
            moveTime[i] = moveTime[movePos.Length - i - 1];
            moveTime[movePos.Length - i - 1] = moveTimeTemp;
            waitTimeTemp = waitTime[i];
            waitTime[i] = waitTime[movePos.Length - i - 1];
            waitTime[movePos.Length - i - 1] = waitTimeTemp;


        }
        pointA = movePos[0].transform.position;
        pointB = movePos[1].transform.position;
        currentSegment = 0;
        timeInMotion = 0f;
        lerpTime = 0f;


    }

    public void ActivateElevator()
    {

        elevatorIsActive = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            print("collided");
            collision.gameObject.transform.SetParent(transform);

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            print("called");
            collision.gameObject.transform.SetParent(null);

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!platformIsActiveObject)
        {
            MovePlatform();
        }
        else if ((platformIsActiveObject) && (elevatorIsActive))
        {
            MovePlatform();
        }
    }
}
