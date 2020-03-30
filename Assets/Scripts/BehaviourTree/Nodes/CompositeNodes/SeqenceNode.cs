
/// <summary>
/// 按顺序执行所有子节点
/// 如果成功则运行下一个子节点，直到全部成功则返回成功
/// 如果失败则返回失败，不继续执行后续节点
/// </summary>
[Node("顺序节点")]
public class SeqenceNode : CompositeNode
{
    int _index;
    /// <summary>
    /// 中断时,运行失败时
    /// </summary>
    void OnInterrupt()
    {
        //SetAfterNodesNotWork(_index);
        _index = 0;
    }

    protected override E_NodeStatus Trick()
    {
        CheckResetNodeStatus();
        for (int i = _index; i < ChildList.Count;)
        {
            E_NodeStatus result = ChildList[i].GetTrick();
            SetAfterNodesNotWork(i);
            switch (result)
            {
                case E_NodeStatus.Running:
                    m_Tree.SetCurNode(ChildList[i]);
                    return result;
                case E_NodeStatus.Success:
                    ChildList[i].OnComplete();
                    _index++;
                    i = _index;
                    if (_index == ChildList.Count)
                    {
                        _index = 0;
                        return result;
                    }
                    break;
                case E_NodeStatus.Failure:
                    OnInterrupt();
                    return result;
            }
        }
        throw new System.Exception("未知错误");
    }
}
