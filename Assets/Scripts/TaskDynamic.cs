using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskDynamic : MonoBehaviour
{
    public Text taskText;
    public RectTransform rectTransform;
    public float initialHeight;

    private void Start()
    {
        initialHeight = rectTransform.sizeDelta.y;
    }

    public void ChangeHeightOnTask()
    {
        float newHeight = initialHeight + taskText.preferredHeight;
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, newHeight);

        float yOffset = newHeight - initialHeight;
        int siblingIndex = transform.GetSiblingIndex();
        int siblingCount = transform.parent.childCount;
        for (int i = siblingIndex + 1; i < siblingCount; i++)
        {
            RectTransform siblingRectTransform = transform.parent.GetChild(i).GetComponent<RectTransform>();
            siblingRectTransform.anchoredPosition += new Vector2(0f, yOffset);
        }
    }



    //void Update()
    //{
        
    //}
}
