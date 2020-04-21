using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioFX : MonoBehaviour
{
    [Header("Audio Files")]
    public AudioClip footStep;
    public AudioClip jumpSound;

    [Header("FootStep Settings")]
    public float minStepVolume = .2f;
    public float maxStepVolume = .5f;

    public float minLoudStepVolume = .6f;
    public float maxLoudStepVolume = 1f;

    [Header("Jump Settings")]
    public float minJumpVolume = .4f;
    public float maxJumpVolume = .6f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Step()
    {
        float footStepVolume = Random.Range(minStepVolume, maxStepVolume);
        AudioSource.PlayClipAtPoint(footStep, gameObject.transform.position, footStepVolume);
    }

    public void LoudStep()
    {
        float footStepVolume = Random.Range(minLoudStepVolume, maxLoudStepVolume);
        AudioSource.PlayClipAtPoint(footStep, gameObject.transform.position, footStepVolume);
    }

    public void Jump()
    {
        float jumpVolume = Random.Range(minJumpVolume, maxJumpVolume);
        AudioSource.PlayClipAtPoint(jumpSound, gameObject.transform.position, jumpVolume);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
