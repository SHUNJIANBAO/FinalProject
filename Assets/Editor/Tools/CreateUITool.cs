using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class CreateUITool : EditorWindow
{
    [MenuItem("工具/创建UI")]
    static void OpenWindow()
    {
        var window = GetWindow<CreateUITool>();
        window.titleContent = new GUIContent("创建UI工具");
    }
    public enum UIType
    {
        Window,
        //Panel
    }

    string m_WindowPrefabModelPath= "Assets/Editor/Demo/UGUI/WindowDemo.prefab";
    string m_WindowScriptModelPath = "Assets/Editor/Demo/UGUI/WindowDemo.cs";
    string m_WindowScriptContent;
    private void Awake()
    {
        m_WindowScriptContent = File.ReadAllText(m_WindowScriptModelPath);
    }


    string uiName;
    UIType type;
    string uiTypeName;
    private void OnGUI()
    {
        uiName = EditorGUILayout.TextField("UI名称", uiName);
        type = (UIType)EditorGUILayout.EnumPopup("UI类型", type);
        uiTypeName = "UI" + uiName + "Window";
        if (!string.IsNullOrEmpty(uiName))
        {
            Type classType = Util.GetType(uiTypeName);
            if (classType == null)
            {
                if (GUILayout.Button("创建脚本"))
                {
                    //uiName = "UI" + uiName + "Window";
                    if (string.IsNullOrEmpty(uiName)) return;
                    EditorUtility.DisplayProgressBar("创建UI脚本", "正在创建文件夹... ...", 0);
                    CreateDirectory(type, uiTypeName);
                    EditorUtility.DisplayProgressBar("创建UI脚本", "正在生成脚本... ...", 0.5f);
                    CreateWindowScript(uiTypeName, scriptPath);
                    EditorUtility.DisplayProgressBar("创建UI脚本", "生成完毕", 1);
                    AssetDatabase.Refresh();
                    EditorUtility.ClearProgressBar();
                }
            }
            else if (WindowIsNull(uiTypeName)) 
            {
                if (GUILayout.Button("创建UI物体"))
                {
                    if (string.IsNullOrEmpty(uiTypeName)) return;
                    CreateDirectory(type, uiTypeName);
                    CreateWindowPrefab(uiTypeName, prefabPath, uiTypeName);
                    AssetDatabase.Refresh();
                }
            }
        }

    }


    string prefabPath;
    string scriptPath;

    void CreateDirectory(UIType type, string uiName)
    {
        CreateDirectory(type, uiName, out prefabPath, out scriptPath);

    }

    void CreateDirectory(UIType type, string uiName, out string prefabPath, out string scriptPath)
    {
        prefabPath = "";
        scriptPath = "";
        switch (type)
        {
            case UIType.Window:
                prefabPath = PathManager.GetWindowAssetsPath(uiName);
                scriptPath = PathManager.GetWindowScriptPath(uiName);
                break;
        }
        if (!Directory.Exists(prefabPath))
        {
            Directory.CreateDirectory(prefabPath);
        }
        if (!Directory.Exists(scriptPath))
        {
            Directory.CreateDirectory(scriptPath);
        }
    }

    void CreateWindowScript(string uiName,string scriptPath)
    {
        string scriptAssetPath= scriptPath+"/"+ uiName + ".cs";
        string content = m_WindowScriptContent.Replace("WindowDemo", uiName);
        File.WriteAllText(scriptAssetPath, content,System.Text.Encoding.UTF8);
    }

    void CreateWindowPrefab(string uiName,string prefabPath,string scriptName)
    {
        string prefabAssetPath = prefabPath + "/" + uiName + ".prefab";
        var obj= AssetDatabase.LoadAssetAtPath(m_WindowPrefabModelPath, typeof(GameObject));
        GameObject go= PrefabUtility.InstantiatePrefab(obj) as GameObject;
        PrefabUtility.DisconnectPrefabInstance(go);
        go.transform.SetParent(UIManager.Instance.MainCanvas.transform, false);
        go.name = uiName;
        Type type = Util.GetType(scriptName);
        go.AddComponent(type);
        var createGo = PrefabUtility.CreatePrefab(prefabAssetPath, go);
        PrefabUtility.ConnectGameObjectToPrefab(go, createGo);
    }

    bool WindowIsNull(string uiName)
    {
        string path = PathManager.GetWindowPath(uiName);
        var obj= Resources.Load(path);
        return obj==null;
    }
}
