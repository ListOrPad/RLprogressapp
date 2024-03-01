using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public static bool timerActive;

    [Header("Component")]
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("TimerSettings")]
    public static float currentTime;  //make static?
    [SerializeField] private bool countDown;

    [Header("LimitSettings")]
    [SerializeField] private bool hasLimit;
    [SerializeField] private float timerLimit;

    [Header("Start/Pause Settings")]
    [SerializeField] public Button pausePlayButton;
    [SerializeField] private Sprite pauseSprite;
    [SerializeField] private Sprite playSprite;

    private bool soundHasRun = false;
    public bool SoundHasRun { get { return soundHasRun; } set { soundHasRun = value; } }

    private void Awake()
    {
        currentTime = PlayerPrefs.GetFloat("CurrentTime");
        GameObject[] timerObj = GameObject.FindGameObjectsWithTag("Timer");
        if (timerObj.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        if (timerActive)
        {
            if (!soundHasRun)
            {
                FindObjectOfType<SoundManager>().PlaySound("Ticking");
                soundHasRun = true;
            }
            currentTime = countDown ? currentTime -= Time.deltaTime : currentTime += Time.deltaTime;

            //Save time across the scenes
            PlayerPrefs.SetFloat("CurrentTime", currentTime);

            if (hasLimit && (countDown && currentTime <= timerLimit))
            {
                currentTime = timerLimit;
            }
        }
        SetTimerText();
    }

    
    public void SetTimerText()
    {
        try
        {
            timerText = GameObject.Find("Stopwatch").GetComponent<TextMeshProUGUI>();
        }
        catch (NullReferenceException)
        {

        }
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        if (currentTime >= 3600)
            timerText.text = time.Hours.ToString() + ":" + time.Minutes.ToString("00") + ":" + time.Seconds.ToString("00");
        else
            timerText.text = time.Minutes.ToString("00") + ":" + time.Seconds.ToString("00");
    }

    public void StartPauseTimer()
    {
        if(soundHasRun)
        {
            FindObjectOfType<SoundManager>().StopSound("Ticking");
            soundHasRun = false;
        }
        timerActive = !timerActive;
        pausePlayButton.image.sprite = timerActive ? pauseSprite : playSprite;
    }
    public void StopTimer()
    {
        if (soundHasRun)
        {
            FindObjectOfType<SoundManager>().StopSound("Ticking");
            soundHasRun = false;
        }
        timerActive = false;
        //here should be displayed menu of what to do with the session etc.

        if (DiscardSession() || SaveSession())
        {
            timerText.text = string.Empty;
            PlayerPrefs.DeleteKey("CurrentTime");
        }
    }
    public bool DiscardSession()
    {
        //write: Are you sure you want to discard?
        bool discard = false; //could be string
        if (discard)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool SaveSession()
    {
        return false;
    }
}
