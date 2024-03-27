using System;
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
    }
    private void Start()
    {
        if (SessionDataHolder.Sessions != null)
        {
            foreach (SessionData sessionData in SessionDataHolder.Sessions)
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
        if (today == null || today.GetDate().Date != data.StartTime.Date)
        {
            CreateNewDay(data.StartTime.Date);
        }

        // Add the session to the current day
        today.AddSession(data);
    }
}