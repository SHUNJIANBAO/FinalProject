using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameObjectMenuTools 
{
    static string SceneGameObjectPath = "Assets/Editor/Demo/SceneObjects/";

    [MenuItem("GameObject/场景相关/创建存档点",false,20)]
    static void CreateSavePoint()
    {
        CreateGameObject("SavePoint");
    }
    [MenuItem("GameObject/场景相关/创建敌人")]
    static void CreateEnemyPoint()
    {
        CreateGameObject("EnemyPoint");
    }

    public static void CreateGameObject(string name)
    {
        GameObject curGo = Selection.activeGameObject;
        string objPath = SceneGameObjectPath + name + ".prefab";
        var obj = AssetDatabase.LoadAssetAtPath(objPath, typeof(GameObject));
        var go = GameObject.Instantiate(obj) as GameObject;
        go.transform.SetParent(curGo.transform);
    }
}
