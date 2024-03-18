using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown timerSoundDropdown; 
    // Start is called before the first frame update
    void Start()
    {
        string savedTimerSound = PlayerPrefs.GetString("TimerSound", "Default Value");
        int index = timerSoundDropdown.options.FindIndex(option => option.text == savedTimerSound);
        if (index != -1)
        {
            timerSoundDropdown.value = index;
        }
    }

    // Update is called once per frame
    void Update()
    {
        SetTimerSound();
    }
    private void SetTimerSound()
    {
        string timerSound = timerSoundDropdown.options[timerSoundDropdown.value].text;
        PlayerPrefs.SetString("TimerSound", timerSound);
    }
}
