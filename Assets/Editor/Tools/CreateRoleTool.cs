using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.IO;

public class CreateRoleTool : EditorWindow
{
    enum RoleType
    {
        小怪,
        Boss,
        主角,
    }

    GUIContent _roleNameTitle;
    string _roleName;
    RoleType _roleType;

    string _animatorControllerPath = "Assets/Editor/Demo/AnimatorController/AnimatorControllerDemo.controller";

    [MenuItem("工具/创建角色")]
    static void OpenWindow()
    {
        var window = GetWindow<CreateRoleTool>();
        window.titleContent = new GUIContent("创建角色");
        window.Show();
    }

    private void OnEnable()
    {
        _roleNameTitle = new GUIContent("角色名称");
    }


    private void OnGUI()
    {
        _roleName = EditorGUILayout.TextField(_roleNameTitle, _roleName);
        _roleType = (RoleType)EditorGUILayout.EnumPopup("角色类型", _roleType);

        if (GUILayout.Button("创建"))
        {
            if (string.IsNullOrEmpty(_roleName))
            {
                Debug.LogError("角色名不能为空！！！");
                return;
            }
            CreateRole(_roleName);
        }
    }

    void CreateRole(string name)
    {
        CreatTargetDirectory(name);

        string savePath = GetNewAnimatorPath(name);
        var controller = CreateAnimatorController(GetAnimatorPath(name), savePath);

        GameObject go = new GameObject(name);
        Rigidbody2D rigi = null;
        rigi= go.GetComponent<Rigidbody2D>();
        if (rigi==null)
        {
            rigi = go.AddComponent<Rigidbody2D>();
        }
        switch (_roleType)
        {
            case RoleType.小怪:
                go.AddComponent<Monster>();
                rigi.bodyType = RigidbodyType2D.Kinematic;
                break;
            case RoleType.Boss:
                rigi.bodyType = RigidbodyType2D.Kinematic;
                go.AddComponent<Boss>();
                break;
            case RoleType.主角:
                rigi.gravityScale = 4;
                rigi.constraints = RigidbodyConstraints2D.FreezeRotation;
                go.AddComponent<Player>();
                break;
            default:
                go.AddComponent<Character>();
                break;
        }
        var sr = go.GetComponent<SpriteRenderer>();
        sr.sprite = GetRoleIdleSprite(name);
        sr.sortingOrder = 10;
        go.GetComponent<BoxCollider2D>().size = Vector2.one * 2;
        go.GetComponent<Animator>().runtimeAnimatorController = controller;
        go.AddComponent<CharacterMovement>();

        GameObject prefab = PrefabUtility.CreatePrefab(GetNewPrefabPath(name), go);
        PrefabUtility.ConnectGameObjectToPrefab(go, prefab);
        //Object.DestroyImmediate(go, true);
    }

    AnimatorController CreateAnimatorController(string animPath, string newControllerPath)
    {
        //AssetDatabase.CopyAsset(_animatorControllerPath, newControllerPath);
        if (File.Exists(newControllerPath))
        {
            File.Delete(newControllerPath);
        }
        File.Copy(_animatorControllerPath, newControllerPath);
        AssetDatabase.Refresh();
        AnimatorController controller = AssetDatabase.LoadAssetAtPath(newControllerPath, typeof(AnimatorController)) as AnimatorController;

        string[] clipPathArry = System.IO.Directory.GetFiles(animPath);
        List<AnimationClip> clipList = new List<AnimationClip>();
        for (int i = 0; i < clipPathArry.Length; i++)
        {
            if (clipPathArry[i].EndsWith(".meta")) continue;
            var clip = AssetDatabase.LoadAssetAtPath(clipPathArry[i], typeof(AnimationClip)) as AnimationClip;
            clipList.Add(clip);
        }

        var stateArray = controller.layers[0].stateMachine.states;
        for (int i = 0; i < stateArray.Length; i++)
        {
            for (int j = 0; j < clipList.Count; j++)
            {
                string stateName = clipList[j].name.Split('_')[1];
                if (stateName.ToUpperInvariant() == stateArray[i].state.name.ToUpperInvariant())
                {
                    stateArray[i].state.motion = clipList[j];
                }
            }
        }
        return controller;
    }

    /// <summary>
    /// 创建目标文件夹
    /// </summary>
    /// <param name="name"></param>
    void CreatTargetDirectory(string name)
    {
        string newPath = "Assets/Resources/Prefabs/Roles/" + name + "/";
        if (!Directory.Exists(newPath))
        {
            Directory.CreateDirectory(newPath);
        }
        AssetDatabase.Refresh();
    }

    string GetAnimatorPath(string name)
    {
        return "Assets/ArtResources/Animator/Roles/" + name + "/";
    }

    string GetNewAnimatorPath(string name)
    {
        return "Assets/Resources/Prefabs/Roles/" + name + "/" + name + "AnimatorController.controller";
    }

    string GetNewPrefabPath(string name)
    {
        return "Assets/Resources/Prefabs/Roles/" + name + "/" + name + ".prefab";
    }

    Sprite GetRoleIdleSprite(string name)
    {
        string spritePath = "Assets/ArtResources/ArtSources/Roles/" + name + "/Idle/" + name + "_Idle_00001.png";
        Sprite sprite = AssetDatabase.LoadAssetAtPath(spritePath, typeof(Sprite)) as Sprite;
        return sprite;
    }
}
