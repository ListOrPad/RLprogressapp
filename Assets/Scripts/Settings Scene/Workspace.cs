using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Workspace
{
    public string Name { get; set;}
    public float TimeValue { get; set; }
    public float StorageValue { get; set; }
    public int Prestige { get; set; }
    public List<SessionData> Sessions { get; set; }

    private static string filePath;

    public Workspace()
    {
        Name = "Default Workspace";
        StorageValue = 0;
        Sessions = new List<SessionData>();
    }
    public Workspace(string name)
    {
        Name = name;
        StorageValue = 0;
        TimeValue = 0;
        Prestige = 0;
    }
    public Workspace(string name, float storageValue, float timeValue, int prestige)
    {
        Name = name;
        StorageValue = storageValue;
        TimeValue = timeValue;
        Prestige = prestige;
        Sessions = new List<SessionData>();
    }

    public void Initialize()
    {
        filePath = Path.Combine(Application.persistentDataPath, Name + "_sessions.json");
        LoadSessions();
    }

    public void AddSession(SessionData session)
    {
        Sessions.Add(session);
        SaveSessions();
    }

    private void SaveSessions()
    {
        if (Sessions.Count > 0)
        {
            string json = JsonUtility.ToJson(new SerializationSessionWrapper<SessionData> { Items = Sessions });
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
                Sessions.Clear();
                Sessions.AddRange(data.Items);
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