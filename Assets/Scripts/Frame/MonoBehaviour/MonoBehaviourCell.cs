using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBehaviourCell : MonoBehaviour 
{
    /// <summary>
    /// 排序下标，越小越先执行
    /// </summary>
    public int Index=1;

    /// <summary>
    /// 初始化方法（包括重置）
    /// </summary>
    public virtual void Init()
    {

    }

    protected virtual void OnUpdate()
    {

    }
    public void CellUpdate()
    {
        if (!gameObject.activeSelf) return;
        OnUpdate();
    }

    protected virtual void OnFixedUpdate()
    {

    }
    public void CellFixedUpdate()
    {
        if (!gameObject.activeSelf) return;
        OnFixedUpdate();
    }

    protected virtual void OnLateUpdate()
    {
        
    }
    public void CellLateUpdate()
    {
        if (!gameObject.activeSelf) return;
        OnLateUpdate();
    }

    public void OnDrawGizmos()
    {
        OnDrawGizmosUpdate();
    }
    protected virtual void OnDrawGizmosUpdate() { }
}
