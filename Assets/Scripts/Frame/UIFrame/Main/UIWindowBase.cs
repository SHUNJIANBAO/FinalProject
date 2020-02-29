using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWindowBase : UIBase
{
    protected RectTransform m_Root;
    protected CanvasGroup m_CanvasGroup;
    protected override void OnInit()
    {
        base.OnInit();
        m_Root = GetUI<RectTransform>("Root");
        m_CanvasGroup = GetComponent<CanvasGroup>();
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

}
