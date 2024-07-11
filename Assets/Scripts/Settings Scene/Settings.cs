using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void Start()
    {
        SetTimerSoundDropdown();
        //workspaceDropdown.value = workspaceManager.WorkspacesNames.IndexOf(GetWorkspace().Name);
    }
    private void SetTimerSoundDropdown()
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

    private void Update()
    {
        try
        {
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
            SetTimerSoundDropdown();
            workspaceManager = GameObject.Find("WorkspaceManager").GetComponent<WorkspaceManager>();
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
            currentWorkspace = workspaceManager.workspaces.Find(w => w.Name == PlayerPrefs.GetString("CurrentWorkspace")); 
            return currentWorkspace;
        }
        catch
        {
            return currentWorkspace;
        }
    }
}
