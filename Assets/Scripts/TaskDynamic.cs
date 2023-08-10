using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TaskDynamic : MonoBehaviour
{
    private Text textMeshPro;
    public ChecklistManager checklistManager;


    private float previousTextHeight;
    private VerticalLayoutGroup layoutGroup;
    private ContentSizeFitter contentSizeFitter;

    void Start()
    {
        layoutGroup = GetComponent<VerticalLayoutGroup>();
        contentSizeFitter = GetComponent<ContentSizeFitter>();

        previousTextHeight = CalculateTextHeight();
    }

    void Update()
    {
        float currentTextHeight = CalculateTextHeight();


        if (!Mathf.Approximately(currentTextHeight, previousTextHeight))
        {
            MoveObjectsBelowText(currentTextHeight);
        }

        previousTextHeight = currentTextHeight;
    }

    private float CalculateTextHeight()
    {
        try
        {
            //assigns the last added task to the 'textMeshPro' var
            textMeshPro = checklistManager.tasks[^1].GetComponentInChildren<Text>();
            return textMeshPro.preferredHeight;
        }
        catch
        {
            Debug.Log("I catched it!");
            return 0;
        }

    }

    private void MoveObjectsBelowText(float currentTextHeight)
    {
        foreach (GameObject task in checklistManager.tasks)
        {
            RectTransform taskRectTransform = task.GetComponent<RectTransform>();
            taskRectTransform.anchoredPosition -= new Vector2(0f, currentTextHeight - previousTextHeight);
        }
    }

    private void UpdateLayout(float currentTextHeight)
    {
        //float newPreferredHeight = layoutGroup.preferredHeight + currentTextHeight - previousTextHeight;
        //layoutGroup.minHeight = newPreferredHeight;
        //layoutGroup.preferredHeight = newPreferredHeight;
    }
}
