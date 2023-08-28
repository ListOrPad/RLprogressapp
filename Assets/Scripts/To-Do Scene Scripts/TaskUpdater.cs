using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskUpdater : MonoBehaviour
{
    private bool taskEdited;
    private bool taskDeleted;
    void Start()
    {
        
    }

    void Update()
    {
        //UpdateHistory();
        //UpdateTasks();
    }

    /// <summary>
    /// Updates history numbering each time task is checked or unchecked
    /// </summary>
    internal void UpdateHistory(List<Task> historyObjects)
    {
        GameObject history = GameObject.Find("History");


        historyObjects.Sort((a, b) => b.transform.GetSiblingIndex().CompareTo(a.transform.GetSiblingIndex()));

        for (int i = 0; i < historyObjects.Count; i++)
        {
            Task historyObject = historyObjects[i];
            historyObject.taskText = historyObject.GetComponent<Text>();
            historyObject.numberOfTask = i + 1;
            historyObject.taskText.text = $"{historyObject.numberOfTask}. {historyObject.taskOnlyName} {{{historyObject.reward}}}";
        }
    }
    internal void UpdateTasks()
    {

    }
}
