using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Workspace
{
    public string name;
    public float storageValue;
    public List<SessionData> sessionData;

    public Workspace(string name, float storageValue)
    {
        this.name = name;
        this.storageValue = storageValue;
        this.sessionData = new List<SessionData>();  //we need to get session data from somewhere. maybe create InstantiateSessionData() and pass it here?
    }

    public void AddSession(SessionData session)
    {
        sessionData.Add(session);
    }
}
