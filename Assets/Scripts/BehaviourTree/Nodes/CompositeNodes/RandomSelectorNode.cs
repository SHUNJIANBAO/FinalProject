using System.Collections.Generic;

/// <summary>
/// 随机选择子节点中的一个来运行，如果成功则返回成功，失败会选择一个其它的节点
/// 当所有节点全部失败会返回失败
/// </summary>
[Node("随机选择节点")]
public class RandomSelectorNode : CompositeNode
{
    /// <summary>
    /// 存储当前运行过的节点的下标
    /// </summary>
    List<NodeBase> _nodesList;
    /// <summary>
    /// 随机排序的节点列表
    /// </summary>
    List<NodeBase> _randomList;
    public override void Init()
    {
        base.Init();
        _nodesList = new List<NodeBase>(); ;
        _randomList = Util.GetRandomList(ChildList);
    }
    public override void OnComplete()
    {
        base.OnComplete();
        _randomList = Util.GetRandomList(ChildList);
#if UNITY_EDITOR
        SetOtherNodesNotWork(_nodesList);
#endif
        _nodesList.Clear();
    }
    protected override E_NodeStatus Trick()
    {
#if UNITY_EDITOR
        CheckResetNodeStatus();
#endif
        for (int i = 0; i < ChildList.Count; i++)
        {
            E_NodeStatus result = _randomList[i].GetTrick();
            switch (result)
            {
                case E_NodeStatus.Running:
                    if (!_nodesList.Contains(_randomList[i]))
                        _nodesList.Add(_randomList[i]);
                    m_Tree.SetCurNode(_randomList[i]);
                    return result;
                case E_NodeStatus.Success:
                    if (!_nodesList.Contains(_randomList[i]))
                        _nodesList.Add(_randomList[i]);
                    _randomList[i].OnComplete();
                    return result;
                case E_NodeStatus.Failure:
                    if (!_nodesList.Contains(_randomList[i]))
                        _nodesList.Add(_randomList[i]);
                    continue;
            }
        }
        return E_NodeStatus.Failure;
        //throw new System.Exception("未知错误");
    }
}
