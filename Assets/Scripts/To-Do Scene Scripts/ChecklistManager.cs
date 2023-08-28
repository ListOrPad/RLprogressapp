using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ChecklistManager : MonoBehaviour
{
    #region legacy
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

    private List<Task> tasks = new List<Task> ();
    private List<Task> historyTasks = new List<Task> ();

    private TMP_InputField[] addInputFields;

    internal List<GameObject> taskObjects = new List<GameObject>();

    private TaskUpdater updater;

    [Header("Edit task")]
    public GameObject saveButtonObj;
    public GameObject editSaveButtonObj;
    public Button editSaveButton;
    public GameObject recycleBin;
    public Button deleteTaskButton;

    private List<Button> editButtonsCollection = new List<Button>();
    //collecting data for edit button
    private List<string> names = new List<string> ();
    private List<int> rewards = new List<int> ();

    private void Start()
    {
        updater = gameObject.AddComponent<TaskUpdater>();
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

    void CreateChecklistItem(string name, int reward) 
    {
        if (name == "")
        {
            editPanel.SetActive(false);
            return;
        }

        GameObject taskObject = Instantiate(checklistItemPrefab, content);
        taskObject.transform.SetSiblingIndex(0);

        //prepare task for using edit button on task create, collecting data on lists
        //PrepareEditButtons(taskObject, name, reward);

        //Manipulations with Task
        Task task = taskObject.GetComponent<Task>();
        int index = 0;
        if(tasks.Count > 0)
            index = tasks.Count - 1;
        task.SetTaskInfo(name, reward, index);
        tasks.Add(task);

        //transfer data to history object on task check
        Task temp = task;
        task.GetComponent<Toggle>().onValueChanged.AddListener(delegate { CheckTask(temp);});

        editPanel.SetActive(false);
    }

    /// <summary>
    /// Transfers the task to the history
    /// </summary>
    void CheckTask(Task taskObject)
    {

        GameObject historyTaskObject = Instantiate(historyItemPrefab, history);
        historyTaskObject.transform.SetSiblingIndex(0);
        Task historyTask = historyTaskObject.GetComponent<Task>();
        int index = 0;
        if(historyTasks.Count > 0)
            index = historyTasks.Count;  //may cause problems with index value(no -1)
        historyTasks.Add(historyTask);
        updater.UpdateHistory(historyTasks);;
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
        if(historyTasks.Count > 0)
            index = historyTasks.Count - 1;
        tasks.Add(task);
        historyTasks.Remove(historyTask);
        updater.UpdateHistory(historyTasks);
        task.SetTaskInfo(historyTask.taskOnlyName, historyTask.reward, index);
        historyTask.SetHistoryTaskInfo(task.taskOnlyName, task.reward, index);
        Destroy(historyTask.gameObject);

        Task temp = task;
        task.GetComponent<Toggle>().onValueChanged.AddListener(delegate { CheckTask(temp); });
    }

   
    #endregion
}
