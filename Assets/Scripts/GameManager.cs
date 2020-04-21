using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float GameTime { get { return Time.time - gameStartTime; } }

    AudioSource musicPlayer;

    public string GameTimeString { 
        get {

            var ts = TimeSpan.FromSeconds(GameTime);
            return string.Format("{0:00}:{1:00}", ts.TotalMinutes, ts.Seconds);
        } 
    }

    public float gameStartTime;


    void Awake()
    {
        DontDestroyOnLoad(this);
        if (Instance == null)
            Instance = this;
        else Destroy(gameObject);
        
    }

    private void Start()
    {
        musicPlayer = GetComponent<AudioSource>();

    }

    public void ToggleMusicPlayer(bool On)
    {
        if (On)
        {
            musicPlayer.Play();

        }
        else musicPlayer.Stop();
    }

    public void Update()
    {

    }

    public void StartGame()
    {
        gameStartTime = Time.time;
    }
}
