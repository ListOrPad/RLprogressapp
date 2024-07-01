using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class WorkspaceManager : MonoBehaviour
{
    public Workspace defaultWorkspace = new Workspace();
    private List<Workspace> workspaces = new List<Workspace>();
    private List<string> workspacesNames = new List<string>();

    private static string workspacesFilepath;
    private static WorkspaceManager instance; //why do i need this?

    [SerializeField] private Settings settings;
    [SerializeField] private Button OKButton;
    [SerializeField] private Button addButton;
    [SerializeField] private TMP_InputField workspaceInputField;
    [SerializeField] private GameObject workspacePrefab;
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject addMenu;
    [SerializeField] private GameObject deleteConfirmationMenu;
    [SerializeField] private TMP_Dropdown workspaceDropdown;

    private void Update()
    {

    }
    private void Awake()
    {
        workspacesFilepath = Path.Combine(Application.persistentDataPath, "_workspaces_list.json");
    }
    void Start()
    {
        LoadWorkspaceListFromJSON();

        if (!workspaces.Exists(w => w.name == defaultWorkspace.name))
        {
            workspaces.Add(defaultWorkspace);
            workspacesNames.Add(defaultWorkspace.name);
            SaveWorkspaceListToJSON();
        }

        UpdateWorkspaceDropdown();

        OKButton.onClick.AddListener(delegate { CreateWorkspace(); });
        addButton.onClick.AddListener(delegate { ToggleAddMenu(); });
    }

    private void CreateWorkspace()
    {
        if(workspaceInputField != null & !string.IsNullOrWhiteSpace(workspaceInputField.text))
        {
            Workspace workspace = new Workspace(workspaceInputField.text, 0);
            workspace.Initialize();
            GameObject workspaceObject = Instantiate(workspacePrefab, content.transform);
            workspaceObject.GetComponentInChildren<TextMeshProUGUI>().text = workspace.name;
            workspaces.Add(workspace);
            SaveWorkspaceListToJSON();
            workspacesNames.Add(workspace.name);

            //prepare workspace for possible future deleting
            workspaceObject.GetComponentInChildren<Button>().onClick.AddListener(() => { PrepareWorkspaceDelete(workspace, workspaceObject); });

            UpdateWorkspaceDropdown();
            addMenu.SetActive(false);
        }
        else
        {
            addMenu.SetActive(false);
        }
        
    }

    private void SaveWorkspaceListToJSON()
    {
        WorkspaceList workspaceList = new WorkspaceList();
        workspaceList.workspaces = workspaces;

        string json = JsonUtility.ToJson(workspaceList);
        File.WriteAllText(workspacesFilepath, json);
        Debug.Log("Saved workspaces to JSON: " + json);
    }

    private void LoadWorkspaceListFromJSON()
    {
        if (File.Exists(workspacesFilepath))
        {
            string json = File.ReadAllText(workspacesFilepath);

            if (!string.IsNullOrEmpty(json))
            {
                WorkspaceList data = JsonUtility.FromJson<WorkspaceList>(json);
                Debug.Log("Loaded from JSON: " + data);

                workspaces.Clear();
                workspaces.AddRange(data.workspaces);
                workspacesNames.Clear();
                foreach (var workspace in workspaces)
                {
                    GameObject workspaceObject = Instantiate(workspacePrefab, content.transform);
                    workspaceObject.GetComponentInChildren<TextMeshProUGUI>().text = workspace.name;
                    workspacesNames.Add(workspace.name);

                    //assign the delete button listener
                    workspaceObject.GetComponentInChildren<Button>().onClick.AddListener(() => { PrepareWorkspaceDelete(workspace, workspaceObject); });
                }
            }
            else
            {
                Debug.Log("JSON file is empty");
            }
        }
        else
        {
            Debug.Log("JSON file does not exist at path: " + workspacesFilepath);
        }
    }

    private void PrepareWorkspaceDelete(Workspace workspace, GameObject workspaceObject)
    {
        deleteConfirmationMenu.SetActive(true);
        deleteConfirmationMenu.GetComponentInChildren<TextMeshProUGUI>().text = $"Sure want to delete the workspace \"{workspace.name}\"? it may lead to data loss";
        Button yesButton = deleteConfirmationMenu.GetComponentInChildren<Button>();
        yesButton.onClick.RemoveAllListeners();
        yesButton.onClick.AddListener(delegate { DeleteWorkspace(workspace, workspaceObject); });
    }
    private void DeleteWorkspace(Workspace workspace, GameObject workspaceObject)
    {
        deleteConfirmationMenu.SetActive(false);
        Destroy(workspaceObject);
        workspaces.Remove(workspace);
        workspacesNames.Remove(workspace.name);
        SaveWorkspaceListToJSON();
        UpdateWorkspaceDropdown();
    }

    private void UpdateWorkspaceDropdown()
    {
        workspaceDropdown.ClearOptions();
        workspaceDropdown.AddOptions(workspacesNames);
        workspaceDropdown.value = workspacesNames.IndexOf(settings.GetWorkspace().name);
    }
    private void ToggleAddMenu()
    {
        // Toggle the active state of the Add Menu
        addMenu.SetActive(!addMenu.activeSelf);
    }
}

[System.Serializable]
public class WorkspaceList
{
    public List<Workspace> workspaces;
}