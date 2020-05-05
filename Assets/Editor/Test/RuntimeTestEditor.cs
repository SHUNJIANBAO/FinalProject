using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RuntimeTest))]
public class RuntimeTestEditor : Editor
{
    RuntimeTest Target
    {
        get
        {
            return target as RuntimeTest;
        }
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Time.timeScale = Target.TimeScale;
    }
}
