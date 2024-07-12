using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class WorkspaceManager : MonoBehaviour
{
    public List<Workspace> workspaces = new List<Workspace>();

    public List<string> WorkspacesNames { get { return workspacesNames; } }
    private List<string> workspacesNames = new List<string>();

    public Workspace DefaultWorkspace {get { return defaultWorkspace; } }
    private Workspace defaultWorkspace = new Workspace();

    private string workspacesFilepath;

    [SerializeField] private Settings settings;
    [SerializeField] private Button OKButton;
    [SerializeField] private Button addButton;
    [SerializeField] private TMP_InputField workspaceInputField;
    [SerializeField] private GameObject workspacePrefab;
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject addMenu;
    [SerializeField] private GameObject deleteConfirmationMenu;
    [SerializeField] private TMP_Dropdown workspaceDropdown;

    private void Awake()
    {
        workspacesFilepath = Path.Combine(Application.persistentDataPath, "_workspaces_list.json");
    }
    void Start()
    {
        LoadWorkspaceListFromJSON();

        if (!workspaces.Exists(w => w.Name == DefaultWorkspace.Name))
        {
            workspaces.Add(DefaultWorkspace);
            workspacesNames.Add(DefaultWorkspace.Name);
            SaveWorkspaceListToJSON();
        }

        UpdateWorkspaceDropdown();

        OKButton.onClick.AddListener(delegate { CreateWorkspace(); });
        addButton.onClick.AddListener(delegate { ToggleAddMenu(); });
    }

    private void CreateWorkspace()
    {
        if(workspaceInputField != null && !string.IsNullOrWhiteSpace(workspaceInputField.text))
        {
            Workspace workspace = new Workspace(workspaceInputField.text);
            workspace.Initialize();
            GameObject workspaceObject = Instantiate(workspacePrefab, content.transform);
            workspaceObject.GetComponentInChildren<TextMeshProUGUI>().text = workspace.Name;
            workspaces.Add(workspace);
            SaveWorkspaceListToJSON();
            workspacesNames.Add(workspace.Name);

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

        string json = JsonConvert.SerializeObject(workspaceList);
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
                WorkspaceList data = JsonConvert.DeserializeObject<WorkspaceList>(json);
                Debug.Log("Loaded from JSON: " + data);

                workspaces.Clear();
                workspaces.AddRange(data.workspaces);
                workspacesNames.Clear();
                foreach (var workspace in workspaces)
                {
                    GameObject workspaceObject = Instantiate(workspacePrefab, content.transform);
                    workspaceObject.GetComponentInChildren<TextMeshProUGUI>().text = workspace.Name;
                    workspacesNames.Add(workspace.Name);

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
        deleteConfirmationMenu.GetComponentInChildren<TextMeshProUGUI>().text = $"Sure want to delete the workspace \"{workspace.Name}\"? it may lead to data loss";
        Button yesButton = deleteConfirmationMenu.GetComponentInChildren<Button>();
        yesButton.onClick.RemoveAllListeners();
        yesButton.onClick.AddListener(delegate { DeleteWorkspace(workspace, workspaceObject); });
    }
    private void DeleteWorkspace(Workspace workspace, GameObject workspaceObject)
    {
        deleteConfirmationMenu.SetActive(false);
        Destroy(workspaceObject);
        workspaces.Remove(workspace);
        workspacesNames.Remove(workspace.Name);
        SaveWorkspaceListToJSON();
        UpdateWorkspaceDropdown();
    }

    private void UpdateWorkspaceDropdown()
    {
        workspaceDropdown.ClearOptions();
        workspaceDropdown.AddOptions(workspacesNames);
        workspaceDropdown.value = workspacesNames.IndexOf(settings.GetWorkspace().Name);
    }
    private void ToggleAddMenu()
    {
        // Change to opposite the active state of the Add Menu
        addMenu.SetActive(!addMenu.activeSelf);
    }
}

[System.Serializable]
public class WorkspaceList
{
    public List<Workspace> workspaces;
}