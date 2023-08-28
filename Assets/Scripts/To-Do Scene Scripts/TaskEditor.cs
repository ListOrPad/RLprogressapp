using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskEditor : MonoBehaviour
{
    private string editedTaskName;
    private int editedTaskReward;
    void Start()
    {
        
    }
    //public void PrepareEditButtons(GameObject taskObject, string name, int reward)
    //{
    //    taskObjects.Add(taskObject);
    //    names.Add(name);
    //    rewards.Add(reward);

    //    names.Reverse();
    //    rewards.Reverse();

    //    for (int i = 0; i < taskObjects.Count; i++)
    //    {
    //        editButtonsCollection.Add(taskObjects[i].GetComponentInChildren<Button>());

    //        int index = i;
    //        editButtonsCollection[i].onClick.AddListener(delegate { editPanel.SetActive(true); });
    //        editButtonsCollection[i].onClick.AddListener(delegate { recycleBin.SetActive(true); });
    //        editButtonsCollection[i].onClick.AddListener(delegate { SwitchSaveButtonTo("EDIT"); });
    //        editButtonsCollection[i].onClick.AddListener(delegate { TransferDataForEdit(editButtonsCollection[index], taskObjects[index], names[index], rewards[index]); });
    //    }
    //}

    //public void TransferDataAfterEdit(GameObject taskObject, string name, int reward)
    //{
    //    addInputFields[0].text = name;
    //    addInputFields[1].text = reward.ToString();
    //}
    ///// <summary>
    ///// called when Edit button(attached to target task) is pressed;
    ///// transfers data from target task to edit panel
    ///// </summary>
    //public void TransferDataForEdit(Button editButton, GameObject taskObject, string name, int reward)
    //{
    //    addInputFields[0].text = name;
    //    addInputFields[1].text = reward.ToString();
    //    editButton.onClick.AddListener(delegate { });
    //    editSaveButton.onClick.AddListener(delegate { EditTask(taskObject); });
    //    deleteTaskButton.onClick.AddListener(delegate { DeleteTask(taskObject); });
    //}

    ///// <summary>
    ///// Called when SaveEdit button in edit panel is pressed;
    ///// Changes data in task that's edited
    ///// </summary>
    //public void EditTask(GameObject taskObject)
    //{
    //    Task task = taskObject.GetComponent<Task>();
    //    task.name = addInputFields[0].text;
    //    task.reward = int.Parse(addInputFields[1].text);
    //    task.taskText.text = addInputFields[0].text + $" {{{task.reward}}}";

    //    //Button editButton = taskObject.GetComponent<Button>();\
    //    //TransferDataForEdit(editButton, taskObject, task.name, task.reward); // like this?

    //    //when task is finished editing
    //    SwitchSaveButtonTo("SAVE");
    //    recycleBin.SetActive(false);
    //    editPanel.SetActive(false);
    //}

    ///// <summary>
    ///// Called when delete button pressed in edit panel of edit mode
    ///// </summary>
    //public void DeleteTask(GameObject taskObject)
    //{
    //    Task task = taskObject.GetComponent<Task>();
    //    tasks.Remove(task);
    //    Destroy(taskObject.gameObject);
    //    editPanel.SetActive(false);
    //    SwitchSaveButtonTo("SAVE");
    //    recycleBin.SetActive(false);
    //}
    //public void DeleteHistoryTask(GameObject historyTaskObject)
    //{
    //    Task historyTask = historyTaskObject.GetComponent<Task>();
    //    tasks.Remove(historyTask);
    //    Destroy(historyTask);
    //}
    ///// <summary>
    ///// Just type in string, which Save button(located in edit panel) mode you want: ("SAVE") or ("EDIT")
    ///// </summary>
    //public void SwitchSaveButtonTo(string saveMode)
    //{
    //    if (saveMode == "EDIT")
    //    {
    //        saveButtonObj.SetActive(false);
    //        editSaveButtonObj.SetActive(true);
    //    }
    //    else if (saveMode == "SAVE")
    //    {
    //        saveButtonObj.SetActive(true);
    //        editSaveButtonObj.SetActive(false);
    //    }
    //}
}
