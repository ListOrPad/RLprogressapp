using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatsHistoryView : MonoBehaviour
{
    public Day dayPrefab;
    private List<Day> days = new List<Day>();

    public void ChangeViewPeriod(ViewPeriod period)
    {
        // Clear the current view
        foreach (Day day in days)
        {
            Destroy(day.gameObject);
        }
        days.Clear();

        // Get the sessions for the selected view period
        List<SessionData> sessions = GetSessionsForPeriod(period);

        // Group the sessions by day
        var groupedSessions = sessions.GroupBy(s => s.GetStartTime().Date);

        // Create a new Day prefab for each group and add the sessions to it
        foreach (var group in groupedSessions)
        {
            Day newDay = Instantiate(dayPrefab, transform);
            days.Add(newDay);

            foreach (SessionData session in group)
            {
                //newDay.AddSession(session);
            }
        }
    }

    private List<SessionData> GetSessionsForPeriod(ViewPeriod period)
    {
        // Get the sessions for the selected view period from your data source
        // This is just a placeholder and will need to be replaced with your actual implementation
        return new List<SessionData>();
    }
}
