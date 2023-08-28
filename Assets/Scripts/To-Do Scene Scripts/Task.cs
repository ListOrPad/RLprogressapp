using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Task : MonoBehaviour
{
    #region Legacy
    internal int numberOfTask;
    internal string taskOnlyName;
    public string taskFullName;
    public int reward;
    public int index;

    internal Text taskText;


    void Start()
    {
        taskText = GetComponent<Text>();
        taskText.text = taskFullName;
    }

    public void SetTaskInfo(string name, int reward, int index)
    {
        this.taskOnlyName = name;
        this.taskFullName = $"{taskOnlyName} {{{reward}}}";
        this.reward = reward;
        this.index = index;
    }
    public void SetHistoryTaskInfo(string name, int reward, int index)
    {
        this.taskOnlyName = name;
        this.taskFullName = $"{numberOfTask}. {taskOnlyName} {{{reward}}}";
        this.reward = reward;
        this.index = index;
    }
    #endregion
}
