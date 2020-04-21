using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitMusicPlayer : MonoBehaviour
{
    AudioSource musicPlayer;
    AudioSource exitMusicPlayer;
    // Start is called before the first frame update
    void Start()
    {
        exitMusicPlayer = GetComponent<AudioSource>();
        musicPlayer = FindObjectOfType<GameManager>().GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider player)
    {
        if (player.tag == "Player")
        {
            exitMusicPlayer.Play();
            musicPlayer.Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
