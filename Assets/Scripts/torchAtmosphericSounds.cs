using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class torchAtmosphericSounds : MonoBehaviour
{
    public AudioClip[] atmosphericSound;
    public GameObject[] visualFX;
    torchlight torch;
    bool soundCoolDown = false;
    [Range(0f, 1f)] public float defaultVolume;
    public float baseSoundCoolDownTimer = 10f;
    public float defaultFXVisualDuration = 4f; //default time for visual fx
    float FXVisualDuration;
    float soundCoolDownTimer;
    AudioSource audioSource;
    GameObject atmosphericFX;
    // Start is called before the first frame update
    void Start()
    {
        atmosphericFX = transform.Find("AtmosphericSounds").gameObject;
        torch = FindObjectOfType<torchlight>();
        audioSource = atmosphericFX.GetComponent<AudioSource>();
        soundCoolDownTimer = baseSoundCoolDownTimer;
        FXVisualDuration = defaultFXVisualDuration;
    }


    public void EnableAtmosphericSounds()
    {
        atmosphericFX.SetActive(true);
    }

    private Vector3 SoundOffset()
    {
        float radius = torch.GetRadius();
        float angle = Random.Range(0f, Mathf.PI * 2);
        float x = Mathf.Sin(angle) * radius;
        float y = Mathf.Cos(angle) * radius;
        Vector3 offset = new Vector3(x, y, 0f);
        return offset;
    }

    private void SpawnRandomVisualFX()
    {
        float randomValue = Random.value;
        int fXtoPlay = Mathf.RoundToInt(randomValue * (visualFX.Length - 1));

        try
        {
            Instantiate(visualFX[fXtoPlay], audioSource.transform);


        }
        catch
        {
            Debug.Log("Visual FX out of range");
        }
    }

    public float GetVisualFXDuration()
    {

        float FXVisualDuration = defaultFXVisualDuration * (1 - torch.GetRadius() / torch.GetMaxRadius());
        return FXVisualDuration;
    }

    private void PlayRandomSound()
    {
        float randomValue = Random.value;
        int soundToPlay = Mathf.RoundToInt(randomValue * (atmosphericSound.Length-1));
        float soundVolume = defaultVolume * torch.GetRadius() / torch.GetMaxRadius();
        try
        {
            audioSource.transform.position = gameObject.transform.position + SoundOffset();
            audioSource.PlayOneShot(atmosphericSound[soundToPlay], soundVolume);
            SpawnRandomVisualFX();
        }
        catch
        {
            Debug.Log("sound file out of range");
        }
    }

    private IEnumerator PlayRandomAtmosphericSound()
    {
        soundCoolDown = true;
        PlayRandomSound();
        yield return new WaitForSeconds(soundCoolDownTimer);
        float soundCoolDownRatio = (torch.GetRadius() / torch.GetMaxRadius());
        soundCoolDownTimer = baseSoundCoolDownTimer + baseSoundCoolDownTimer * soundCoolDownRatio;
        print("Sound Cooldown Timer is" + soundCoolDownTimer);
        soundCoolDown = false;
    }

    public void DisableAtmosphericSounds()
    {
        atmosphericFX.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if(!soundCoolDown)
        {
            StartCoroutine(PlayRandomAtmosphericSound());
        }
    }
}
