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

    [Header("History Toggle")]
    public Toggle historyToggle;
    public GameObject historyToggleObject;
    public GameObject scrollViewHistory;
    //public Sprite historyArrowEnabled;
    //public Sprite historyArrowDisabled;
    private Image toggleImage;

    private void Start()
    {
        filepath = Application.persistentDataPath + "/checklist.txt";
        addInputFields = editPanel.GetComponentsInChildren<TMP_InputField>();

        saveButton.onClick.AddListener(delegate { CreateChecklistItem(addInputFields[0].text, TryParseInput(addInputFields[1].text)); } );
        historyToggle.onValueChanged.AddListener(delegate { ToggleHistoryVisibility(); } );
    }

    /// <summary>
    /// Enable or disable edit task panel
    /// </summary>
    public void SwitchMode(int mode)
    {
        switch(mode)
        {
            case 0:
                editPanel.SetActive(false);
                break;
            case 1:
                editPanel.SetActive(true);
                break;
        }
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

    private void MoveTasks(GameObject task)
    {
        //make a list or array of tasks?
    }

    /// <summary>
    /// Moves history toggle to a new position and sets history list (in)visible
    /// </summary>
    private void ToggleHistoryVisibility()
    {
        if (scrollViewHistory.activeSelf == false)
        {
            //move history toggle up
            historyToggleObject.GetComponent<RectTransform>().offsetMin = new Vector2(2f, 750f); //(left, bottom)
            historyToggleObject.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 750f); //(-right, -top)
            //set scrollview active
            scrollViewHistory.SetActive(true);
            //change sprite
            Sprite historyArrowEnabled = Resources.Load<Sprite>("Images/History Enabled Arrow");
            Image[] imageComponents = historyToggleObject.GetComponentsInChildren<Image>();
            Image secondImageInToggle = imageComponents[1];
            secondImageInToggle.sprite = historyArrowEnabled;
        }
        else if (scrollViewHistory.activeSelf == true)
        {
            //move history toggle down
            historyToggleObject.GetComponent<RectTransform>().offsetMin = new Vector2(1f, -1.5f); //(left, bottom)
            historyToggleObject.GetComponent<RectTransform>().offsetMax = new Vector2(0f, -0.5f); //(-right, -top)
            //set scrollview inactive
            scrollViewHistory.SetActive(false);
            //change sprite
            Sprite historyArrowDisabled = Resources.Load<Sprite>("Images/History Disabled Arrow");
            Image[] imageComponents = historyToggleObject.GetComponentsInChildren<Image>();
            Image secondImageInToggle = imageComponents[1];
            secondImageInToggle.sprite = historyArrowDisabled;
        }
    }
    void CreateChecklistItem(string name, int reward) 
    {
        if (name == "")
        {
            SwitchMode(0);
            return;
        }
        GameObject task = Instantiate(checklistItemPrefab, content);
        task.transform.SetSiblingIndex(0);
        ChecklistObject taskObject = task.GetComponent<ChecklistObject>();
        int index = 0;
        if(checklistObjects.Count > 0)
            index = checklistObjects.Count - 1;
        taskObject.SetObjectInfo(name, reward, index);
        checklistObjects.Add(taskObject);
        ChecklistObject temp = taskObject;
        taskObject.GetComponent<Toggle>().onValueChanged.AddListener(delegate { CheckTask(temp);});

        SwitchMode(0);
    }

    /// <summary>
    /// Transfers the task to the history
    /// </summary>
    void CheckTask(ChecklistObject taskObject)
    {

        GameObject historyTask = Instantiate(historyItemPrefab, history);
        historyTask.transform.SetSiblingIndex(1);
        //CrossHistoryTask();
        ChecklistObject historyObject = historyTask.GetComponent<ChecklistObject>();
        int index = 0;
        if(historyObjects.Count > 0)
            index = historyObjects.Count;  //may cause problems with index value(no -1)
        historyObjects.Add(historyObject);
        historyObject.SetHistoryNumbering(historyObjects);
        historyObject.SetHistoryObjectInfo(taskObject.objName, taskObject.reward, index);
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
        taskObject.SetObjectInfo(historyObject.objNameNoNumber, historyObject.reward, index);
        historyObject.SetHistoryObjectInfo(taskObject.objName, taskObject.reward, index);
        Destroy(historyObject.gameObject);

        ChecklistObject temp = taskObject;
        taskObject.GetComponent<Toggle>().onValueChanged.AddListener(delegate { CheckTask(temp); });
    }

   
    void Update()
    {
    }
}
