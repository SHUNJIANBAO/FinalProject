
[Node("选择节点")]
public class SelectorNode : CompositeNode
{
    protected override E_NodeStatus Trick()
    {
#if UNITY_EDITOR
        CheckResetNodeStatus();
#endif
        for (int i = 0; i < ChildList.Count; i++)
        {
            E_NodeStatus result = ChildList[i].GetTrick();
#if UNITY_EDITOR
            SetAfterNodesNotWork(i);
#endif
            switch (result)
            {
                case E_NodeStatus.Running:
                    m_Tree.SetCurNode(ChildList[i]);
                    return result;
                case E_NodeStatus.Success:
                    ChildList[i].OnComplete();
                    return result;
            }
        }

        return E_NodeStatus.Failure;
    }

}
