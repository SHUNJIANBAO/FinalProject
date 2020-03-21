using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class TestEditor
{
    [MenuItem("工具/ClickTest")]
    static void Click()
    {
        
        AnimConfig.LoadCsvCfg();
        Debug.Log(AnimConfig.GetData(100).Name);
    }
}
