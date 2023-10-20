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

    //collecting data for edit button
    private TMP_InputField[] addInputFields;
    private List<string> names = new List<string>();
    private List<int> rewards = new List<int>();
    private List<Button> editButtons = new List<Button>();
    private ChecklistManager manager;

    internal List<GameObject> taskObjects = new List<GameObject>();

    void Start()
    {
        addInputFields = editPanel.GetComponentsInChildren<TMP_InputField>();
    }

    /// <summary>
    /// When creating a task, prepare its edit button for interaction
    /// </summary>
    public void PrepareEditButtons(GameObject taskObject, string name, int reward)
    {
        //Add data to lists
        taskObjects.Add(taskObject);
        names.Add(name);
        rewards.Add(reward);

        //Reverse lists
        names.Reverse();
        rewards.Reverse();

        for (int i = 0; i < taskObjects.Count; i++)
        {
            //Get next taskobject's edit button in list and add it to editButtons list
            editButtons.Add(taskObjects[i].GetComponentInChildren<Button>());

            //Add events to next editButton in list that are called when user presses edit button
            AddEditEvents(i);
        }
    }

    private void AddEditEvents(int index)
    {
        editButtons[index].onClick.AddListener(delegate { editPanel.SetActive(true); });
        editButtons[index].onClick.AddListener(delegate { recycleBin.SetActive(true); });
        editButtons[index].onClick.AddListener(delegate { SwitchSaveButtonTo("EDIT"); });
        //###BUG: он добавился дважды+, этот ивент, поэтому метод вызывается несколько раз, типа два отдельных одинаковых ивента
        //получается добавился на одну кнопку? Так не должно быть...
        editButtons[index].onClick.AddListener(delegate { TransferDataForEdit(taskObjects[index], names[index], rewards[index]); });
    }

    public void TransferDataAfterEdit(string name, int reward)
    {
        addInputFields[0].text = name;
        addInputFields[1].text = reward.ToString();
    }
    /// <summary>
    /// called when Edit button(attached to target task) is pressed;
    /// transfers data from target task to edit panel
    /// </summary>
    public void TransferDataForEdit(GameObject taskObject, string name, int reward)
    {
        //Transfer name and reward
        addInputFields[0].text = name;
        addInputFields[1].text = reward.ToString();
        //add events that call edit or delete operations when corresponding buttons are pressed
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

        //Button editButton = taskObject.GetComponent<Button>();
        //TransferDataForEdit(editButton, taskObject, task.name, task.reward); // like this?

        //when task is finished editing
        SwitchSaveButtonTo("SAVE");
        recycleBin.SetActive(false);
        editPanel.SetActive(false);
    }

    /// <summary>
    /// Called when delete button pressed in edit mode
    /// </summary>
    public void DeleteTask(GameObject taskObject)
    {
        Task task = taskObject.GetComponent<Task>();
        manager.Tasks.Remove(task);
        Destroy(taskObject.gameObject);
        editPanel.SetActive(false);
        SwitchSaveButtonTo("SAVE");
        recycleBin.SetActive(false);
    }

    //public void DeleteHistoryTask(GameObject historyTaskObject)
    //{
    //    Task historyTask = historyTaskObject.GetComponent<Task>();
    //    tasks.Remove(historyTask);
    //    Destroy(historyTask);
    //}

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
