using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorkspaceManager : MonoBehaviour
{
    private Workspace defaultWorkspace = new Workspace("Default Workspace", 0);
    private List<Workspace> workspaces = new List<Workspace>();
    private List<string> workspacesNames = new List<string>();
    [SerializeField] private TMP_Dropdown workspaceDropdown;
    [SerializeField] private Button OKButton;
    [SerializeField] private Button addButton;
    [SerializeField] private TMP_InputField workspaceInputField;
    [SerializeField] private GameObject workspacePrefab;
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject addMenu;
    [SerializeField] private GameObject deleteConfirmationMenu;

    void Start()
    {
        if (!workspaces.Contains(defaultWorkspace))
        {
            workspaces.Add(defaultWorkspace);
            workspacesNames.Add(defaultWorkspace.name);
        }
        workspaceDropdown.ClearOptions();
        workspaceDropdown.AddOptions(workspacesNames);
        OKButton.onClick.AddListener(delegate { CreateWorkspace(); });
        addButton.onClick.AddListener(delegate { ToggleAddMenu(); });
    }

    private void CreateWorkspace()
    {
        if(workspaceInputField != null)
        {
            Workspace workspace = new Workspace(workspaceInputField.text, 0);
            GameObject workspaceObject = Instantiate(workspacePrefab, content.transform);
            workspaceObject.GetComponentInChildren<TextMeshProUGUI>().text = workspace.name;
            workspaces.Add(workspace);
            workspacesNames.Add(workspace.name);
            //update Workspace Dropdown in settings 
            workspaceDropdown.ClearOptions();
            workspaceDropdown.AddOptions(workspacesNames);
            //prepare workspace for deleting
            workspaceObject.GetComponentInChildren<Button>().onClick.AddListener(() => { PrepareWorkspaceDelete(workspace, workspaceObject); });
            addMenu.SetActive(false);
        }
        else
        {
            addMenu.SetActive(false);
        }
        
    }
    private void PrepareWorkspaceDelete(Workspace workspace, GameObject workspaceObject)
    {
        deleteConfirmationMenu.SetActive(true);
        deleteConfirmationMenu.GetComponentInChildren<TextMeshProUGUI>().text = $"Sure want to delete the workspace \"{workspace.name}\"? it may lead to data loss";
        Button yesButton = deleteConfirmationMenu.GetComponentInChildren<Button>();
        yesButton.onClick.AddListener(delegate { DeleteWorkspace(workspace, workspaceObject); });
    }
    private void DeleteWorkspace(Workspace workspace, GameObject workspaceObject)
    {
        deleteConfirmationMenu.SetActive(false);
        Destroy(workspaceObject);
        workspaces.Remove(workspace);
        workspacesNames.Remove(workspace.name);
        workspaceDropdown.ClearOptions();
        workspaceDropdown.AddOptions(workspacesNames);
    }
    private void ToggleAddMenu()
    {
        // Toggle the active state of the Add Menu
        addMenu.SetActive(!addMenu.activeSelf);
    }
}
