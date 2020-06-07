using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class CreateSceneTool : EditorWindow
{
    string _levelId;
    string _sceneName;

    static string SceneGameObjectPath = "Assets/Editor/Demo/Objects/";

    [MenuItem("工具/创建新场景")]
    static void OpenWindow()
    {
        var window = GetWindow<CreateSceneTool>();
        window.titleContent = new GUIContent("创建新场景");
        window.Show();
    }

    private void OnGUI()
    {
        _levelId = EditorGUILayout.TextField("场景Id", _levelId);

        if (GUILayout.Button("创建"))
        {
            CreateScene();
        }
    }

    void CreateScene()
    {
        _sceneName = "Level_" + _levelId;
        string scenePath = PathManager.ScenesPath + _sceneName+".unity";

        var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects,NewSceneMode.Single);
        CreateGameObject("SceneManager");
        FindObjectOfType<Camera>().orthographicSize = 5;
        bool value = EditorSceneManager.SaveScene(scene, scenePath);
        Debug.Log(value);
    }




    [MenuItem("GameObject/场景相关/创建存档点", false, 20)]
    static void CreateSavePoint()
    {
        var go = CreateGameObject("SavePoint", Selection.activeGameObject.transform);
        go.name = "存档点";
    }
    [MenuItem("GameObject/场景相关/创建敌人出生点")]
    static void CreateEnemyPoint()
    {
        var go = CreateGameObject("EnemyPoint", Selection.activeGameObject.transform);
        go.name = "出怪点";
    }
    [MenuItem("GameObject/场景相关/创建玩家初始点", false, 20)]
    static void CreatePlayerPoint()
    {
        var go = CreateGameObject("PlayerPoint", Selection.activeGameObject.transform);
        go.name = "玩家初始点";
    }
    [MenuItem("GameObject/场景相关/创建传送门")]
    static void CreatePortal()
    {
        var go = CreateGameObject("PortalPoint", Selection.activeGameObject.transform);
        go.name = "传送门";
    }

    [MenuItem("GameObject/场景相关/创建阻挡门")]
    static void CreateDoor()
    {
        var go = CreateGameObject("Door", Selection.activeGameObject.transform);
        go.name = "阻挡门";
    }

    [MenuItem("GameObject/场景相关/创建钥匙")]
    static void CreateKey()
    {
        var go = CreateGameObject("Key", Selection.activeGameObject.transform);
        go.name = "钥匙";
    }

    public static GameObject CreateGameObject(string name,Transform parent=null)
    {
        string objPath = SceneGameObjectPath + name + ".prefab";
        var obj = AssetDatabase.LoadAssetAtPath(objPath, typeof(GameObject));
        var go = GameObject.Instantiate(obj) as GameObject;
        go.name = name;
        if (parent!=null)
        {
            go.transform.SetParent(parent,false);
            go.transform.localPosition = Vector3.zero;
        }
        Selection.activeGameObject = go;
        return go;
    }

}
