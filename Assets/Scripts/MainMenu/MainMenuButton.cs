using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{

    public TextMeshPro textObj;
    public string levelToLoad;
    public bool exitGame;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        if (exitGame) Application.Quit();
        else SceneManager.LoadScene(levelToLoad); 
    }

    public void OnMouseOver()
    {
        if (textObj)
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnClick();
            }
            textObj.color = Color.red;
        }
    }

    public void OnMouseExit()
    {
        if (textObj) textObj.color = Color.white;
    }
}
