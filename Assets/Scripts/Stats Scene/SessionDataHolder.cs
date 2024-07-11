using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SessionDataHolder
{
    public static List<SessionData> Sessions { get; }

    private static string filePath;

    static SessionDataHolder()
    {
        Sessions = new List<SessionData>();
        filePath = Path.Combine(Application.persistentDataPath, "sessions.json");
        LoadSessions();
    }

    public static void AddSession(SessionData session)
    {
        Sessions.Add(session);
        SaveSessions();
    }

    private static void SaveSessions()
    {
        if (Sessions.Count > 0)
        {
            string json = JsonUtility.ToJson(new SerializationWrapper<SessionData> { Items = Sessions });
            File.WriteAllText(filePath, json);
            Debug.Log("Saved sessions: " + json); // Log the saved sessions
        }
        else
        {
            Debug.Log("No sessions to save."); // Log if there are no sessions to save
        }
    }

    private static void LoadSessions()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            Debug.Log("JSON from file: " + json); // Log the contents of the JSON file

            SerializationWrapper<SessionData> data = JsonUtility.FromJson<SerializationWrapper<SessionData>>(json);

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
}

[System.Serializable]
public class SerializationWrapper<T>
{
    public List<T> Items;
}