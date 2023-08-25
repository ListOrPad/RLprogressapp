using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskChecker : MonoBehaviour
{
    void Start()
    {
        
    }
    void CheckTask(Task taskObject)
    {

        GameObject historyTaskObject = Instantiate(historyItemPrefab, history);
        historyTaskObject.transform.SetSiblingIndex(0);
        Task historyTask = historyTaskObject.GetComponent<Task>();
        int index = 0;
        if (historyTasks.Count > 0)
            index = historyTasks.Count;  //may cause problems with index value(no -1)
        historyTasks.Add(historyTask);
        historyTask.SetHistoryNumbering(historyTasks);
        historyTask.SetHistoryTaskInfo(taskObject.taskOnlyName, taskObject.reward, index);
        tasks.Remove(taskObject);
        Destroy(taskObject.gameObject);

        ///TransferDataForEdit(historyTaskObject,taskObject.taskOnlyName,taskObject.reward);

        Task temp = historyTask;
        historyTask.GetComponent<Toggle>().onValueChanged.AddListener(delegate { UncheckTask(temp); });
    }

    /// <summary>
    /// Returns the history task back to the To-Do list
    /// </summary>
    void UncheckTask(Task historyTask)
    {

        GameObject taskObject = Instantiate(checklistItemPrefab, content);
        taskObject.transform.SetSiblingIndex(0);
        Task task = taskObject.GetComponent<Task>();
        int index = 0;
        if (historyTasks.Count > 0)
            index = historyTasks.Count - 1;
        tasks.Add(task);
        historyTasks.Remove(historyTask);
        historyTask.SetHistoryNumbering(historyTasks);
        task.SetTaskInfo(historyTask.taskOnlyName, historyTask.reward, index);
        historyTask.SetHistoryTaskInfo(task.taskOnlyName, task.reward, index);
        Destroy(historyTask.gameObject);

        Task temp = task;
        task.GetComponent<Toggle>().onValueChanged.AddListener(delegate { CheckTask(temp); });
    }
}
