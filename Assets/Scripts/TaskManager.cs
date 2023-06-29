using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{

    public string filePath;

    private void Start()
    {
        filePath = Application.persistentDataPath + "/checklist.txt";
    }
}
