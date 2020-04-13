using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.Animations;

public class CreateEffectTool : EditorWindow
{
    string _effName;
    Object _animClip;

    string _animatorControllerPath = "Assets/Editor/Demo/AnimatorController/EffectAnimatorController.controller";
    static string _gameObjectPath = "Assets/Editor/Demo/Objects/";

    [MenuItem("工具/创建特效")]
    static void OpenWindow()
    {
        var window = GetWindow<CreateEffectTool>();
        window.titleContent = new GUIContent("创建特效");
        window.Show();
    }

    private void OnGUI()
    {
        _effName = EditorGUILayout.TextField("特效名", _effName);
        _animClip = EditorGUILayout.ObjectField("动画", _animClip, typeof(AnimationClip), true);
        if (GUILayout.Button("创建"))
        {
            if (string.IsNullOrEmpty(_effName))
            {
                Debug.LogError("名称不能为空");
                return;
            }
            if (_animClip == null)
            {
                Debug.LogError("动画不能为空");
                return;
            }
            CreateEffect();
        }
    }

    void CreateEffect()
    {
        CreatTargetDirectory(_effName);


        GameObject go = new GameObject("Effect_" + _effName);
        var sr = go.AddComponent<SpriteRenderer>();
        sr.sortingOrder = 20;
        var controller = CreateAnimatorController(GetAnimatorPath(_effName), GetNewAnimatorPath(_effName));
        go.AddComponent<Animator>().runtimeAnimatorController = controller;

        GameObject prefab = PrefabUtility.CreatePrefab(GetNewPrefabPath(_effName), go);
        PrefabUtility.ConnectGameObjectToPrefab(go, prefab);

    }

    /// <summary>
    /// 创建目标文件夹
    /// </summary>
    /// <param name="name"></param>
    void CreatTargetDirectory(string name)
    {
        string newPath = "Assets/Resources/Prefabs/Effects/" + name + "/";
        if (!Directory.Exists(newPath))
        {
            Directory.CreateDirectory(newPath);
        }
        AssetDatabase.Refresh();
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

        AnimationClip clip = (AnimationClip)_animClip;

        var stateArray = controller.layers[0].stateMachine.states;
        for (int i = 0; i < stateArray.Length; i++)
        {
            stateArray[i].state.motion = clip;
        }
        return controller;
    }

    string GetNewAnimatorPath(string name)
    {
        return "Assets/Resources/Prefabs/Effects/" + name + "/" + name + "AnimatorController.controller";
    }

    string GetAnimatorPath(string name)
    {
        return "Assets/ArtResources/Animator/Effects/" + name + ".anim";
    }

    string GetNewPrefabPath(string name)
    {
        return "Assets/Resources/Prefabs/Effects/" + name + "/"+ name + ".prefab";
    }

}
