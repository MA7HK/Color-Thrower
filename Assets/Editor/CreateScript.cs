using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CreateScript: EditorWindow
{
    [MenuItem("Component/Scripts/New Script %&n")]
    public static void ShowWindow()
    {
        var window = EditorWindow.GetWindow(typeof(CreateScript));
        window.minSize = new Vector2(400, 80);
        window.maxSize = new Vector2(400, 80);
        window.Center();      
    }

    private bool autoFocus = true;
    private string scriptName = "NewScript";
    private string selectedFolder = "Assets\\SourceCode"; // Default folder

    private void OnGUI()
    {
        EditorGUI.BeginChangeCheck();
        GUI.SetNextControlName(scriptName);

        scriptName = EditorGUILayout.TextField("Script Name", scriptName);
        selectedFolder = EditorGUILayout.TextField("Folder", selectedFolder);

        if (GUILayout.Button("Select Folder"))
        {
            string path = EditorUtility.OpenFolderPanel("Select Folder", "", "");
            if (!string.IsNullOrEmpty(path))
            {
                selectedFolder = path;
            }
        }

        if (autoFocus)
        {
            EditorGUI.FocusTextInControl("NewScript");
            autoFocus = false;
        }

        if (GUILayout.Button("Create Script") || Event.current.keyCode == KeyCode.Return)
        {
            EditorGUI.EndChangeCheck();
            CreateNewScript();
        }
    }

private void CreateNewScript()
{
    // Check if the folder exists, if not, create it
    if (!System.IO.Directory.Exists(selectedFolder))
    {
        System.IO.Directory.CreateDirectory(selectedFolder);
    }

    string scriptPath = System.IO.Path.Combine(selectedFolder, scriptName + ".cs");

    string template = "using UnityEngine;\n\npublic class {0} : MonoBehaviour\n{{\n\t// Start is called before the first frame update\n\tvoid Start()\n\t{{\n\n\t}}\n\n\t// Update is called once per frame\n\tvoid Update()\n\t{{\n\n\t}}\n}}";
    string scriptContent = string.Format(template, scriptName.Replace(" ", ""), scriptName);

    System.IO.File.WriteAllText(scriptPath, scriptContent);
    AssetDatabase.Refresh();

    Object createdScript = AssetDatabase.LoadAssetAtPath(scriptPath, typeof(Object));
    Selection.activeObject = createdScript;
    EditorGUIUtility.PingObject(createdScript);

    // Open the newly created script in the default script editor
    AssetDatabase.OpenAsset(createdScript);

    Close();
}

}
