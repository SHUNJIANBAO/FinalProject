using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWindowBase : UIBase
{
    protected RectTransform m_Root;
    protected CanvasGroup m_CanvasGroup;
    public Button Button_Close;
    protected override void OnInit()
    {
        base.OnInit();
        m_Root = GetUI<RectTransform>("Root");
        m_CanvasGroup = GetComponent<CanvasGroup>();
        if (Button_Close != null)
            AddButtonListen(Button_Close, () => { UIManager.Instance.CloseWindow(this); });
    }
    public override void OnOpen(params object[] objs)
    {
        base.OnOpen(objs);
        gameObject.SetActive(true);
        transform.SetAsLastSibling();
    }

    public override void OnClose(params object[] objs)
    {
        base.OnClose(objs);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 得到焦点时
    /// </summary>
    public virtual void OnFocus()
    {

    }

    /// <summary>
    /// 失去焦点时
    /// </summary>
    public virtual void OnLostFocus()
    {

    }

    #region 获取UI
    /// <summary>
    /// 获取UI列表中第一个UI
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="uiName"></param>
    /// <returns></returns>
    protected override T GetUI<T>(string uiName)
    {
        var uiList = UIManager.Instance.GetUIList(this, uiName);
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

    /// <summary>
    /// 获取UI列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="uiName"></param>
    /// <returns></returns>
    protected override List<T> GetUIList<T>(string uiName)
    {
        var uiList = UIManager.Instance.GetUIList(this, uiName);
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

}
