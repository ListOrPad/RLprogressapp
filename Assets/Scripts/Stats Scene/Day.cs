using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day : MonoBehaviour
{
    public GameObject sessionPrefab;
    private SessionManager sessionManager;

    public void AddSession(SessionData data)
    {
        // Instantiate a new session object
        GameObject newSession = Instantiate(sessionPrefab, sessionManager.content.transform);

        // Get the Session component and set the data
        Session session = newSession.GetComponent<Session>();
        session.SetData(data);
    }
}
