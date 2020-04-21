using UnityEngine;
using TMPro;

public class EndScene : MonoBehaviour
{
    public TextMeshProUGUI endText;

    void Start()
    {
        if (GameManager.Instance) endText.text += " in " + GameManager.Instance.GameTimeString;
    }
}
