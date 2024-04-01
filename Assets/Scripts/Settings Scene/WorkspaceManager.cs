using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorkspaceManager : MonoBehaviour
{
    private List<string> workspaces;
    [SerializeField] private Dropdown workspaceDropdown;
    [SerializeField] private GameObject addButtonObject;
    [SerializeField] private Button OKButton;
    [SerializeField] private TMP_InputField workspaceInputField;
    [SerializeField] private GameObject workspacePrefab;
    [SerializeField] private GameObject content;

    void Start()
    {
        workspaceDropdown.AddOptions(workspaces);
        OKButton.onClick.AddListener(delegate { CreateWorkspace(); });
    }

    private void CreateWorkspace()
    {
        GameObject workspaceObject = Instantiate(workspacePrefab, content.transform);
        workspaces.Add(workspaceInputField.text);
    }
}
