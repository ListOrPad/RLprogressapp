using System;
using TMPro;
using UnityEngine;
public class SessionManager : MonoBehaviour
{
    public GameObject dayPrefab;
    public GameObject sessionPrefab;
    public GameObject content;
    [SerializeField] private Settings settings;

    public Day today;
    private DateTime currentDate;

    private void Awake()
    {
        currentDate = DateTime.Today;
    }
    private void Start()
    {
        settings = GameObject.Find("Settings").GetComponent<Settings>();
        if (settings.GetWorkspace().sessions != null)
        {
            foreach (SessionData sessionData in settings.GetWorkspace().sessions)
            {
                AddSession(sessionData);
            }
        }
    }

    private void Update()
    {
        if (currentDate < DateTime.Today)
        {
            currentDate = DateTime.Today;
        }
    }

    private void CreateNewDay(DateTime date)
    {
        GameObject newDayObject = Instantiate(dayPrefab, content.transform);
        today = newDayObject.GetComponent<Day>();
        today.SetDate(date); // Pass the date when creating a new day
    }
    public void AddSession(SessionData data)
    {
        // Check if a new day needs to be created
        if (today == null || today.GetDate().Date != data.GetStartTime().Date)
        {
            CreateNewDay(data.GetStartTime().Date);
        }

        // Add the session to the current day
        today.AddSession(data);
    }
}