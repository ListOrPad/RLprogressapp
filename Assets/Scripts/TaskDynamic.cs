using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskDynamic : MonoBehaviour
{
    public GameObject taskPrefab;
    public RectTransform contentContainer;

    public void AddTask(string taskText)
    {
        GameObject taskObject = Instantiate(taskPrefab, contentContainer);

        Text taskTextComponent = taskObject.GetComponentInChildren<Text>();
        taskTextComponent.text = taskText;

        //вот до сюда все хорошо

        float yOffset = taskTextComponent.preferredHeight;
        int siblingIndex = taskObject.transform.GetSiblingIndex();
        int siblingCount = contentContainer.childCount;
        for (int i = siblingIndex + 1; i < siblingCount; i++)
        {
            RectTransform siblingRectTransform = contentContainer.GetChild(i).GetComponent<RectTransform>();
            siblingRectTransform.anchoredPosition += new Vector2(0f, yOffset);
        }
    }

    private void Start()
    {

    }
}
