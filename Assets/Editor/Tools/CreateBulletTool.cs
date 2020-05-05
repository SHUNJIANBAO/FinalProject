using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.IO;

public class CreateBulletTool : EditorWindow
{
    GUIContent _roleNameTitle;
    string _bulletId;
    Object _animClipStart;
    Object _animClipRun;
    Object _animClipEnd;
    E_BulletType type;

    string _animatorControllerPath = "Assets/Editor/Demo/AnimatorController/BulletAnimatorControllerDemo.controller";

    public enum E_BulletType
    {
        直线飞行,

    }

    [MenuItem("工具/创建子弹")]
    static void OpenWindow()
    {
        var window = GetWindow<CreateBulletTool>();
        window.titleContent = new GUIContent("创建子弹");
        window.Show();
    }

    private void OnEnable()
    {
        _roleNameTitle = new GUIContent("子弹名称");
    }


    private void OnGUI()
    {
        _bulletId = EditorGUILayout.TextField(_roleNameTitle, _bulletId);
        _animClipStart = EditorGUILayout.ObjectField("出生动画", _animClipStart, typeof(AnimationClip), true);
        _animClipRun = EditorGUILayout.ObjectField("飞行动画", _animClipRun, typeof(AnimationClip), true);
        _animClipEnd = EditorGUILayout.ObjectField("撞击动画", _animClipEnd, typeof(AnimationClip), true);
        type = (E_BulletType)EditorGUILayout.EnumPopup("子弹类型", type);

        if (GUILayout.Button("创建"))
        {
            if (string.IsNullOrEmpty(_bulletId))
            {
                Debug.LogError("子弹Id不能为空！！！");
                return;
            }
            if (_animClipStart == null || _animClipRun == null || _animClipEnd == null)
            {
                Debug.LogError("动画不能为空！！！");
                return;
            }
            CreateBullet("Bullet_" + _bulletId);
        }
    }

    void CreateBullet(string name)
    {
        CreatTargetDirectory();


        GameObject go = new GameObject(name);
        var sr = go.AddComponent<SpriteRenderer>();
        sr.sortingOrder = 20;
        var controller = CreateAnimatorController(GetNewAnimatorPath(name));
        go.AddComponent<Animator>().runtimeAnimatorController = controller;
        BulletBase ctrl = null;
        switch (type)
        {
            case E_BulletType.直线飞行:
                ctrl = go.AddComponent<BulletMoveForward>();
                break;
            default:
                break;
        }
        ctrl.Id = int.Parse(_bulletId);
        go.AddComponent<BoxCollider2D>().isTrigger = true;
        go.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

        go.layer = LayerMask.NameToLayer("Bullet");

        GameObject prefab = PrefabUtility.CreatePrefab(GetNewPrefabPath(name), go);
        PrefabUtility.ConnectGameObjectToPrefab(go, prefab);

    }

    /// <summary>
    /// 创建目标文件夹
    /// </summary>
    /// <param name="name"></param>
    void CreatTargetDirectory()
    {
        string newPath = "Assets/Resources/Prefabs/Bullets/";
        if (!Directory.Exists(newPath))
        {
            Directory.CreateDirectory(newPath);
            AssetDatabase.Refresh();
        }
    }

    AnimatorController CreateAnimatorController(string newControllerPath)
    {
        //AssetDatabase.CopyAsset(_animatorControllerPath, newControllerPath);
        if (File.Exists(newControllerPath))
        {
            File.Delete(newControllerPath);
        }
        File.Copy(_animatorControllerPath, newControllerPath);
        AssetDatabase.Refresh();
        AnimatorController controller = AssetDatabase.LoadAssetAtPath(newControllerPath, typeof(AnimatorController)) as AnimatorController;

        List<AnimationClip> clipList = new List<AnimationClip>();
        clipList.Add((AnimationClip)_animClipStart);
        clipList.Add((AnimationClip)_animClipRun);
        clipList.Add((AnimationClip)_animClipEnd);

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

    string GetNewAnimatorPath(string name)
    {
        return "Assets/Resources/Prefabs/Bullets/" + name + "AnimatorController.controller";
    }


    string GetNewPrefabPath(string name)
    {
        return "Assets/Resources/Prefabs/Bullets/" + name + ".prefab";
    }
}