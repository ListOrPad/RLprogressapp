using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Task : MonoBehaviour
{
    #region Legacy
    internal int numberOfTask;
    internal string taskOnlyName;
    public string taskFullName;
    public int reward;
    public int index;

    internal Text taskText;


    void Start()
    {
        taskText = GetComponent<Text>();
        taskText.text = taskFullName;
    }

    public void SetTaskInfo(string name, int reward, int index)
    {
        this.taskOnlyName = name;
        this.taskFullName = $"{taskOnlyName} {{{reward}}}";
        this.reward = reward;
        this.index = index;
    }
    public void SetHistoryTaskInfo(string name, int reward, int index)
    {
        this.taskOnlyName = name;
        this.taskFullName = $"{numberOfTask}. {taskOnlyName} {{{reward}}}";
        this.reward = reward;
        this.index = index;
    }

    ///// <summary>
    ///// Updates history numbering each time task is checked or unchecked
    ///// </summary>
    //internal void SetHistoryNumbering(List<Task> historyObjects)
    //{
    //    GameObject history = GameObject.Find("History");

    //    taskText = GetComponent<Text>();

    //    historyObjects.Sort((a, b) => b.transform.GetSiblingIndex().CompareTo(a.transform.GetSiblingIndex()));

    //    for (int i = 0; i < historyObjects.Count; i++)
    //    {
    //        Task historyObject = historyObjects[i];
    //        historyObject.numberOfTask = i + 1;
    //        historyObject.taskText.text = $"{historyObject.numberOfTask}. {historyObject.taskOnlyName} {{{historyObject.reward}}}";
    //    }
    //}
    #endregion
}
