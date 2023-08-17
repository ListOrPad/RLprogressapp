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

    private List<ChecklistObject> checklistObjects = new List<ChecklistObject> ();
    private List<ChecklistObject> historyObjects = new List<ChecklistObject> ();

    private TMP_InputField[] addInputFields;

    internal List<GameObject> tasks = new List<GameObject>();

    [Header("History Toggle")]
    public Toggle historyToggle;
    public GameObject historyToggleObject;
    public GameObject scrollViewHistory;

    [Header("Edit task")]
    public GameObject saveButtonObj;
    public GameObject editSaveButtonObj;
    public GameObject deleteTaskButton;
    //public Button editButton;    // NOTE THIS ONE
    public Button editSaveButton;


    private void Start()
    {
        filepath = Application.persistentDataPath + "/checklist.txt";
        addInputFields = editPanel.GetComponentsInChildren<TMP_InputField>();

        saveButton.onClick.AddListener(delegate { CreateChecklistItem(addInputFields[0].text, TryParseInput(addInputFields[1].text)); } );
        historyToggle.onValueChanged.AddListener(delegate { ToggleHistoryVisibility(); } );

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

        GameObject task = Instantiate(checklistItemPrefab, content);

        //add a task on a list for wrapping 
        //I probably dont need it(And 'tasks' var)
        tasks.Add(task);

        //prepare task for using edit button on task create
        Button editButton = task.GetComponentInChildren<Button>();
        editButton.onClick.AddListener(delegate { editPanel.SetActive(true); });
        editButton.onClick.AddListener(delegate { TransferDataForEdit(task, name, reward); });
        editButton.onClick.AddListener(delegate { SwitchSaveButtonTo("EDIT"); });

        task.transform.SetSiblingIndex(0);
        ChecklistObject taskObject = task.GetComponent<ChecklistObject>();
        int index = 0;
        if(checklistObjects.Count > 0)
            index = checklistObjects.Count - 1;
        taskObject.SetObjectInfo(name, reward, index);
        checklistObjects.Add(taskObject);
        ChecklistObject temp = taskObject;
        taskObject.GetComponent<Toggle>().onValueChanged.AddListener(delegate { CheckTask(temp);});

        editPanel.SetActive(false);
    }

    /// <summary>
    /// Transfers the task to the history
    /// </summary>
    void CheckTask(ChecklistObject taskObject)
    {

        GameObject historyTask = Instantiate(historyItemPrefab, history);
        historyTask.transform.SetSiblingIndex(0);
        ChecklistObject historyObject = historyTask.GetComponent<ChecklistObject>();
        int index = 0;
        if(historyObjects.Count > 0)
            index = historyObjects.Count;  //may cause problems with index value(no -1)
        historyObjects.Add(historyObject);
        historyObject.SetHistoryNumbering(historyObjects);
        historyObject.SetHistoryObjectInfo(taskObject.objOnlyName, taskObject.reward, index);
        checklistObjects.Remove(taskObject);
        Destroy(taskObject.gameObject);

        ChecklistObject temp = historyObject;
        historyObject.GetComponent<Toggle>().onValueChanged.AddListener(delegate { UncheckTask(temp); });
    }

    /// <summary>
    /// Returns the history task back to the To-Do list
    /// </summary>
    void UncheckTask(ChecklistObject historyObject)
    {
        
        GameObject task = Instantiate(checklistItemPrefab, content);
        task.transform.SetSiblingIndex(0);
        ChecklistObject taskObject = task.GetComponent<ChecklistObject>();
        int index = 0;
        if(historyObjects.Count > 0)
            index = historyObjects.Count - 1;
        checklistObjects.Add(taskObject);
        historyObjects.Remove(historyObject);
        historyObject.SetHistoryNumbering(historyObjects);
        taskObject.SetObjectInfo(historyObject.objOnlyName, historyObject.reward, index);
        historyObject.SetHistoryObjectInfo(taskObject.objOnlyName, taskObject.reward, index);
        Destroy(historyObject.gameObject);

        ChecklistObject temp = taskObject;
        taskObject.GetComponent<Toggle>().onValueChanged.AddListener(delegate { CheckTask(temp); });
    }

    /// <summary>
    /// called when Edit button(attached to target task) is pressed;
    /// transfers data from target task to edit panel
    /// </summary>
    public void TransferDataForEdit(GameObject task, string name, int reward)
    {
        ChecklistObject taskObject = task.GetComponent<ChecklistObject>();
        addInputFields[0].text = name;
        addInputFields[1].text = reward.ToString();


    }

    /// <summary>
    /// Called when SaveEdit button in edit panel is pressed;
    /// Changes data in task that's edited
    /// </summary>
    public void EditTask(GameObject task)
    {
        ChecklistObject taskObject = task.GetComponent<ChecklistObject>();

        //when task is finished editing
        SwitchSaveButtonTo("SAVE");
        editPanel.SetActive(false);
    }
    /// <summary>
    /// Called when delete button pressed in edit panel of edit mode
    /// </summary>
    public void DeleteTask(GameObject task)
    {
        ChecklistObject taskObject = task.GetComponent<ChecklistObject>();
        deleteTaskButton.SetActive(true);
        checklistObjects.Remove(taskObject);
        Destroy(task);
        SwitchSaveButtonTo("SAVE") ;
        deleteTaskButton.SetActive(false);
    }
    public void DeleteHistoryTask(GameObject historyTask)
    {
        ChecklistObject historyTaskObject = historyTask.GetComponent<ChecklistObject>();
        checklistObjects.Remove(historyTaskObject);
        Destroy(historyTask);
    }
    /// <summary>
    /// Just type in string, which Save button mode you want: ("SAVE") or ("EDIT")
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
