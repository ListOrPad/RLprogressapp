using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionData
{
    public DateTime StartTime { get; set; }
    public string WorkspaceLabel { get; set; }
    public float Duration { get; set; }

    public SessionData(DateTime startTime, string workspaceLabel, float duration)
    {
        StartTime = startTime;
        WorkspaceLabel = workspaceLabel;
        Duration = duration;
    }
}