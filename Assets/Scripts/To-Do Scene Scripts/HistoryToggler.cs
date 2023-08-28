using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HistoryToggler : MonoBehaviour
{
    [Header("History Toggle")]
    [SerializeField]
    private Toggle historyToggle;
    [SerializeField]
    private GameObject historyToggleObject;
    [SerializeField]
    private GameObject scrollViewHistory;

    GameObject scrollViewChecklist;

    private void Start()
    {
        scrollViewChecklist = GameObject.Find("Scroll View Checklist");
        historyToggle.onValueChanged.AddListener(delegate { ToggleHistoryVisibility(); });
    }

    private void ToggleHistoryVisibility()
    {
        //if history enables
        if (!scrollViewHistory.activeSelf)
        {
            MoveHistoryToggleUp();
            ExpandHistory();
            ToggleHistoryEnableSprite();
        }
        //if history disables
        else if (scrollViewHistory.activeSelf)
        {
            MoveHistoryToggleDown();
            CompressHistory();
            ToggleHistoryDisableSprite();
        }
    }

    private void MoveHistoryToggleUp()
    {
        historyToggleObject.GetComponent<RectTransform>().offsetMin = new Vector2(2f, 750f); //(left, bottom)
        historyToggleObject.GetComponent<RectTransform>().offsetMax = new Vector2(0f, 750f); //(-right, -top)
    }

    private void MoveHistoryToggleDown()
    {
        historyToggleObject.GetComponent<RectTransform>().offsetMin = new Vector2(1f, -1.5f); //(left, bottom)
        historyToggleObject.GetComponent<RectTransform>().offsetMax = new Vector2(0f, -0.5f); //(-right, -top)
    }

    private void ExpandHistory()
    {
        scrollViewHistory.SetActive(true);
        scrollViewChecklist.GetComponent<RectTransform>().offsetMin = new Vector2(-25f, 885.5f);
    }

    private void CompressHistory()
    {
        scrollViewHistory.SetActive(false);
        scrollViewChecklist.GetComponent<RectTransform>().offsetMin = new Vector2(-25f, 194f);
    }

    private void ToggleHistoryEnableSprite()
    {
        Sprite historyArrowEnabled = Resources.Load<Sprite>("Images/History Enabled Arrow");
        Image[] imageComponentsInToggle = historyToggleObject.GetComponentsInChildren<Image>();
        Image secondImageInToggle = imageComponentsInToggle[1];
        secondImageInToggle.sprite = historyArrowEnabled;
    }

    private void ToggleHistoryDisableSprite()
    {
        Sprite historyArrowDisabled = Resources.Load<Sprite>("Images/History Disabled Arrow");
        Image[] imageComponentsInToggle = historyToggleObject.GetComponentsInChildren<Image>();
        Image secondImageInToggle = imageComponentsInToggle[1];
        secondImageInToggle.sprite = historyArrowDisabled;
    }
}
