using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

internal class CreateDefaultFolders
{
    [MenuItem("Assets/Create/Default Folders")]
    private static void CreateFolders()
    {
        CreateDirectory("Editor");
        CreateDirectory("Game");
        CreateDirectory("Gizmos");
        CreateDirectory("Plugins");
        CreateDirectory("Resources");
        CreateDirectory("Tests");
        
        CreateDirectory("Game/Animations");
        CreateDirectory("Game/Audio");
        CreateDirectory("Game/Fonts");
        CreateDirectory("Game/Materials");
        CreateDirectory("Game/Models");        
        CreateDirectory("Game/Prefabs");
        CreateDirectory("Game/Scenes");       
        CreateDirectory("Game/Scripts");
        CreateDirectory("Game/Shaders");        
        CreateDirectory("Game/Textures");
       
        AssetDatabase.Refresh();
    }

    private static void CreateDirectory(string name)
    {
        string path = Path.Combine(Application.dataPath, name);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
}
