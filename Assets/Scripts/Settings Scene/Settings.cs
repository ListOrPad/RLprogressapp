using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown timerSoundDropdown;
    [SerializeField] private TMP_Dropdown currentWorkspaceDropdown;
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
        if (timerSoundDropdown == null || currentWorkspaceDropdown == null)
        {
            timerSoundDropdown = GameObject.Find("Timer Sound Dropdown").GetComponentInChildren<TMP_Dropdown>();
            currentWorkspaceDropdown = GameObject.Find("Workspace Dropdown").GetComponentInChildren<TMP_Dropdown>();
        }
    }
    // Update is called once per frame
    void Update()
    {
        try
        {
            RefreshBinds();
            SetTimerSound();
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
    public Workspace GetWorkspace()
    {
        //there maybe later must be a variable for 0 float value so that it hold storage value and not nullify it all the time
        try
        {
            string currentWorkspaceName = currentWorkspaceDropdown.options[currentWorkspaceDropdown.value].text;
            PlayerPrefs.SetString("CurrentWorkspace", currentWorkspaceName);
            //this is wrong, sessions should be created only in workspace manager... or not?
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
