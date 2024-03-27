using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Session : MonoBehaviour
{
    private DateTime startTime;
    private string workspaceLabel;
    private float duration;

    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI startTimeText;
    [SerializeField] private TextMeshProUGUI workspaceText;

    public void SetData(SessionData data)
    {
        startTime = data.StartTime;
        workspaceLabel = data.WorkspaceLabel;
        duration = data.Duration;

        // Convert the duration from seconds to minutes and round up
        int durationInMinutes = Mathf.CeilToInt(data.Duration / 60f);
        // Update the session display here
        timeText.text = durationInMinutes.ToString();
        startTimeText.text = data.StartTime.ToString("HH:mm");
        workspaceText.text = data.WorkspaceLabel;
    }
    
}