using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Workspace
{
    public string Name { get; set; }
    public float TimeValue { get; set; }
    public float StorageValue { get; set; }
    public int Prestige { get; set; }

    public Workspace(string name = "Default Workspace", float storageValue = 0, float timeValue = 0, int prestige = 0)
    {
        Name = name;
        StorageValue = storageValue;
        TimeValue = timeValue;
        Prestige = prestige;
    }

    //public Workspace()
    //{
    //    Name = "Default Workspace";
    //    StorageValue = 0;
    //    TimeValue = 0;
    //    Prestige = 0;
    //}
    //public Workspace(string name = "Default Workspace")
    //{
    //    Name = name;
    //    StorageValue = 0;
    //    TimeValue = 0;
    //    Prestige = 0;
    //}
    //public Workspace(string name, float storageValue, float timeValue, int prestige)
    //{
    //    Name = name;
    //    StorageValue = storageValue;
    //    TimeValue = timeValue;
    //    Prestige = prestige;
    //}
}