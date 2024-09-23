# m_EditorTool

## Folder Organizer
Objects without folders are automatically transferred to the files you specify.
```C#
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
```

![FolderOrganizer](https://github.com/user-attachments/assets/7bf7843b-2fcc-442f-847e-6c8229e5b4ad)

<img src="https://github.com/user-attachments/assets/7bf7843b-2fcc-442f-847e-6c8229e5b4ad" width="400px" height="300px" />

## Create Folders
Automatic folder creation.

```C#
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
```
![createFolder](https://github.com/user-attachments/assets/740d2f9a-ba5f-45dc-b3be-546accbf6fb5)

## Add Package
You can quickly add packages you use frequently.

```C#
 private List<string> packages = new List<string>   // ADD PACKAGES HERE
    {
        "com.unity.recorder",
        "com.unity.cinemachine",
        "com.unity.2d.tilemap.extras",
        "com.unity.timeline",
        "com.unity.postprocessing"
    };

```

![package](https://github.com/user-attachments/assets/f57940e6-f991-490c-8d9c-d6d7ece4de8b)
