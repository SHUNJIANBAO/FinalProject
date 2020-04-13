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
        CreateGameObject("SavePoint", Selection.activeGameObject.transform);
    }
    [MenuItem("GameObject/场景相关/创建敌人出生点")]
    static void CreateEnemyPoint()
    {
        CreateGameObject("EnemyPoint", Selection.activeGameObject.transform);
    }
    [MenuItem("GameObject/场景相关/创建玩家初始点", false, 20)]
    static void CreatePlayerPoint()
    {
        CreateGameObject("PlayerPoint", Selection.activeGameObject.transform);
    }
    [MenuItem("GameObject/场景相关/创建传送门")]
    static void CreatePortal()
    {
        CreateGameObject("PortalPoint", Selection.activeGameObject.transform);
    }

    public static void CreateGameObject(string name,Transform parent=null)
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
    }

}
