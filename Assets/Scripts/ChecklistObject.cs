using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChecklistObject : MonoBehaviour
{
    internal int numberOfTask;
    internal string objNameNoNumber;
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
    public void SetHistoryObjectInfo(string name, int reward, int index)
    {
        this.objName = $"{numberOfTask}. " + name;
        this.objNameNoNumber = name;
        this.reward = reward;
        this.index = index;
    }

    /// <summary>
    /// Updates history numbering each time task is checked or unchecked
    /// </summary>
    internal void SetHistoryNumbering(List<ChecklistObject> historyObjects)
    {
        GameObject history = GameObject.Find("History");

        foreach (ChecklistObject historyObject in historyObjects)
        {
            itemText = GetComponentInChildren<Text>();
            historyObject.itemText.text = historyObject.objNameNoNumber;
        }

        historyObjects.Sort((a, b) => a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex()));

        for (int i = 0; i < historyObjects.Count; i++)
        {
            ChecklistObject historyObject = historyObjects[i];
            historyObject.numberOfTask = i + 1;
            historyObject.itemText.text = $"{historyObject.numberOfTask}. {historyObject.objNameNoNumber}";
        }
    }
}
