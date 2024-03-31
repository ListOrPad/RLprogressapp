using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SessionData
{
    public string StartTime { get; set; }
    public string WorkspaceLabel { get; set; }
    public float Duration { get; set; }

    public SessionData() { }

    public SessionData(DateTime startTime, string workspaceLabel, float duration)
    {
        StartTime = startTime.ToString("O");
        WorkspaceLabel = workspaceLabel;
        Duration = duration;
    }

    public DateTime GetStartTime()
    {
        return DateTime.Parse(StartTime, null, System.Globalization.DateTimeStyles.RoundtripKind);
    }
}