using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternGuide : MonoBehaviour
{
    public static LanternGuide Instance;
    public Lantern[] lantern; //stores the path of lanterns in sequence for the player
    private int current_Lantern = 0; //first lantern
    public Lantern CurrentLantern
    {
        get { return lantern[current_Lantern]; }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Incorrect number of Lantern Guides");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        lantern[current_Lantern].ToggleLightsOn();
        for (int lanternNumber = 1; lanternNumber < lantern.Length; lanternNumber++)
        {
            lantern[lanternNumber].ToggleLightsOff();
        }
    }

    public void NextLantern() // toggle the current lantern off and the next one on.
    {
        Debug.Log("current Lantern is " + current_Lantern);
        lantern[current_Lantern].ToggleLightsOff();
        print("Turning off " + lantern[current_Lantern]);
        current_Lantern++;
        if (current_Lantern > lantern.Length - 1)
        {
            Debug.Log("No more lanterns");
        }
        else
        {
            lantern[current_Lantern].ToggleLightsOn();
            print("Turning on " + lantern[current_Lantern]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
