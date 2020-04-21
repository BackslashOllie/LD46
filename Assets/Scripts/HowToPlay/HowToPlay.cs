using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class HowToPlay : MonoBehaviour
{
    public string[] instructions;
    public Transform[] tutorials;
    public Transform cameraParent;
    public TextMeshProUGUI instructionText;
    public Text NextButtonText;


    private int index = 0;
    // Start is called before the first frame update
    void Start()
    {
        SetTutorial(index);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            OnNextButtonClick();
        }
    }

    public void OnNextButtonClick()
    {
        index++;
        if (index == tutorials.Length)
        {
            SceneManager.LoadScene("MainMenu");
            return;
        }
        else if (index == tutorials.Length - 1)
            NextButtonText.text = "Exit";

        SetTutorial(index);
    }

    void SetTutorial(int _index)
    {
        cameraParent.position = tutorials[_index].position;
        instructionText.text = instructions[_index];
    }
}
