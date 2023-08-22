using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ChecklistManager : MonoBehaviour
{
    public Transform content;
    public GameObject editPanel;
    public Button saveButton;
    public GameObject checklistItemPrefab;
    public GameObject historyItemPrefab;
    public Transform history;

    private string filepath;

    private List<Task> tasks = new List<Task> ();
    private List<Task> historyTasks = new List<Task> ();

    private TMP_InputField[] addInputFields;

    internal List<GameObject> taskObjects = new List<GameObject>();

    [Header("History Toggle")]
    public Toggle historyToggle;
    public GameObject historyToggleObject;
    public GameObject scrollViewHistory;

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
        filepath = Application.persistentDataPath + "/checklist.txt";
        addInputFields = editPanel.GetComponentsInChildren<TMP_InputField>();

        saveButton.onClick.AddListener(delegate { CreateChecklistItem(addInputFields[0].text, TryParseInput(addInputFields[1].text)); } );
        historyToggle.onValueChanged.AddListener(delegate { ToggleHistoryVisibility(); } );

        //maybe transfer later next line to somewhere where it can get taskObj var
        //delete?
        //editSaveButtonObj.GetComponent<Button>().onClick.AddListener(delegate { EditTask(); });
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


    /// <summary>
    /// Moves history toggle to a new position; sets history list (in)visible; expands and compresses scroll view size; changes sprite
    /// </summary>
    private void ToggleHistoryVisibility()
    {
        if (scrollViewHistory.activeSelf == false)
        {
            //move history toggle up
            historyToggleObject.GetComponent<RectTransform>().offsetMin = new Vector2(2f, 750f); //(left, bottom)
            historyToggleObject.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 750f); //(-right, -top)
            //set scrollview visible
            scrollViewHistory.SetActive(true);
            //compress base checklist scrollview size
            GameObject scrollViewChecklist = GameObject.Find("Scroll View Checklist");
            scrollViewChecklist.GetComponent<RectTransform>().offsetMin = new Vector2(-25f, 885.5f);
            //change sprite
            Sprite historyArrowEnabled = Resources.Load<Sprite>("Images/History Enabled Arrow");
            Image[] imageComponentsInToggle = historyToggleObject.GetComponentsInChildren<Image>();
            Image secondImageInToggle = imageComponentsInToggle[1];
            secondImageInToggle.sprite = historyArrowEnabled;
        }
        else if (scrollViewHistory.activeSelf == true)
        {
            //move history toggle down
            historyToggleObject.GetComponent<RectTransform>().offsetMin = new Vector2(1f, -1.5f); //(left, bottom)
            historyToggleObject.GetComponent<RectTransform>().offsetMax = new Vector2(0f, -0.5f); //(-right, -top)
            //set scrollview invisible
            scrollViewHistory.SetActive(false);
            //expand base checklist scrollview size
            GameObject scrollViewChecklist = GameObject.Find("Scroll View Checklist");
            scrollViewChecklist.GetComponent<RectTransform>().offsetMin = new Vector2(-25f, 194f);
            //change sprite
            Sprite historyArrowDisabled = Resources.Load<Sprite>("Images/History Disabled Arrow");
            Image[] imageComponentsInToggle = historyToggleObject.GetComponentsInChildren<Image>();
            Image secondImageInToggle = imageComponentsInToggle[1];
            secondImageInToggle.sprite = historyArrowDisabled;
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
        PrepareEditButtons(taskObject, name, reward);

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
        historyTask.SetHistoryNumbering(historyTasks);
        historyTask.SetHistoryTaskInfo(taskObject.taskOnlyName, taskObject.reward, index);
        tasks.Remove(taskObject);
        Destroy(taskObject.gameObject);

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
        historyTask.SetHistoryNumbering(historyTasks);
        task.SetTaskInfo(historyTask.taskOnlyName, historyTask.reward, index);
        historyTask.SetHistoryTaskInfo(task.taskOnlyName, task.reward, index);
        Destroy(historyTask.gameObject);

        Task temp = task;
        task.GetComponent<Toggle>().onValueChanged.AddListener(delegate { CheckTask(temp); });
    }

    public void PrepareEditButtons(GameObject taskObject, string name, int reward)
    {
        taskObjects.Add(taskObject);
        names.Add(name);
        rewards.Add(reward);

        names.Reverse();
        rewards.Reverse();

        for (int i = 0; i < taskObjects.Count; i++)
        {
            editButtonsCollection.Add(taskObjects[i].GetComponentInChildren<Button>());

            int index = i;
            editButtonsCollection[i].onClick.AddListener(delegate { editPanel.SetActive(true); });
            editButtonsCollection[i].onClick.AddListener(delegate { recycleBin.SetActive(true); });
            editButtonsCollection[i].onClick.AddListener(delegate { SwitchSaveButtonTo("EDIT"); });
            editButtonsCollection[i].onClick.AddListener(delegate { TransferDataForEdit(taskObjects[index], names[index], rewards[index]); });
        }
    }

    /// <summary>
    /// called when Edit button(attached to target task) is pressed;
    /// transfers data from target task to edit panel
    /// </summary>
    public void TransferDataForEdit(GameObject taskObject, string name, int reward)
    {
        addInputFields[0].text = name;
        addInputFields[1].text = reward.ToString();
        editSaveButton.onClick.AddListener(delegate { EditTask(taskObject); });
        deleteTaskButton.onClick.AddListener(delegate { DeleteTask(taskObject); });
    }

    /// <summary>
    /// Called when SaveEdit button in edit panel is pressed;
    /// Changes data in task that's edited
    /// </summary>
    public void EditTask(GameObject taskObject)
    {
        Task task = taskObject.GetComponent<Task>();
        task.name = addInputFields[0].text;
        task.reward = int.Parse(addInputFields[1].text);
        task.taskText.text = addInputFields[0].text + $" {{{task.reward}}}";

        //when task is finished editing
        SwitchSaveButtonTo("SAVE");
        recycleBin.SetActive(false);
        editPanel.SetActive(false);
    }
    /// <summary>
    /// Called when delete button pressed in edit panel of edit mode
    /// </summary>
    public void DeleteTask(GameObject taskObject)
    {
        Task task = taskObject.GetComponent<Task>();
        tasks.Remove(task);
        Destroy(taskObject.gameObject);
        editPanel.SetActive(false);
        SwitchSaveButtonTo("SAVE");
        recycleBin.SetActive(false);
    }
    public void DeleteHistoryTask(GameObject historyTask)
    {
        Task historyTaskObject = historyTask.GetComponent<Task>();
        tasks.Remove(historyTaskObject);
        Destroy(historyTask);
    }
    /// <summary>
    /// Just type in string, which Save button(located in edit panel) mode you want: ("SAVE") or ("EDIT")
    /// </summary>
    public void SwitchSaveButtonTo(string saveMode)
    {
        if (saveMode == "EDIT")
        {
            saveButtonObj.SetActive(false);
            editSaveButtonObj.SetActive(true); 
        }
        else if (saveMode == "SAVE")
        {
            saveButtonObj.SetActive(true);
            editSaveButtonObj.SetActive(false);
        }
    }
}
