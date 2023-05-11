using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Sprites;
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timerActive)
        {
            currentTime = countDown ? currentTime -= Time.deltaTime : currentTime += Time.deltaTime;

            if (hasLimit && (countDown && currentTime <= timerLimit))
            {
                currentTime = timerLimit;
            }
            SetTimerText(); 
        }
        else
        {
            SetTimerText();
        }
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
        if (timerActive == false)
        {
            timerActive = true;
            button.image.sprite = pauseSprite; 
        }
        else
        {
            timerActive = false;
            button.image.sprite = playSprite;
        }
    }
    public void StopTimer()
    {
        timerActive = false;
        //here should be displayed menu of what to do with the session etc.
    }
}
