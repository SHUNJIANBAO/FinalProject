using System.Collections.Generic;

/// <summary>
/// 以随机的顺序来执行所有子节点
/// 如果成功则运行其它随机一个子节点，直到全部成功则返回成功
/// 如果失败则返回失败，不继续执行其它节点
/// </summary>
[Node("组合节点/随机顺序节点")]
public class RandomSeqenceNode : CompositeNode
{
    int _index;
    /// <summary>
    /// 存储当前运行过的节点的下标（失败的节点）
    /// </summary>
    List<NodeBase> _nodesList;
    /// <summary>
    /// 随机排序的节点列表
    /// </summary>
    List<NodeBase> _randomList;
    public override void Init()
    {
        base.Init();
        _index = 0;
        _randomList = Util.GetRandomList(ChildList);
        _nodesList = new List<NodeBase>();
        _nodesList.Clear();
    }
    public override void OnComplete()
    {
        base.OnComplete();
        _randomList = Util.GetRandomList(ChildList);
        _nodesList.Clear();
        _index = 0;
    }
    /// <summary>
    /// 中断时,运行失败时
    /// </summary>
    void OnInterrupt()
    {
        _randomList = Util.GetRandomList(ChildList);
        _nodesList.Clear();
        _index = 0;
    }

    protected override E_NodeStatus Trick()
    {
        CheckResetNodeStatus();
        for (int i = _index; i < _randomList.Count;)
        {
            E_NodeStatus result = _randomList[i].GetTrick();
            SetOtherNodesNotWork(_nodesList);
            switch (result)
            {
                case E_NodeStatus.Running:
                    if (!_nodesList.Contains(_randomList[i]))
                        _nodesList.Add(_randomList[i]);
                    m_Tree.SetCurNode(_randomList[i]);
                    return result;
                case E_NodeStatus.Success:
                    _randomList[i].OnComplete();
                    _index++;
                    i = _index;
                    if (i == _randomList.Count)
                    {
                        return result;
                    }
                    else
                    {
                        if (!_nodesList.Contains(_randomList[i]))
                            _nodesList.Add(_randomList[i]);
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
