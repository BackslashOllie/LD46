using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TextMeshProUGUI flashText;
    torchlight torch;
    GameObject fillTorchMessage;
    GameObject activateMonolithMessage;
    GameObject dragonWarning;
    GameObject pausePanel;
    GameObject dragonWarningText;

    [Header("Object and Class References")]
    public AudioClip UIBeep;
    public Image flameBar;
    public Image flameBarBG;
    public Image dragonIcon;
    public Image torchIcon;


    [Header("UI settings")]
    [Range(0f, 1f)]
    [Tooltip("Audiobeep volume for low health and warnings")]
    public float beepVolume = .5f;
    public float levelStartTextDisplayTime = 1f;
    public float dragonWarningDisplayTime = 1f; //time to display warning text
    public float dragonWarningDisplayCoolDownTime = 3f; // interval before displaying above warning again
    public float iconFlashTimer; //interval AND cooldown for icon flashes

    bool dragonWarningCoolDown = false;
    float flashRedTimer = .25f;
    float redCoolDownTimer = 2f;
    bool bgColorFlashOn = false;
    bool redCoolDown = false;
    float maxFuel;
    bool gamePaused = false;
    bool torchIconFlashCooldown = false;
    bool dragonIconFlashCooldown = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        fillTorchMessage = transform.Find("TorchFill").gameObject;
        torch = FindObjectOfType<torchlight>();
        maxFuel = torch.GetMaxTorchLife();
        dragonWarning = transform.Find("DragonWarning").gameObject;
        pausePanel = transform.Find("Pause Menu").gameObject;
        FlashText("PROTECT BOBB THE DRAGON AND ESCAPE", 2f);
        activateMonolithMessage = transform.Find("ActivateMonolith").gameObject;
        dragonWarningText = transform.Find("DragonUnderAttack").gameObject;
    }

    public void EnableFillTorchMessage()
    {
        fillTorchMessage.SetActive(true);
    }

    public void EnableActivateMonolithMessage()
    {
        activateMonolithMessage.SetActive(true);
    }

    public bool GamePaused()
    {
        if (gamePaused)
        {
            return true;
        }
        else return false;

    }

    private void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0;
        gamePaused = true;
    }

    public void UnPauseGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
        gamePaused = false;
    }

    public void ShowDragonWarning()
    {
        if (!dragonWarningCoolDown)
        {
            dragonWarningCoolDown = true;
            dragonWarning.SetActive(true);
            StartCoroutine(DragonWarningDisplayTimer());
        }

    }

    private IEnumerator TorchIconFlash()
    {
        torchIconFlashCooldown = true;
        torchIcon.color = Color.red;
        yield return new WaitForSeconds(iconFlashTimer);
        torchIcon.color = Color.white;
        StartCoroutine(CoolDownFlashes("Torch"));

    }

    private IEnumerator DragonIconFlash()
    {
        dragonWarningText.SetActive(true);
        dragonIconFlashCooldown = true;
        dragonIcon.color = Color.red;
        yield return new WaitForSeconds(iconFlashTimer);
        dragonWarningText.SetActive(false);
        dragonIcon.color = Color.white;
        StartCoroutine(CoolDownFlashes("Dragon"));
    }

    private IEnumerator CoolDownFlashes(string coolDown)
    {
        yield return new WaitForSeconds(iconFlashTimer);
        if (coolDown == "Dragon")
        {
            dragonIconFlashCooldown = false;
        }
        else
        {
            torchIconFlashCooldown = false;
        }
    }

    public void FlashText(string text, float time)
    {
        StopCoroutine("EndTextDisplay");
        flashText.text = text;
        flashText.gameObject.SetActive(true);
        StartCoroutine("EndTextDisplay", time);
    }

    private IEnumerator EndTextDisplay(float time)
    {
        yield return new WaitForSeconds(time);
        flashText.gameObject.SetActive(false);
    }

    public void FlashTorchIcon()
    {
        if (!torchIconFlashCooldown)
        {
            StartCoroutine(TorchIconFlash());
        }
    }

    public void FlashDragonIcon()
    {
        if (!dragonIconFlashCooldown)
        {
            StartCoroutine(DragonIconFlash());
        }
    }


    private IEnumerator DragonWarningDisplayTimer()
    {

        yield return new WaitForSeconds(dragonWarningDisplayTime);
        dragonWarning.SetActive(false);
        StartCoroutine(DragonWarningCoolDown());
    }

    private IEnumerator DragonWarningCoolDown()
    {
        yield return new WaitForSeconds(dragonWarningDisplayCoolDownTime);
        dragonWarningCoolDown = false;
    }


    public void DisableFillTorchMessage()
    {
        fillTorchMessage.SetActive(false);
    }

    public void DisableActivateMonolithMessage()
    {
        activateMonolithMessage.SetActive(false);
    }

    private void UpdateFlameBar()
    {
        float fillPercentage = torch.GetTorchLife() / maxFuel;
        flameBar.fillAmount = fillPercentage;
        if (fillPercentage <= .25)
        {
            bgColorFlashOn = true;
        }
        else bgColorFlashOn = false;
    }

    private IEnumerator FlashBGBar()
    {
        redCoolDown = true;
        flameBarBG.color = Color.red;
        AudioSource.PlayClipAtPoint(UIBeep, torch.transform.position, beepVolume);
        yield return new WaitForSeconds(flashRedTimer);
        flameBarBG.color = Color.black;
        StartCoroutine(RedCoolDown());

    }

    private IEnumerator RedCoolDown()
    {
        
        yield return new WaitForSeconds(redCoolDownTimer);
        redCoolDown = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFlameBar();

        if ((bgColorFlashOn) && (!redCoolDown))
        {
            StartCoroutine(FlashBGBar());
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused)
            {
                UnPauseGame();
            }
            else PauseGame();
        }
    }
}
