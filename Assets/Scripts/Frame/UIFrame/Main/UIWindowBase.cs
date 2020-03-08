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

}
