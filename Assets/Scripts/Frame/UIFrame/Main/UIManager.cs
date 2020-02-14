﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void UICallBack(params object[] objs);

public class UIManager
{
    #region 单例
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new UIManager();
            }
            return _instance;
        }
    }
    public static UIManager GetInstance()
    {
        return _instance;
    }
    private UIManager()
    {
        if (MainCanvas == null)
            MainCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        if (MainCanvas == null)
            MainCanvas = GameObject.FindObjectOfType<Canvas>();
    }
    #endregion

    public Canvas MainCanvas;


    Dictionary<string, UIBase> m_WindowDict = new Dictionary<string, UIBase>();
    Dictionary<UIBase, Dictionary<string, List<GameObject>>> m_UIList = new Dictionary<UIBase, Dictionary<string, List<GameObject>>>();

    /// <summary>
    /// 加载window
    /// </summary>
    /// <param name="windowName"></param>
    void LoadWindow(string windowName)
    {
        string path = PathManager.GetWindowPath(windowName);
        var windowObj = ResourceManager.Load(path);
        UIBase window = windowObj.GetComponent<UIBase>();
        m_WindowDict.Add(windowObj.name, window);
        var uiDict = window.LoadAllUI();
        RegistUI(window, uiDict);
    }

    /// <summary>
    /// 打开window
    /// </summary>
    /// <param name="uiCallBack"></param>
    /// <param name="objs"></param>
    /// <returns></returns>
    public T OpenWindow<T>(UICallBack uiCallBack = null, params object[] objs) where T : UIBase
    {
        string windowName = typeof(T).Name;
        if (!m_WindowDict.ContainsKey(windowName))
        {
            LoadWindow(windowName);
        }
        T window = m_WindowDict[windowName] as T;
        window.transform.SetAsLastSibling();
        window.OnOpen(objs);
        window.StartCoroutine(window.StartOpenAnim(uiCallBack, objs));
        return window;
    }

    /// <summary>
    /// 关闭window
    /// </summary>
    /// <param name="uiCallBack"></param>
    /// <param name="objs"></param>
    public void CloseWindow<T>(UICallBack uiCallBack = null, params object[] objs) where T : UIBase
    {
        string windowName = typeof(T).Name;
        T window = m_WindowDict[windowName] as T;
        if (uiCallBack == null)
        {
            uiCallBack = window.OnClose;
        }
        else
        {
            uiCallBack += window.OnClose;
        }
        window.StartCloseAnim(uiCallBack, objs);
    }

    /// <summary>
    /// 得到window
    /// </summary>
    /// <returns></returns>
    public T GetWindow<T>() where T : UIBase
    {
        string windowName = typeof(T).Name;
        return m_WindowDict[windowName] as T;
    }

    /// <summary>
    /// 得到ui列表
    /// </summary>
    /// <param name="window"></param>
    /// <param name="uiName"></param>
    /// <returns></returns>
    public List<GameObject> GetUIList(UIBase window, string uiName)
    {
        return m_UIList[window][uiName];
    }

    /// <summary>
    /// 注册ui
    /// </summary>
    /// <param name="window"></param>
    /// <param name="uiName"></param>
    /// <param name="uiObj"></param>
    void RegistUI(UIBase uiBase, Dictionary<string, List<GameObject>> uiDict)
    {
        if (m_UIList.ContainsKey(uiBase))
        {
            Debug.LogError("存在相同UIBase" + uiBase.name);
        }
        else
            m_UIList.Add(uiBase, uiDict);
    }
}
