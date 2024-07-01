using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown timerSoundDropdown;
    [SerializeField] private TMP_Dropdown workspaceDropdown;
    private WorkspaceManager workspaceManager;
    private Workspace currentWorkspace;

    private void Awake()
    {
        GameObject[] settingsObj = GameObject.FindGameObjectsWithTag("Settings");
        if (settingsObj.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        string savedTimerSound = PlayerPrefs.GetString("TimerSound", "Default Value");
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

    void Update()
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
        //there maybe later must be a variable for 0 float value so that it hold storage value and not nullify it all the time
        try
        {
            
            //it always resets bank to 0?
            currentWorkspace = new Workspace(PlayerPrefs.GetString("CurrentWorkspace"), 0); 
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
