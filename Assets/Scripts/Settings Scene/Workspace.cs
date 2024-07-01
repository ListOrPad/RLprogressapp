using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Workspace
{
    public string name;
    public float storageValue;
    public List<SessionData> sessions;

    private static string filePath;

    public Workspace()
    {
        name = "Default Workspace";
        storageValue = 0;
        sessions = new List<SessionData>();
    }
    public Workspace(string name, float storageValue)
    {
        this.name = name;
        this.storageValue = storageValue;
        this.sessions = new List<SessionData>();
    }

    public void Initialize()
    {
        filePath = Path.Combine(Application.persistentDataPath, name + "_sessions.json");
        LoadSessions();
    }

    public void AddSession(SessionData session)
    {
        sessions.Add(session);
        SaveSessions();
    }

    private void SaveSessions()
    {
        if (sessions.Count > 0)
        {
            string json = JsonUtility.ToJson(new SerializationSessionWrapper<SessionData> { Items = sessions });
            File.WriteAllText(filePath, json);
            Debug.Log("Saved sessions: " + json);
        }
        else
        {
            Debug.Log("No sessions to save.");
        }
    }

    private void LoadSessions()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            Debug.Log("JSON from file: " + json);

            SerializationSessionWrapper<SessionData> data = JsonUtility.FromJson<SerializationSessionWrapper<SessionData>>(json);

            if (data != null && data.Items != null)
            {
                sessions.Clear();
                sessions.AddRange(data.Items);
                Debug.Log("Loaded sessions: " + json);
            }
            else
            {
                Debug.Log("Data from JsonUtility.FromJson: " + data);
            }
        }
    }
}

[System.Serializable]
public class SerializationSessionWrapper<T>
{
    public List<T> Items;
}