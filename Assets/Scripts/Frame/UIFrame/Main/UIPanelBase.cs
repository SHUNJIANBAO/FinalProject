using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelBase : UIBase 
{
    Dictionary<string, List<GameObject>> _uiDict;
    private void Awake()
    {
        _uiDict= LoadAllUI();
        Init();
    }

    protected override T GetUI<T>(string uiName)
    {
        var uiList = _uiDict[uiName];
        if (typeof(T) == typeof(GameObject))
        {
            return uiList[0] as T;
        }
        if (typeof(T).IsSubclassOf(typeof(Component)))
        {
            return uiList[0].GetComponent<T>();
        }
        return null;
    }

    protected override List<T> GetUIList<T>(string uiName)
    {
        var uiList = _uiDict[uiName];
        if (typeof(T) == typeof(GameObject))
        {
            return uiList as List<T>;
        }
        if (typeof(T).IsSubclassOf(typeof(Component)))
        {
            List<T> coms = new List<T>();
            foreach (var ui in uiList)
            {
                coms.Add(ui.GetComponent<T>());
            }
            return coms;
        }
        return null;
    }

}
