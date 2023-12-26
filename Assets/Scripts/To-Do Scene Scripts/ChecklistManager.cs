using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChecklistManager : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private GameObject editPanel;
    [SerializeField] private Button saveButton;
    [SerializeField] private GameObject checklistItemPrefab;
    [SerializeField] private GameObject historyItemPrefab;
    [SerializeField] private Transform history;

    private string filepath;
    private string historyFilepath;

    private List<Task> tasks = new List<Task>();

    public List<Task> Tasks { get  => tasks; set => tasks = value; }
    private List<Task> historyTasks = new List<Task> ();
    private TMP_InputField[] addInputFields;

    private TaskUpdater updater;
    private EditManager editManager;

    private void Start()
    {
        filepath = Application.persistentDataPath + "/checklist.txt";
        historyFilepath = Application.persistentDataPath + "/history.txt";

        updater = GetComponent<TaskUpdater>();
        editManager = GetComponent<EditManager>();

        LoadJSONData();
        LoadJSONHistoryData();

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

    private void CreateChecklistItem(string name, int reward, int loadIndex = 0, bool loading = false) 
    {
        if (name == "")
        {
            editPanel.SetActive(false);
            return;
        }

        GameObject taskObject = Instantiate(checklistItemPrefab, content);
        taskObject.transform.SetSiblingIndex(0);

        Task task = taskObject.GetComponent<Task>();

        //prepare task for using edit button on task create, collecting data on lists

        int index = loadIndex;
        if(!loading)
        {
            index = Tasks.Count;
        }
        task.SetTaskInfo(name, reward, index);
        Tasks.Add(task);
        editManager.PrepareEditButtons(task, taskObject);

        //transfer data to history object on task check
        task.GetComponent<Toggle>().onValueChanged.AddListener(delegate { CheckTask(task);});

        editPanel.SetActive(false);

        if(!loading)
        {
            SaveJSONData();
        }
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
            index = historyTasks.Count;
        historyTasks.Add(historyTask);
        updater.UpdateHistory(historyTasks);;
        historyTask.SetHistoryTaskInfo(task.taskOnlyName, task.reward, index);
        Tasks.Remove(task);
        SaveJSONData();
        SaveJSONHistoryData();
        Destroy(task.gameObject);

        historyTask.GetComponent<Toggle>().onValueChanged.AddListener(delegate { UncheckTask(historyTask); });
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
            index = historyTasks.Count;
        Tasks.Add(task);
        updater.UpdateHistory(historyTasks);
        task.SetTaskInfo(historyTask.taskOnlyName, historyTask.reward, index);
        editManager.PrepareEditButtons(task, taskObject);
        historyTask.SetHistoryTaskInfo(task.taskOnlyName, task.reward, index);
        historyTasks.Remove(historyTask);
        SaveJSONData();
        SaveJSONHistoryData();
        Destroy(historyTask.gameObject);

        task.GetComponent<Toggle>().onValueChanged.AddListener(delegate { CheckTask(task); });
    }

    private void SaveJSONData()
    {
        string contents = "";

        for (int i = 0; i < Tasks.Count; i++)
        {
            TaskItem temp = new TaskItem(Tasks[i].taskOnlyName, Tasks[i].reward, Tasks[i].index);
            contents += JsonUtility.ToJson(temp) + "\n";
        }

        File.WriteAllText(filepath, contents);
    }
    
    private void SaveJSONHistoryData()
    {
        string contents = "";

        for (int i = 0; i < historyTasks.Count; i++)
        {
            TaskItem temp = new TaskItem(historyTasks[i].taskOnlyName, historyTasks[i].reward, historyTasks[i].index);
            contents += JsonUtility.ToJson(temp) + "\n";
        }

        File.WriteAllText(historyFilepath, contents);
    }

    private void LoadJSONData()
    {
        if(File.Exists(filepath))
        {
            string contents = File.ReadAllText(filepath);
            string[] splitContents = contents.Split('\n');

            foreach (string content in splitContents)
            {
                if (content.Trim() != "")
                {
                    TaskItem temp = JsonUtility.FromJson<TaskItem>(content.Trim());
                    CreateChecklistItem(temp.name, temp.reward, temp.index, true);
                }
            }
        }
        else
        {
            Debug.Log("No file!");
        }
    }

    private void CreateHistoryItem(string name, int reward, int loadIndex = 0, bool loading = false)
    {
        GameObject historyTaskObject = Instantiate(historyItemPrefab, history);
        historyTaskObject.transform.SetSiblingIndex(0);
        Task historyTask = historyTaskObject.GetComponent<Task>();

        int index = loadIndex;
        if (!loading)
        {
            index = Tasks.Count;
        }

        historyTasks.Add(historyTask);
        updater.UpdateHistory(historyTasks); ;
        historyTask.SetHistoryTaskInfo(name, reward, index);

        historyTask.GetComponent<Toggle>().onValueChanged.AddListener(delegate { UncheckTask(historyTask); });

        if (!loading)
        {
            SaveJSONHistoryData();
        }
    }

    private void LoadJSONHistoryData()
    {
        if (File.Exists(historyFilepath))
        {
            string contents = File.ReadAllText(historyFilepath);
            string[] splitContents = contents.Split('\n');

            foreach (string content in splitContents)
            {
                if (content.Trim() != "")
                {
                    TaskItem temp = JsonUtility.FromJson<TaskItem>(content.Trim());
                    CreateHistoryItem(temp.name, temp.reward, temp.index, true);
                }
            }
        }
        else
        {
            Debug.Log("No file!");
        }
    }

    public class TaskItem
    {
        public string name;
        public int reward;
        public int index;

        public TaskItem(string name, int reward, int index)
        {
            this.name = name;
            this.reward = reward;
            this.index = index;
        }
    }
}
