using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using UnityEditor;
using TMPro;

public class SessionHistory : MonoBehaviour
{
    public ViewPeriod CurrentViewPeriod { get; set; } = ViewPeriod.ThisWeek;

    [Header("Session Management")]
    [SerializeField] private GameObject sessionPrefab;
    [SerializeField] private GameObject dayPrefab;
    [SerializeField] private GameObject content;

    [Header("Periods")]
    [SerializeField] private TMP_Dropdown periodDropdown;

    public static List<SessionData> Sessions { get; } = new List<SessionData>();
    private List<GameObject> SessionObjects { get; } = new List<GameObject>();

    private string filePath;
    private Day today;
    //is this good enough?
    private Day yesterday;
    private Day onThisWeek;

    private void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, "sessions.json");
        SaveSessions();
        LoadSessions();
    }
    private void Start()
    {
        
        CreateNewDay();
        today.SetDay(DateTime.Today);

        DisplaySessions();
    }
    private void Update()
    {
        ChangeViewPeriod(periodDropdown.options[periodDropdown.value].text);
    }

    private void ChangeViewPeriod(string period)
    {
        switch (period)
        {
            case "Today":
                CurrentViewPeriod = ViewPeriod.Today; break;
            case "Yesterday":
                CurrentViewPeriod = ViewPeriod.Yesterday; break;
            case "This Week":
                CurrentViewPeriod = ViewPeriod.ThisWeek; break;
            case "Previous Week":
                CurrentViewPeriod = ViewPeriod.PreviousWeek; break;
            case "This Month":
                CurrentViewPeriod = ViewPeriod.ThisMonth; break;
            case "Previous Month":
                CurrentViewPeriod = ViewPeriod.PreviousMonth; break;
            case "Lifetime":
                CurrentViewPeriod = ViewPeriod.Lifetime; break;
        }
    }

    public void DisplaySessions()
    {
        if (CurrentViewPeriod == ViewPeriod.Today)
        {
            List<SessionData> sessionsToDisplay = GetSessionsInChronologicalOrder();
            foreach (SessionData sessionData in sessionsToDisplay)
            {
                AddSession(sessionData, today);
            }
        }
    }

    /// <summary>
    /// Should add one simple record of a Session *Under corresponding Day prefab*?
    /// </summary>
    public void AddSession(SessionData data, Day day)
    {
        GameObject newSession = Instantiate(sessionPrefab, day.transform);
        newSession.GetComponent<Session>().SetSessionText(data);

        SessionObjects.Add(newSession); //needed?
        SaveSessions();
    }

    public Day CreateNewDay() //remember, this returns day, but this isn't in use yet
    {
        if (today == null)
        {
            GameObject newDayObject = Instantiate(dayPrefab, content.transform);
            today = newDayObject.GetComponent<Day>();
            return today;
        }
        return today; //???????
    }

    private void SaveSessions()
    {
        if (Sessions.Count > 0)
        {
            string json = JsonConvert.SerializeObject(new SerializationWrapper<SessionData> { Items = Sessions });
            File.WriteAllText(filePath, json);
            Debug.Log("Saved sessions: " + json); // Log the saved sessions
        }
        else
        {
            Debug.Log("No sessions to save."); // Log if there are no sessions to save
        }
    }

    private void LoadSessions()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            Debug.Log("JSON from file: " + json); // Log the contents of the JSON file

            SerializationWrapper<SessionData> data = JsonConvert.DeserializeObject<SerializationWrapper<SessionData>>(json);

            if (data != null && data.Items != null)
            {
                Sessions.Clear();
                Sessions.AddRange(data.Items);
                Debug.Log("Loaded sessions: " + json); // Log the loaded sessions
            }
            else
            {
                Debug.Log("Data from JsonUtility.FromJson: " + data); // Log the result of the JsonUtility.FromJson call
            }
        }
    }

    private List<SessionData> GetSessionsInChronologicalOrder()
    {
        if (Sessions.Count > 0 && !IsNewestSessionFirst())
        {
            List<SessionData> orderedSessions = Sessions;
            orderedSessions.Reverse();
            return orderedSessions;
        }

        return Sessions;
    }

    /// <summary>
    /// A check to determine if the first session is the newest
    /// </summary>
    private bool IsNewestSessionFirst()
    {
        if(Sessions.Count < 2) return false;

        return Sessions[0].TimeCreated > Sessions[1].TimeCreated;
    }






}

[System.Serializable]
public class SerializationWrapper<T>
{
    public List<T> Items;
}