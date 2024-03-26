using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day : MonoBehaviour
{
    [SerializeField] private GameObject sessionPrefab;
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

        // Update the size and position of the day prefab
        UpdateLayout();
    }

    private void UpdateLayout()
    {
        // Calculate the total height of all sessions
        float totalHeight = 0;
        foreach (GameObject session in sessions)
        {
            totalHeight += session.GetComponent<RectTransform>().sizeDelta.y;
        }

        // Set the height of the day prefab
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, totalHeight);
    }
}
