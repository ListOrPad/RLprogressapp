using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ChecklistManager : MonoBehaviour
{
    [SerializeField]
    private Transform content;
    [SerializeField]
    private GameObject editPanel;
    [SerializeField]
    private Button saveButton;
    [SerializeField]
    private GameObject checklistItemPrefab;
    [SerializeField]
    private GameObject historyItemPrefab;
    [SerializeField]
    private Transform history;

    private string filepath;

    private List<Task> tasks = new List<Task>();

    public List<Task> Tasks { get  => tasks; set => tasks = value; }
    private List<Task> historyTasks = new List<Task> ();
    private TMP_InputField[] addInputFields;

    private TaskUpdater updater;
    private EditManager editManager;

    private void Start()
    {
        updater = GetComponent<TaskUpdater>();
        editManager = GetComponent<EditManager>();
        filepath = Application.persistentDataPath + "/checklist.txt";
        addInputFields = editPanel.GetComponentsInChildren<TMP_InputField>();

        saveButton.onClick.AddListener(delegate { CreateChecklistItem(addInputFields[0].text, TryParseInput(addInputFields[1].text)); } );
    }

    private int TryParseInput(string input)
    {
        try
        {
            return int.Parse(input);
        }
        catch
        {
            return 0;
        }
    }

    private void CreateChecklistItem(string name, int reward) 
    {
        if (name == "")
        {
            editPanel.SetActive(false);
            return;
        }

        GameObject taskObject = Instantiate(checklistItemPrefab, content);
        taskObject.transform.SetSiblingIndex(0);

        //prepare task for using edit button on task create, collecting data on lists
        editManager.PrepareEditButtons(taskObject, name, reward);

        //Manipulations with Task
        Task task = taskObject.GetComponent<Task>();
        int index = 0;
        if(Tasks.Count > 0)
            index = Tasks.Count - 1;
        task.SetTaskInfo(name, reward, index);
        Tasks.Add(task);

        //transfer data to history object on task check
        Task temp = task;
        task.GetComponent<Toggle>().onValueChanged.AddListener(delegate { CheckTask(temp);});

        editPanel.SetActive(false);
    }

    /// <summary>
    /// Transfers the task to the history
    /// </summary>
    private void CheckTask(Task task)
    {

        GameObject historyTaskObject = Instantiate(historyItemPrefab, history);
        historyTaskObject.transform.SetSiblingIndex(0);
        Task historyTask = historyTaskObject.GetComponent<Task>();
        int index = 0;
        if(historyTasks.Count > 0)
            index = historyTasks.Count;  //may cause problems with index value(no -1)
        historyTasks.Add(historyTask);
        updater.UpdateHistory(historyTasks);;
        historyTask.SetHistoryTaskInfo(task.taskOnlyName, task.reward, index);
        Tasks.Remove(task);
        editManager.taskObjects.Remove(task.gameObject);
        Destroy(task.gameObject);

        //TransferDataForEdit(historyTaskObject,taskObject.taskOnlyName,taskObject.reward);

        Task temp = historyTask;
        historyTask.GetComponent<Toggle>().onValueChanged.AddListener(delegate { UncheckTask(temp); });
    }

    /// <summary>
    /// Returns the history task back to the To-Do list
    /// </summary>
    private void UncheckTask(Task historyTask)
    {
        
        GameObject taskObject = Instantiate(checklistItemPrefab, content);
        taskObject.transform.SetSiblingIndex(0);
        Task task = taskObject.GetComponent<Task>();
        int index = 0;
        if(historyTasks.Count > 0)
            index = historyTasks.Count - 1;
        Tasks.Add(task);
        historyTasks.Remove(historyTask);
        updater.UpdateHistory(historyTasks);
        task.SetTaskInfo(historyTask.taskOnlyName, historyTask.reward, index);
        historyTask.SetHistoryTaskInfo(task.taskOnlyName, task.reward, index);
        editManager.taskObjects.Add(historyTask.gameObject);
        Destroy(historyTask.gameObject);

        Task temp = task;
        task.GetComponent<Toggle>().onValueChanged.AddListener(delegate { CheckTask(temp); });
    }
}
