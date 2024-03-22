using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Session : MonoBehaviour
{
    private DateTime startTime;
    private string workspaceLabel;
    private float duration;

    public void SetData(SessionData data)
    {
        startTime = data.StartTime;
        workspaceLabel = data.WorkspaceLabel;
        duration = data.Duration;

        // Update the session display here
    }
    
}