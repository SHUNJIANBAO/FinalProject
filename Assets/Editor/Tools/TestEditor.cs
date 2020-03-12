using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TestEditor
{
    [MenuItem("工具/ClickTest")]
    static void Click()
    {
        ItemConfig.LoadCsvCfg();
        Debug.Log(ItemConfig.GetData(1).TestList[2]);
    }
}
