using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Button = UnityEngine.UIElements.Button;
using Toggle = UnityEngine.UIElements.Toggle;

public class SetupFolderAndPackageTool : EditorWindow
{
    #region Package Variables
   
    private List<string> packages = new List<string>   // ADD PACKAGES HERE
    {
        "com.unity.recorder",
        "com.unity.cinemachine",
        "com.unity.2d.tilemap.extras",
        "com.unity.timeline",
        "com.unity.postprocessing"
    };

    private List<bool> packageSelections = new List<bool>();
    private int currentPackageIndex = 0;
    private AddRequest packageRequest;

    #endregion

    [MenuItem("m_EditorTools/Fast Setup", false, 0)]
    private static void ShowWindow()
    {
        var window = GetWindow<SetupFolderAndPackageTool>();
        window.titleContent = new GUIContent("Fast Setup");
        window.Show();
    }

    private void OnEnable()
    {
        foreach (var package in packages)
        {
            packageSelections.Add(false);
        }
    }

    private void CreateGUI()
    {
        var root = rootVisualElement;

        #region FOLDER CREATION
        
        var folderCreationPanel = new VisualElement();
        folderCreationPanel.AddToClassList("folder-creation-panel");
        var folderCreationLabel = new Label("Folder Creation");
        folderCreationLabel.AddToClassList("panel-title");
        folderCreationPanel.Add(folderCreationLabel);
        
        Edit.EditPanel(folderCreationPanel);
        Edit.EditLabel(folderCreationLabel);
    
        var createFoldersButton = new Button(() => { CreateFolders(); }) { text = "Create Folder Structure" };
        
        Edit.EditButton(createFoldersButton,200,50);
        folderCreationPanel.Add(createFoldersButton);
        root.Add(folderCreationPanel);

        #endregion
        
        #region PACKAGE SELECTION

        var packageSelectionPanel = new VisualElement();
        packageSelectionPanel.AddToClassList("package-selection-panel");
        var packageSelectionLabel = new Label("Package Selection");
        packageSelectionLabel.AddToClassList("panel-title");
        packageSelectionPanel.Add(packageSelectionLabel);
        
        Edit.EditPanel(packageSelectionPanel);
        Edit.EditLabel(packageSelectionLabel);
    
        for (int i = 0; i < packages.Count; i++)
        {
            var toggle = new Toggle(packages[i])
            {
                value = packageSelections[i]
            };
            int index = i;
            toggle.RegisterValueChangedCallback(evt =>
            {
                packageSelections[index] = evt.newValue;
            });
            packageSelectionPanel.Add(toggle);
        }
    
        var importButton = new Button(() => { currentPackageIndex = 0; ImportNextPackage(); }) { text = "Import Selected Packages" };
        
        Edit.EditButton(importButton,200,50);
        packageSelectionPanel.Add(importButton);
        root.Add(packageSelectionPanel);

        #endregion
    }
    
    private void ImportNextPackage()
    {
        while (currentPackageIndex < packages.Count && !packageSelections[currentPackageIndex])
        {
            currentPackageIndex++;
        }

        if (currentPackageIndex < packages.Count)
        {
            var package = packages[currentPackageIndex];
            Debug.Log($"Importing package: {package}");
            packageRequest = Client.Add(package);
            EditorApplication.update += Progress;
        }
        else
        {
            Debug.Log("All selected packages imported successfully.");
        }
    }

    private void Progress()
    {
        if (packageRequest.IsCompleted)
        {
            if (packageRequest.Status == StatusCode.Success)
            {
                Debug.Log($"Installed: {packageRequest.Result.packageId}");
            }
            else if (packageRequest.Status >= StatusCode.Failure)
            {
                Debug.LogError(packageRequest.Error.message);
            }

            EditorApplication.update -= Progress;
            currentPackageIndex++;
            ImportNextPackage();
        }
    }

    private void CreateFolders()
    {
        var folders = new List<string>  // ADD FOLDERS HERE
        {
            "ThirdParty",
            "Animations",
            "Prefabs",
            "Scenes",
            "Data",
            "Data/SO",
            "Scripts",
            "Scripts/Utility",
            "Scripts/Managers",
            "Scripts/Signals",
            "Scripts/Enums",
            "Scripts/Controllers",
            "Scripts/Interfaces",
            "Scripts/Data",
            "Scripts/Data/SO",
            "Scripts/UI",
        };

        foreach (var folder in folders)
        {
            CreateFolderStructure(folder);
        }
    }

    private void CreateFolderStructure(string folder)
    {
        var subFolders = folder.Split('/');
        var currentPath = "Assets";
        foreach (var subFolder in subFolders)
        {
            var targetPath = $"{currentPath}/{subFolder}";
            if (!AssetDatabase.IsValidFolder(targetPath))
            {
                AssetDatabase.CreateFolder(currentPath, subFolder);
            }
            currentPath = targetPath;
        }
    }
}

public class Edit : EditorWindow
{
    public static void EditLabel(Label label)
    {
        label.style.color = Color.yellow;
        label.style.fontSize = 20;
        label.style.width = Length.Percent(100);
        label.style.unityTextAlign = TextAnchor.MiddleCenter;
        label.style.backgroundColor = new StyleColor(Color.blue); 
    }
    
    public static void EditPanel(VisualElement panel)
    {
        panel.style.flexDirection = FlexDirection.Column;
        panel.style.alignItems = Align.Center;
        panel.style.justifyContent = Justify.Center;
        panel.style.marginBottom = 10;
        panel.style.marginTop = 20;
    }

    public static void EditButton(Button button, int width, int height)
    {
        button.style.width = width; 
        button.style.height = height; 
    }
}
public class FolderOrganizer : EditorWindow
{
    [MenuItem("m_EditorTools/Folder Organizer", false, 1)]
    public static void ShowWindow()
    {
        FolderOrganizer window = GetWindow<FolderOrganizer>("Folder Organizer");
        window.minSize = new Vector2(250, 100);
    }

    private void OnEnable()
    {
        var root = rootVisualElement;

        #region REMOVE EMPTY FOLDERS

        var removeEmptyFoldersPanel = new VisualElement();
        removeEmptyFoldersPanel.AddToClassList("folder-creation-panel");
        var removeEmptyFoldersLabel = new Label("Remove Empty Folders");
        removeEmptyFoldersLabel.AddToClassList("panel-title");
        removeEmptyFoldersPanel.Add(removeEmptyFoldersLabel);
        
        Edit.EditLabel(removeEmptyFoldersLabel);
        Edit.EditPanel(removeEmptyFoldersPanel);
        root.Add(removeEmptyFoldersLabel);
        
        Button removeEmptyFoldersButton = new Button(RemoveEmptyFolders) { text = "Remove Empty Folders" };
        root.Add(removeEmptyFoldersButton);
        
        Edit.EditButton(removeEmptyFoldersButton, 200, 50);
        removeEmptyFoldersPanel.Add(removeEmptyFoldersButton);
        root.Add(removeEmptyFoldersPanel);

        #endregion

        #region ORGANIZATION ASSETS
        
        var organizationAssetsPanel = new VisualElement();
        organizationAssetsPanel.AddToClassList("folder-creation-panel");
        var organizationAssetsLabel = new Label("Organization Assets");
        organizationAssetsLabel.AddToClassList("panel-title");
        organizationAssetsPanel.Add(organizationAssetsLabel);
        
        Edit.EditLabel(organizationAssetsLabel);
        Edit.EditPanel(organizationAssetsPanel);
        root.Add(organizationAssetsLabel);
                
        var organizeButton = new Button(OrganizeAssets) { text = "Organize Assets" };
        root.Add(organizeButton);
        
        Edit.EditButton(organizeButton, 200, 50);
        organizationAssetsPanel.Add(organizeButton);
        root.Add(organizationAssetsPanel);

        #endregion
    }

    private void RemoveEmptyFolders()
    {
        string[] allFolders = AssetDatabase.GetAllAssetPaths();
        foreach (string folder in allFolders)
        {
            if (AssetDatabase.IsValidFolder(folder) && AssetDatabase.FindAssets("", new[] { folder }).Length == 0)
            {
                AssetDatabase.DeleteAsset(folder);
                Debug.Log("Deleted Empty Folder: " + folder);
                RemoveEmptyFolders(); 
                return; 
            }
        }
    }
    
    private void OrganizeAssets() // ASSETS ORGANIZATION
    {
        var folderMappings = new Dictionary<string, string>
        {
            { ".wav", "Assets/Audio" },
            { ".mp3", "Assets/Audio" },
            { ".png", "Assets/Textures" },
            { ".jpg", "Assets/Textures" },
            { ".cs", "Assets/Scripts" },
            { ".prefab", "Assets/Prefabs" },
            { ".asset", "Assets/Data/SO" },
            { ".unity", "Assets/Scenes" }
        };

        foreach (var folder in folderMappings.Values.Distinct())
        {
            CreateFolder(folder);
        }

        string[] allAssets = AssetDatabase.GetAllAssetPaths();

        foreach (string assetPath in allAssets)
        {
            if (assetPath.StartsWith("Assets/") && assetPath.Count(c => c == '/') == 1)
            {
                string extension = Path.GetExtension(assetPath);
                if (folderMappings.TryGetValue(extension, out string targetFolder))
                {
                    MoveAsset(assetPath, targetFolder);
                }
            }
        }

        AssetDatabase.Refresh();
        Debug.Log("Assets organized successfully.");
    }

    private void CreateFolder(string folderPath)
    {
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.CreateFolder(Path.GetDirectoryName(folderPath), Path.GetFileName(folderPath));
            Debug.Log("Created folder: " + folderPath);
        }
    }

    private void MoveAsset(string assetPath, string targetFolder)
    {
        string fileName = Path.GetFileName(assetPath);
        string newAssetPath = Path.Combine(targetFolder, fileName);

        if (AssetDatabase.MoveAsset(assetPath, newAssetPath) == "")
        {
            Debug.Log("Moved asset: " + fileName + " to " + targetFolder);
        }
    }

}
