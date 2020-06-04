using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionNode : NodeBase 
{
#if UNITY_EDITOR
    public override bool CanConnectLineAsParent()
    {
        return false;
    }
#endif

    public override void OnComplete()
    {
        base.OnComplete();
        m_Tree.SetCurNode(null);
    }

    public abstract void OnEnter();

    public abstract void OnStay();

    public abstract void OnExit();

}
