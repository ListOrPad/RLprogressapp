using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskCreator : MonoBehaviour
{
    [SerializeField]
    private Transform content;
    [SerializeField]
    private GameObject editPanel;
    [SerializeField]
    private GameObject checklistItemPrefab;
    [SerializeField]
    private Button saveButton;

    private string filepath;

    private TMP_InputField[] addInputFields;

    private List<Task> tasks = new List<Task>();
    internal List<GameObject> taskObjects = new List<GameObject>();
    void Start()
    {
        filepath = Application.persistentDataPath + "/checklist.txt";
        addInputFields = editPanel.GetComponentsInChildren<TMP_InputField>();

        saveButton.onClick.AddListener(delegate { CreateTask(addInputFields[0].text, TryParseInput(addInputFields[1].text)); });
    }
    private void CreateTask(string name, int reward)
    {
        if (name == "")
        {
            editPanel.SetActive(false);
            return;
        }

        GameObject taskObject = Instantiate(checklistItemPrefab, content);
        taskObject.transform.SetSiblingIndex(0);

        Task task = taskObject.GetComponent<Task>();
        int index = 0;
        if (tasks.Count > 0)
            index = tasks.Count - 1;
        task.SetTaskInfo(name, reward, index);
        tasks.Add(task);

        TranslateToHistory(task);

        editPanel.SetActive(false);
    }

    /// <summary>
    /// transfer data to history object on task check
    /// </summary>
    private void TranslateToHistory(Task task)
    {
        ///maybe should be in TaskChecker class?
        TaskChecker checker;
        task.GetComponent<Toggle>().onValueChanged.AddListener(delegate { checker.CheckTask(task); });
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
}
