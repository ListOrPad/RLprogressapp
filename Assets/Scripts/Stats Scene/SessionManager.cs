using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionManager : MonoBehaviour
{
    public GameObject dayPrefab;
    public GameObject sessionPrefab;
    public GameObject content;

    public Day today;
    private DateTime currentDate;

    private void Awake()
    {
        currentDate = DateTime.Today;
        CreateNewDay();
    }
    private void Start()
    {
        if (SessionDataHolder.Sessions != null)
        {
            foreach (SessionData sessionData in SessionDataHolder.Sessions)
            {
                // Instantiate a new session object
                GameObject newSessionObject = Instantiate(sessionPrefab, today.transform);

                // Get the Session component and set the data
                Session session = newSessionObject.GetComponent<Session>();
                session.SetData(sessionData);
            }
        }
    }

    private void Update()
    {
        if(currentDate < DateTime.Today)
        {
            currentDate = DateTime.Today;
            CreateNewDay();
        }
    }

    private void CreateNewDay()
    {
        GameObject newDayObject = Instantiate(dayPrefab, content.transform);
        today = newDayObject.GetComponent<Day>();
    }
}
