using System.Collections;
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
        {
            var go = GameObject.Find("Canvas");
            if (go != null)
                MainCanvas = go.GetComponent<Canvas>();
        }
        if (MainCanvas == null)
            MainCanvas = GameObject.FindObjectOfType<Canvas>();
    }
    #endregion

    public Canvas MainCanvas;

    Stack<UIWindowBase> _windowStack = new Stack<UIWindowBase>();
    Dictionary<string, UIWindowBase> _windowDict = new Dictionary<string, UIWindowBase>();
    Dictionary<UIWindowBase, Dictionary<string, List<GameObject>>> _uiList = new Dictionary<UIWindowBase, Dictionary<string, List<GameObject>>>();

    /// <summary>
    /// 加载window
    /// </summary>
    /// <param name="windowName"></param>
    void LoadWindow(string windowName)
    {
        string path = PathManager.GetWindowPath(windowName);
        var obj = ResourceManager.Load<GameObject>(path);
        var windowObj = PoolManager.InstantiateGameObject(obj, PoolType.Window);
        windowObj.transform.SetParent(MainCanvas.transform, false);
        UIWindowBase window = windowObj.GetComponent<UIWindowBase>();
        _windowDict.Add(windowObj.name, window);
        var uiDict = window.LoadAllUI();
        RegistUI(window, uiDict);
        window.Init();
    }

    /// <summary>
    /// 打开window
    /// </summary>
    /// <param name="uiCallBack"></param>
    /// <param name="objs"></param>
    /// <returns></returns>
    public T OpenWindow<T>(bool openAnim = true, UICallBack uiCallBack = null, params object[] objs) where T : UIWindowBase
    {
        string windowName = typeof(T).Name;
        if (!_windowDict.ContainsKey(windowName))
        {
            LoadWindow(windowName);
        }
        T window = _windowDict[windowName] as T;
        //window.transform.SetAsLastSibling();
        window.OnOpen(objs);
        window.Status = UIStatus.Open;
        if (openAnim)
        {
            window.StartCoroutine(window.StartOpenAnim(uiCallBack, objs));
        }
        if (_windowStack.Count > 0)
        {
            var curTopWindow = _windowStack.Peek();
            if (curTopWindow != null) curTopWindow.OnLostFocus();
        }
        window.OnFocus();
        _windowStack.Push(window);
        return window;
    }

    /// <summary>
    /// 关闭window
    /// </summary>
    /// <param name="uiCallBack"></param>
    /// <param name="objs"></param>
    public void CloseWindow<T>(bool closeAnim = true, bool beForce = false, UICallBack uiCallBack = null, params object[] objs) where T : UIWindowBase
    {
        if (_windowStack.Count <= 0) return;
        var window = _windowStack.Peek();
        if (window.GetType() is T)
        {
            window = _windowStack.Pop();
        }
        else
        {
            string windowName = typeof(T).Name;
            window = _windowDict[windowName] as T;
            if (window.Status == UIStatus.Close) return;
        }
        window.Status = UIStatus.Close;
        if (closeAnim)
        {
            if (uiCallBack == null)
            {
                uiCallBack = window.OnClose;
            }
            else
            {
                uiCallBack += window.OnClose;
            }
            window.StartCoroutine(window.StartCloseAnim(uiCallBack, objs));
        }
        else
        {
            window.OnClose();
            if (uiCallBack != null)
            {
                uiCallBack(objs);
            }
        }
        if (!beForce && _windowStack.Count > 0)
        {
            window.OnLostFocus();
            var curTopWindow = _windowStack.Peek();
            if (curTopWindow != null) curTopWindow.OnFocus();
        }
    }

    public void CloseWindow(UIWindowBase UIWindowBase, bool closeAnim = true, bool beForce = false, UICallBack uiCallBack = null, params object[] objs)
    {
        if (_windowStack.Count <= 0) return;
        var window = _windowStack.Peek();
        if (window == UIWindowBase)
        {
            window = _windowStack.Pop();
        }
        else
        {
            string windowName = UIWindowBase.name;
            window = _windowDict[windowName] as UIWindowBase;
            if (window.Status == UIStatus.Close) return;
        }
        window.Status = UIStatus.Close;
        if (closeAnim)
        {
            if (uiCallBack == null)
            {
                uiCallBack = window.OnClose;
            }
            else
            {
                uiCallBack += window.OnClose;
            }
            window.StartCoroutine(window.StartCloseAnim(uiCallBack, objs));
        }
        else
        {
            window.OnClose();
            if (uiCallBack != null)
            {
                uiCallBack(objs);
            }
        }

        if (!beForce && _windowStack.Count > 0)
        {
            window.OnLostFocus();
            var curTopWindow = _windowStack.Peek();
            if (curTopWindow != null) curTopWindow.OnFocus();
        }
    }

    public void CloseAllWindow()
    {
        while (_windowStack.Count > 0)
        {
            var window = _windowStack.Pop();
            CloseWindow(window, false, true);
        }
    }

    /// <summary>
    /// 得到window
    /// </summary>
    /// <returns></returns>
    public T GetWindow<T>() where T : UIWindowBase
    {
        string windowName = typeof(T).Name;
        return _windowDict[windowName] as T;
    }

    /// <summary>
    /// 得到ui列表
    /// </summary>
    /// <param name="window"></param>
    /// <param name="uiName"></param>
    /// <returns></returns>
    public List<GameObject> GetUIList(UIWindowBase window, string uiName)
    {
        return _uiList[window][uiName];
    }

    /// <summary>
    /// 注册ui
    /// </summary>
    /// <param name="window"></param>
    /// <param name="uiName"></param>
    /// <param name="uiObj"></param>
    public void RegistUI(UIWindowBase UIWindowBase, Dictionary<string, List<GameObject>> uiDict)
    {
        if (_uiList.ContainsKey(UIWindowBase))
        {
            Debug.LogError("存在相同UIWindowBase" + UIWindowBase.name);
        }
        else
            _uiList.Add(UIWindowBase, uiDict);
    }
}
