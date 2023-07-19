using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChecklistObject : MonoBehaviour
{
    internal int numberOfTask;
    internal string objNameForUncheck;
    public string objName;
    public int reward;
    public int index;

    private Text itemText;

    void Start()
    {
        itemText = GetComponentInChildren<Text>();
        itemText.text = objName;
    }
    
    public void SetObjectInfo(string name, int reward, int index)
    {
        this.objName = name;
        this.reward = reward;
        this.index = index;
    }
    public void SetHistoryObjectInfo(int numberOfTask, string name, int reward, int index)
    {
        this.numberOfTask = numberOfTask;
        this.objName = $"{this.numberOfTask}. " + name;
        this.objNameForUncheck = name;
        this.reward = reward;
        this.index = index;
    }
}
