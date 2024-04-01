using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SessionData
{
    public string startTime;
    public string workspace { get; set; }
    public float duration { get; set; }

    public SessionData() { }

    public SessionData(DateTime startTime, string workspace, float duration)
    {
        this.startTime = startTime.ToString("O");
        this.workspace = workspace;
        this.duration = duration;
    }

    public DateTime GetStartTime()
    {
        if (string.IsNullOrEmpty(startTime))
        {
            return DateTime.MinValue;
        }
        return DateTime.Parse(startTime, null, System.Globalization.DateTimeStyles.RoundtripKind);
    }
}