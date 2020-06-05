using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayEffect : EditorWindow
{
    string _effName;
    Transform _parent;
    Vector3 _offest;
    float _lifeTime;

    [MenuItem("工具/播放特效")]
    static void OpenWindow()
    {
        var window = GetWindow<PlayEffect>();
        window.titleContent = new GUIContent("播放特效");
        window.Show();
    }

    private void OnGUI()
    {
        _effName = EditorGUILayout.TextField("特效名", _effName);
        _parent = (Transform)EditorGUILayout.ObjectField("父物体", _parent, typeof(Transform), true);
        _offest = EditorGUILayout.Vector3Field("偏移值", _offest);
        _lifeTime = EditorGUILayout.FloatField("生命时长", _lifeTime);

        if (GUILayout.Button("播放"))
        {
            if (string.IsNullOrEmpty(_effName))
            {
                Debug.LogError("特效名不能为空");
                return;
            }
            EffectManager.Instance.Play(_effName, _offest, _parent, _lifeTime);
        }

    }

}
