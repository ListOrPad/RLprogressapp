using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SessionData
{
    public string StartTime { get; set; }
    public string WorkspaceName { get; set; }
    public float Duration { get; set; }
    public SessionData() { }

    public SessionData(DateTime startTime, string workspaceName, float duration)
    {
        StartTime = startTime.ToString("O");
        WorkspaceName = workspaceName;
        Duration = duration;
    }

    public DateTime GetStartTime()
    {
        if (string.IsNullOrEmpty(StartTime))
        {
            return DateTime.MinValue;
        }
        return DateTime.Parse(StartTime, null, System.Globalization.DateTimeStyles.RoundtripKind);
    }
}