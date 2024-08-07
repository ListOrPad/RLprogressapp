using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public static bool timerActive;

    [Header("Component")]
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("TimerSettings")]
    public static float currentTime;
    private string timerSound;
    [SerializeField] private bool countDown;

    [Header("LimitSettings")]
    [SerializeField] private bool hasLimit;
    [SerializeField] private float timerLimit;

    [Header("Start/Pause Settings")]
    [SerializeField] public Button pausePlayButton;
    [SerializeField] private Sprite pauseSprite;
    [SerializeField] private Sprite playSprite;
    [Header("Session Management")]
    [SerializeField] private SessionFinish sessionFinisher;
    private DateTime startSessionTime;
    private Workspace defaultWorkspace;
    [SerializeField] private WorkspaceManager workspaceManager;
    [SerializeField] private Settings settings;

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

    private void Start()
    {
        defaultWorkspace = workspaceManager.defaultWorkspace;
        defaultWorkspace.Initialize();
    }
    private void Update()
    {
        //setup previous timer sound and new timer sound(if changed in settings)
        string previousTimerSound = timerSound;
        timerSound = PlayerPrefs.GetString("TimerSound");
        //stop previous sound from playing, but don't if it's the same sound
        if (timerSound != previousTimerSound)
        {
            FindObjectOfType<SoundManager>().StopSound(previousTimerSound);
        }

        if (timerActive)
        {
            if (!soundHasRun)
            {
                FindObjectOfType<SoundManager>().PlaySound(timerSound);
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
        RefreshBinds();
        SetTimerText(); // thats the problem here, shouldn't be in update
        //changes pause sprite to play sprite and vice versa depending on if timer is active
        try
        {
            pausePlayButton.image.sprite = timerActive ? pauseSprite : playSprite;
        }
        catch
        {

        }
    }

    private void RefreshBinds()
    {
        
        try
        {
            timerText = GameObject.Find("Stopwatch").GetComponent<TextMeshProUGUI>(); // thats the problem here, shouldn't be in update
            sessionFinisher = GameObject.Find("SessionFinisher").GetComponent<SessionFinish>();
            settings = GameObject.Find("Settings").GetComponent<Settings>();
        }
        catch (NullReferenceException)
        {

        }
        GameObject discardSessionButton = sessionFinisher.discardSessionButton;
        GameObject saveSessionButton = sessionFinisher.saveSessionButton;
        if (discardSessionButton != null)
        {
            discardSessionButton.GetComponent<Button>().onClick.AddListener(delegate { PrepareDiscardSession(); });
            saveSessionButton.GetComponent<Button>().onClick.AddListener(delegate { PrepareSaveSession(); });
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
        if (currentTime == 0)
        {
            startSessionTime = DateTime.Now;
        }
        if(soundHasRun)
        {
            FindObjectOfType<SoundManager>().StopSound(timerSound);
            soundHasRun = false;
        }
        //reverses timerActive var
        timerActive = !timerActive;
    }
    public void StopTimer()
    {
        if (soundHasRun)
        {
            FindObjectOfType<SoundManager>().StopSound(timerSound);
            soundHasRun = false;
        }
        timerActive = false;
    }

    public void PrepareDiscardSession()
    {
        GameObject discardSessionMenu = sessionFinisher.finishSessionMenu;
        discardSessionMenu.SetActive(true);
        Button[] confirmButtons = discardSessionMenu.GetComponentsInChildren<Button>();
        confirmButtons[0].onClick.AddListener(delegate { DiscardSession(true); });
        confirmButtons[1].onClick.AddListener(delegate { DiscardSession(false); });
    }
    public void DiscardSession(bool discard)
    {
        if (discard)
        {
            PlayerPrefs.DeleteKey("CurrentTime");
            currentTime = 0;
            StopTimer();
        }
    }

    public void PrepareSaveSession()
    {
        GameObject saveSessionMenu = sessionFinisher.finishSessionMenu;
        saveSessionMenu.SetActive(true);
        saveSessionMenu.GetComponentInChildren<TextMeshProUGUI>().text = "Sure want to finish by Saving?";
        Button[] confirmButtons = saveSessionMenu.GetComponentsInChildren<Button>();

        // Remove all listeners(needed since it's connected to update method)
        confirmButtons[0].onClick.RemoveAllListeners();
        confirmButtons[1].onClick.RemoveAllListeners();

        // Add new listeners
        confirmButtons[0].onClick.AddListener(delegate { SaveSession(true); });
        confirmButtons[1].onClick.AddListener(delegate { SaveSession(false); });
    }
    public void SaveSession(bool save)
    {
        if(save)
        {
            SessionData session = new SessionData(startSessionTime, settings.GetWorkspace().name, currentTime);
            settings.GetWorkspace().AddSession(session);

            //finishing touches
            PlayerPrefs.DeleteKey("CurrentTime");
            currentTime = 0;
            StopTimer();
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause && timerActive)
        {
            PlayerPrefs.SetString("TimeOfPause", DateTime.Now.ToBinary().ToString());
        }
        else
        {
            var timeQuitApp = DateTime.FromBinary(long.Parse(PlayerPrefs.GetString("TimeOfPause")));
            var timeEnterApp = DateTime.Now;
            currentTime += (float)(timeEnterApp - timeQuitApp).TotalSeconds;
        }
    }
}
