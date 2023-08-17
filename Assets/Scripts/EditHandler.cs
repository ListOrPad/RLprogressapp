using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditHandler : MonoBehaviour
{
    public EditClickedEvent OnEditClicked;

    public void OnEditClick()
    {
        GameObject taskObject = GetComponent<GameObject>();
        OnEditClicked.Invoke(taskObject);
    }
}
