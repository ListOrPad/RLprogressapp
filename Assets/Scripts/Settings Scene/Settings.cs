using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown timerSoundDropdown;
    [SerializeField] private TMP_Dropdown workspaceDropdown;
    private Workspace currentWorkspace;

    private void Awake()
    {
        GameObject[] settingsObj = GameObject.FindGameObjectsWithTag("Settings");
        if (settingsObj.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void Start()
    {
        StartSettings();
    }
    private void StartSettings()
    {
        Workspace.instance = new Workspace(PlayerPrefs.GetString("CurrentWorkspace"), 0); //storage Value later should be variable

        //Set timer sound dropdown
        string savedTimerSound = PlayerPrefs.GetString("TimerSound", "Default Value");
        Debug.Log("123");
        int index = timerSoundDropdown.options.FindIndex(option => option.text == savedTimerSound);
        if (index != -1)
        {
            timerSoundDropdown.value = index;
        }
    }
    private void RefreshBinds()
    {
        if (timerSoundDropdown == null || workspaceDropdown == null)
        {
            timerSoundDropdown = GameObject.Find("Timer Sound Dropdown").GetComponentInChildren<TMP_Dropdown>();
            workspaceDropdown = GameObject.Find("Workspace Dropdown").GetComponentInChildren<TMP_Dropdown>();
        }
    }

    private void Update()
    {
        try
        {
            RefreshBinds();
            SetTimerSound();
            SetCurrentWorkspaceName();
        }
        catch
        {

        }
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Settings Scene")
        {
            RefreshBinds();
            StartSettings();
        }
            
    }
    private void SetTimerSound()

    {
        string timerSound = timerSoundDropdown.options[timerSoundDropdown.value].text;
        PlayerPrefs.SetString("TimerSound", timerSound);
    }
    private void SetCurrentWorkspaceName()
    {
        string currentWorkspaceName = workspaceDropdown.options[workspaceDropdown.value].text;
        PlayerPrefs.SetString("CurrentWorkspace", currentWorkspaceName);
    }
    public Workspace GetWorkspace()
    {
        try
        {
            //it always resets bank to 0? sounds like a problem
            currentWorkspace = Workspace.instance; 
            //currentWorkspace.Initialize();
            return currentWorkspace;
        }
        catch
        {
            //    currentWorkspace = new Workspace("Default Workspace", 0);
            //    currentWorkspace.Initialize();
            return currentWorkspace;
        }
    }
}
