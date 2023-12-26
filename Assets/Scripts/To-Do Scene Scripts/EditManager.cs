using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditManager : MonoBehaviour
{
    [Header("Edit task")]
    [SerializeField] private GameObject saveButtonObj;
    [SerializeField] private GameObject editSaveButtonObj;
    [SerializeField] private Button editSaveButton;
    [SerializeField] private GameObject editPanel;

    [Header("Delete task")]
    [SerializeField] private GameObject recycleBin;
    [SerializeField] private Button deleteTaskButton;

    private TMP_InputField[] addInputFields;

    [SerializeField] private GameObject checklistManagerObject;
    private ChecklistManager manager = new ChecklistManager();

    void Start()
    {
        manager = checklistManagerObject.GetComponent<ChecklistManager>();
        addInputFields = editPanel.GetComponentsInChildren<TMP_InputField>();
    }

    /// <summary>
    /// When creating a task, prepare its edit button for interaction
    /// </summary>
    public void PrepareEditButtons(Task task, GameObject taskObject)
    {
        Button editButton = taskObject.GetComponentInChildren<Button>();
        AddEditEvents(task, editButton);
    }

    private void AddEditEvents(Task task, Button editButton)
    {
        editButton.onClick.AddListener(() =>
        {
            editPanel.SetActive(true);
            recycleBin.SetActive(true);
            SwitchSaveButtonTo("EDIT");
            TransferDataForEdit(task);
        });
    }

    /// <summary>
    /// called when Edit button(attached to target task) is pressed;
    /// transfers data from target task to edit panel
    /// </summary>
    public void TransferDataForEdit(Task task)
    {
        //Transfer name and reward
        addInputFields[0].text = task.taskOnlyName;
        addInputFields[1].text = task.reward.ToString();

        editSaveButton.onClick.RemoveAllListeners();
        deleteTaskButton.onClick.RemoveAllListeners();
        //add events that call edit or delete operations when corresponding buttons are pressed
        editSaveButton.onClick.AddListener(delegate { EditTask(task); });
        deleteTaskButton.onClick.AddListener(delegate { DeleteTask(task); });
    }

    /// <summary>
    /// Called when SaveEdit button in edit panel is pressed;
    /// Changes data in task that's edited
    /// </summary>
    public void EditTask(Task task)
    {
        task.SetTaskInfo(addInputFields[0].text, int.Parse(addInputFields[1].text), task.index);
        task.taskText.text = addInputFields[0].text + $" {{{task.reward}}}";

        //when task is finished editing
        FinalizeEdit();
    }

    /// <summary>
    /// Called when delete button pressed in edit mode
    /// </summary>
    public void DeleteTask(Task task)
    {
        Destroy(task.gameObject);
        manager.Tasks.Remove(task);
        manager.SaveJSONData(manager.Tasks);
        FinalizeEdit();
    }

    //public void DeleteHistoryTask(GameObject historyTaskObject)
    //{
    //    Task historyTask = historyTaskObject.GetComponent<Task>();
    //    tasks.Remove(historyTask);
    //    Destroy(historyTask);
    //}

    private void FinalizeEdit()
    {
        SwitchSaveButtonTo("SAVE");
        recycleBin.SetActive(false);
        editPanel.SetActive(false);
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
