using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChecklistObject : MonoBehaviour
{
    internal int numberOfTask;
    internal string objOnlyName;
    public string objFullName;
    public int reward;
    public int index;

    private Text taskText;

    void Start()
    {
        taskText = GetComponent<Text>();
        taskText.text = objFullName;
    }

    public void SetObjectInfo(string name, int reward, int index)
    {
        this.objOnlyName = name;
        this.objFullName = $"{objOnlyName} {{{reward}}}";
        this.reward = reward;
        this.index = index;
    }
    public void SetHistoryObjectInfo(string name, int reward, int index)
    {
        this.objOnlyName = name;
        this.objFullName = $"{numberOfTask}. {objOnlyName} {{{reward}}}";
        this.reward = reward;
        this.index = index;
    }

    /// <summary>
    /// Updates history numbering each time task is checked or unchecked
    /// </summary>
    internal void SetHistoryNumbering(List<ChecklistObject> historyObjects)
    {
        GameObject history = GameObject.Find("History");

        taskText = GetComponent<Text>();

        historyObjects.Sort((a, b) => b.transform.GetSiblingIndex().CompareTo(a.transform.GetSiblingIndex()));

        for (int i = 0; i < historyObjects.Count; i++)
        {
            ChecklistObject historyObject = historyObjects[i];
            historyObject.numberOfTask = i + 1;
            historyObject.taskText.text = $"{historyObject.numberOfTask}. {historyObject.objOnlyName} {{{historyObject.reward}}}";
        }
    }
}
