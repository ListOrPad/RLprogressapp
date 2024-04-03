using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SessionData
{
    public string startTime;
    public string workspaceName;
    public float duration;
    public SessionData() { }

    public SessionData(DateTime startTime, string workspaceName, float duration)
    {
        this.startTime = startTime.ToString("O");
        this.workspaceName = workspaceName;
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