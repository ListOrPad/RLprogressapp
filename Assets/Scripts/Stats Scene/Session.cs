using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Session : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI startTimeText;
    [SerializeField] private TextMeshProUGUI workspaceText;


    public void SetSessionText(SessionData data)
    {
        // Convert the duration from seconds to minutes and round up
        int durationInMinutes = Mathf.CeilToInt(data.Duration / 60f);
        // Update the session display here
        timeText.text = durationInMinutes.ToString();
        startTimeText.text = data.GetStartTime().ToString("HH:mm");
        workspaceText.text = data.WorkspaceName;
    }

    
}