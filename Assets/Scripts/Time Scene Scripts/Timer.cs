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
    [SerializeField] private Button pausePlayButton;
    [SerializeField] private Sprite pauseSprite;
    [SerializeField] private Sprite playSprite;

    private bool soundHasRun = false;

    private void Awake()
    {
        currentTime = PlayerPrefs.GetFloat("CurrentTime");
        GameObject[] timerObj = GameObject.FindGameObjectsWithTag("Sound");
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
        RefreshBinds();
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

        //if(Discard Session or Save session)
        //{
        //    PlayerPrefs.DeleteKey("CurrentTime");
        //}
    }
    private void RefreshBinds()
    {
        try
        {
            timerText = GameObject.Find("Stopwatch").GetComponent<TextMeshProUGUI>();
            pausePlayButton = GameObject.Find("Pause/Play Button").GetComponent<Button>();
            //If has no onClick events then add startPauseTimer event to button
            if (!HasOnClickListeners(pausePlayButton))
            {
                pausePlayButton.onClick.AddListener(delegate { StartPauseTimer(); });
            }
        }
        catch (NullReferenceException)
        {

        }
    }
    private bool HasOnClickListeners(Button button)
    {
        if (button != null)
        {
            // Get the UnityEvent associated with the button's OnClick event
            UnityEngine.Events.UnityEvent onClickEvent = button.onClick;

            // Check if the event has any listeners
            Debug.Log("True");
            return onClickEvent.GetPersistentEventCount() > 0;
        }

        Debug.Log("False");
        return false;
    }
}
