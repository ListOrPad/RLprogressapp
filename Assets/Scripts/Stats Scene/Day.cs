using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class Day : MonoBehaviour
{
    [SerializeField] private GameObject sessionPrefab;
    [SerializeField] private TextMeshProUGUI dayText;
    private DateTime day;
    private List<GameObject> sessions = new List<GameObject>();

    public void AddSession(SessionData data)
    {
        // Instantiate a new session object
        GameObject newSession = Instantiate(sessionPrefab, transform);

        // Get the Session component and set the data
        newSession.GetComponent<Session>().SetSessionText(data);

        sessions.Add(newSession);
    }

    private void Start()
    {
        // Set the date label
        dayText.text = day.ToString("dd MMM yy", CultureInfo.InvariantCulture); // Format the DateTime as "day month year"
    }

    public void SetDay(DateTime day)
    {
        this.day = day;
    }

    public DateTime GetDay()
    {
        return day;
    }
}
