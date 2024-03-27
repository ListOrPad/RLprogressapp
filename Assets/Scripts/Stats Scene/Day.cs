using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class Day : MonoBehaviour
{
    [SerializeField] private GameObject sessionPrefab;
    [SerializeField] private TextMeshProUGUI dateText;
    private DateTime date;
    //[SerializeField] private SessionManager sessionManager;
    private List<GameObject> sessions = new List<GameObject>();

    public void AddSession(SessionData data)
    {
        // Instantiate a new session object
        GameObject newSession = Instantiate(sessionPrefab, transform);

        // Get the Session component and set the data
        Session session = newSession.GetComponent<Session>();
        session.SetData(data);

        sessions.Add(newSession);
    }

    private void Start()
    {
        // Set the date label
        dateText.text = date.ToString("dd MMM yy", CultureInfo.InvariantCulture); // Format the DateTime as "day month year"
    }

    public void SetDate(DateTime date)
    {
        this.date = date;
    }

    public DateTime GetDate()
    {
        return date;
    }
}
