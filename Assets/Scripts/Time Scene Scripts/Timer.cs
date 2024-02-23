using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public bool timerActive = true;

    [Header("Component")]
    public TextMeshProUGUI timerText;

    [Header("TimerSettings")]
    public float currentTime;
    public bool countDown;

    [Header("LimitSettings")]
    public bool hasLimit;
    public float timerLimit;

    [Header("Start/Pause Settings")]
    public Button button;
    public Sprite pauseSprite;
    public Sprite playSprite;

    private bool soundHasRun = false;

    // Update is called once per frame
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

            if (hasLimit && (countDown && currentTime <= timerLimit))
            {
                currentTime = timerLimit;
            }
        }
        SetTimerText();
    }

    public void SetTimerText()
    {
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
        button.image.sprite = timerActive ? pauseSprite : playSprite;
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
    }
}
