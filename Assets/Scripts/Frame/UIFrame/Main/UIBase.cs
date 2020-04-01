using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public enum UIStatus
{
    Open,
    Close
}

public abstract class UIBase : MonoBehaviour
{
    [HideInInspector]
    public UIStatus Status;

    /// <summary>
    /// 加载所有子物体
    /// </summary>
    /// <returns></returns>
    public Dictionary<string,List<GameObject>> LoadAllUI()
    {
        Transform[] trans = GetComponentsInChildren<Transform>(true);
        Dictionary<string, List<GameObject>> uiDict = new Dictionary<string, List<GameObject>>();
        for (int i = 1; i < trans.Length; i++)
        {
            if (!uiDict.ContainsKey(trans[i].name))
            {
                List<GameObject> tempList = new List<GameObject>();
                uiDict.Add(trans[i].name, tempList);
            }
            uiDict[trans[i].name].Add(trans[i].gameObject);
        }
        return uiDict;
    }

    #region 生命周期

    public virtual void Init()
    {
        GetUIComponent();
        AddUIListener();
        OnInit();
    }

    protected virtual void GetUIComponent()
    {

    }

    protected virtual void AddUIListener()
    {

    }

    protected virtual void OnInit()
    {

    }

    public virtual void OnOpen(params object[] objs)
    {

    }

    public virtual IEnumerator StartOpenAnim(UICallBack uiCallBack, params object[] objs)
    {
        if (uiCallBack != null)
            uiCallBack(objs);
        yield break;
    }

    public virtual IEnumerator StartCloseAnim(UICallBack uiCallBack, params object[] objs)
    {
        uiCallBack(objs);
        yield break;
    }

    public virtual void OnClose(params object[] objs)
    {

    }

    /// <summary>
    /// 刷新
    /// </summary>
    /// <param name="objs"></param>
    public virtual void OnRefresh(params object[] objs)
    {

    }
    #endregion

    #region 事件接口

    #region 获取UI
    /// <summary>
    /// 获取UI列表中第一个UI
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="uiName"></param>
    /// <returns></returns>
    abstract protected T GetUI<T>(string uiName) where T : UnityEngine.Object;

    /// <summary>
    /// 获取UI列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="uiName"></param>
    /// <returns></returns>
    protected abstract List<T> GetUIList<T>(string uiName);

    /// <summary>
    /// 获取UIBehaviour事件类
    /// </summary>
    /// <param name="uiName"></param>
    /// <returns></returns>
    UIBehaviour GetBehaviour(string uiName)
    {
        UIBehaviour be = GetUI<UIBehaviour>(uiName);
        if (be == null)
        {
            be = GetUI<GameObject>(uiName).AddComponent<UIBehaviour>();
        }
        return be;
    }
    #endregion

    #region 添加事件

    /// <summary>
    /// 添加Toggle点击事件
    /// </summary>
    /// <param name="uiName"></param>
    /// <param name="action"></param>
    protected void AddToggleListen(string uiName, UnityAction<bool> action)
    {
        UIBehaviour uiBehaviour = GetBehaviour(uiName);
        uiBehaviour.AddToggleListen(action);
    }

    /// <summary>
    /// 添加Button点击事件
    /// </summary>
    /// <param name="uiName"></param>
    /// <param name="action"></param>
    protected void AddButtonListen(string uiName, UnityAction action)
    {
        UIBehaviour uiBehaviour = GetBehaviour(uiName);
        uiBehaviour.AddButtonListen(action);
    }
    protected void AddButtonListen(Button btn, UnityAction action)
    {
        btn.onClick.AddListener(action);
    }

    /// <summary>
    /// 添加Slider滑动事件
    /// </summary>
    /// <param name="uiName"></param>
    /// <param name="action"></param>
    protected void AddSliderListen(string uiName, UnityAction<float> action)
    {
        UIBehaviour uiBehaviour = GetBehaviour(uiName);
        uiBehaviour.AddSliderListen(action);
    }
    protected void AddSliderListen(Slider slider,UnityAction<float> action)
    {
        slider.onValueChanged.AddListener(action);
    }

    /// <summary>
    /// 添加点击事件
    /// </summary>
    /// <param name="uiName"></param>
    /// <param name="action"></param>
    public void AddPointClick(string uiName, UnityAction<BaseEventData> action)
    {
        UIBehaviour uiBehaviour = GetBehaviour(uiName);
        uiBehaviour.AddPointClick(action);
    }

    /// <summary>
    /// 添加点击按下事件
    /// </summary>
    /// <param name="uiName"></param>
    /// <param name="action"></param>
    public void AddPointClickDown(string uiName, UnityAction<BaseEventData> action)
    {
        UIBehaviour uiBehaviour = GetBehaviour(uiName);
        uiBehaviour.AddPointClickDown(action);
    }

    /// <summary>
    /// 添加点击抬起事件
    /// </summary>
    /// <param name="uiName"></param>
    /// <param name="action"></param>
    public void AddPointClickUP(string uiName, UnityAction<BaseEventData> action)
    {
        UIBehaviour uiBehaviour = GetBehaviour(uiName);
        uiBehaviour.AddPointClickUP(action);
    }

    /// <summary>
    /// 添加拖拽事件
    /// </summary>
    /// <param name="uiName"></param>
    /// <param name="action"></param>
    public void AddDrag(string uiName, UnityAction<BaseEventData> action)
    {
        UIBehaviour uiBehaviour = GetBehaviour(uiName);
        uiBehaviour.AddDrag(action);
    }

    /// <summary>
    /// 添加拖拽开始事件
    /// </summary>
    /// <param name="uiName"></param>
    /// <param name="action"></param>
    public void AddBeginDrag(string uiName, UnityAction<BaseEventData> action)
    {
        UIBehaviour uiBehaviour = GetBehaviour(uiName);
        uiBehaviour.AddBeginDrag(action);
    }

    /// <summary>
    /// 添加拖拽结束事件
    /// </summary>
    /// <param name="uiName"></param>
    /// <param name="action"></param>
    public void AddEndDrag(string uiName, UnityAction<BaseEventData> action)
    {
        UIBehaviour uiBehaviour = GetBehaviour(uiName);
        uiBehaviour.AddEndDrag(action);
    }

    #endregion

    #endregion
}
