using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    string filepath;

    private List<ChecklistObject> checklistObjects = new List<ChecklistObject> ();
    private List<ChecklistObject> historyObjects = new List<ChecklistObject> (); //а тот ли тип?

    private TMP_InputField[] addInputFields;

    private void Start()
    {
        filepath = Application.persistentDataPath + "/checklist.txt";
        addInputFields = editPanel.GetComponentsInChildren<TMP_InputField> ();

        saveButton.onClick.AddListener(delegate { CreateChecklistItem(addInputFields[0].text, int.Parse(addInputFields[1].text)); });
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
    void CreateChecklistItem(string name, int reward) 
    {
        GameObject task = Instantiate(checklistItemPrefab, content);
        task.transform.SetSiblingIndex(1);
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
            index = historyObjects.Count;  //may cause problems (no -1)
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
        task.transform.SetSiblingIndex(1);
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
