using System.Collections.Generic;

public abstract class CompositeNode : NodeBase
{
    //是否运行过一次，如果为true则在开始时重置所有节点状态
    protected bool m_IsDirty;

#if UNITY_EDITOR
    public override bool CanConnectLineAsParent()
    {
        return true;
    }
    /// <summary>
    /// 检查是否运行过节点，非第一次进入时会先把所有节点状态清空
    /// </summary>
    protected void CheckResetNodeStatus()
    {
        if (m_IsDirty)
        {
            m_IsDirty = false;
            ResetStatus();
        }
    }

    /// <summary>
    /// 将后面的节点设为不活动状态
    /// </summary>
    protected void SetAfterNodesNotWork(int curIndex)
    {
        for (int i = curIndex + 1; i < ChildList.Count; i++)
        {
            ChildList[i].SetNodeNotWork();
        }
    }

    /// <summary>
    /// 将列表中不包含的节点设为不活动状态
    /// </summary>
    /// <param name="nodesList"></param>
    protected void SetOtherNodesNotWork(List<NodeBase> nodesList)
    {
        for (int i = 0; i < ChildList.Count; i++)
        {
            if (!nodesList.Contains(ChildList[i]))
                ChildList[i].SetNodeNotWork();
        }
    }
#endif

    public override void OnComplete()
    {
        base.OnComplete();
        m_IsDirty = true;
    }

}
