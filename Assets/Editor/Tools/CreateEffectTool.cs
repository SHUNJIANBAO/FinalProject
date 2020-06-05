using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.Animations;

public class CreateEffectTool : EditorWindow
{
    string _effName;
    EffectType _effType;
    AnimationClip _animClip;
    ParticleSystem _particle;

    public enum EffectType
    {
        动画特效,
        粒子特效
    }

    string _animatorControllerPath = "Assets/Editor/Demo/AnimatorController/EffectAnimatorController.controller";
    //static string _gameObjectPath = "Assets/Editor/Demo/Objects/";

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
        _effType = (EffectType)EditorGUILayout.EnumPopup("特效类型", _effType);
        switch (_effType)
        {
            case EffectType.动画特效:
                _animClip = (AnimationClip)EditorGUILayout.ObjectField("动画", _animClip, typeof(AnimationClip), true);
                break;
            case EffectType.粒子特效:
                _particle = (ParticleSystem)EditorGUILayout.ObjectField("粒子", _particle, typeof(ParticleSystem), true);
                break;
        }
        if (GUILayout.Button("创建"))
        {
            if (string.IsNullOrEmpty(_effName))
            {
                Debug.LogError("名称不能为空");
                return;
            }
            switch (_effType)
            {
                case EffectType.动画特效:
                    if (_animClip == null)
                    {
                        Debug.LogError("动画不能为空");
                        return;
                    }
                    break;
                case EffectType.粒子特效:
                    if (_particle == null)
                    {
                        Debug.LogError("粒子不能为空");
                        return;
                    }
                    break;
            }
            CreateEffect(_effType);
        }
    }

    void CreateEffect(EffectType effType)
    {
        CreatTargetDirectory(_effName);

        switch (effType)
        {
            case EffectType.动画特效:
                GameObject go = new GameObject(_effName);
                var sr = go.AddComponent<SpriteRenderer>();
                sr.sortingOrder = 20;
                var controller = CreateAnimatorController(GetAnimatorPath(_effName), GetNewAnimatorPath(_effName));
                go.AddComponent<Animator>().runtimeAnimatorController = controller;
                go.AddComponent<EffectCtrl>();
                GameObject prefab = PrefabUtility.CreatePrefab(GetNewPrefabPath(_effName), go);
                PrefabUtility.ConnectGameObjectToPrefab(go, prefab);
                break;
            case EffectType.粒子特效:
                PrefabUtility.CreatePrefab(GetNewPrefabPath(_effName), _particle.gameObject);
                break;
        }



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
