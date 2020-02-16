using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWindowBase : UIBase
{
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
