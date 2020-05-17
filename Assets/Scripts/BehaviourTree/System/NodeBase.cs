using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class NodeBase : ScriptableObject
{
    public string Description;

    protected Character m_Owner;
    protected CharacterMovement m_Movement;
    protected BehaviourTree m_Tree;

    [SerializeField]
    [HideInInspector]
    NodeBase _parentNode;
    public NodeBase ParentNode
    {
        get
        {
            return _parentNode;
        }
        set
        {
            if (_parentNode != null && value != null) return;
            _parentNode = value;
        }
    }
    [SerializeField]
    [HideInInspector]
    List<NodeBase> _childNodesList = new List<NodeBase>();
    public List<NodeBase> ChildList
    {
        get
        {
            return _childNodesList;
        }
    }

    #region 编辑器相关
#if UNITY_EDITOR
    [HideInInspector]
    [SerializeField]
    public NodeEditorStyle Style;
    [HideInInspector]
    [SerializeField]
    protected NodeStatusStyles m_Status;
    [NonSerialized]
    GUIStyle _curStyle;
    [NonSerialized]
    GUIStyle _curStatus;
    Rect _descriptionRect;

    Vector2 _nodeSize;
    //Vector2 _initialNodeSize;
    Vector2 _nodePosition;
    Vector2 _windowCenter;

    public Vector2 Position
    {
        get
        {
            return Style.Rect.position;
        }
    }
    public Vector2 Center
    {
        get
        {
            return Style.Rect.center;
        }
    }
    public Vector2 XYCount
    {
        get
        {
            return Style.XYCount;
        }
    }
    public string Name
    {
        get
        {
            return Style.NodeName;
        }
    }
    /// <summary>
    /// 重新绘制窗口
    /// </summary>
    public static Action Repaint;

    public void InitEditorNode(NodeEditorStyle style)
    {
        this.Style = style;
        Rect statusRect = new Rect(Style.Rect.position.x + Style.Rect.width - (Style.Rect.height / 2.5f * 2), Style.Rect.position.y + Style.Rect.height / 2.5f * 0.5f, Style.Rect.height / 2.5f, Style.Rect.height / 2.5f);
        m_Status = new NodeStatusStyles(statusRect);
        _curStyle = Style.NormalStyle;
        _curStatus = m_Status.NotWork;
        _descriptionRect = new Rect(Style.Rect.position + new Vector2(-Style.Rect.width*0.5f, Style.Rect.height), new Vector2(Style.Rect.width*2, Style.Rect.height * 0.7f));
        //_initialNodeSize = Style.Rect.size;
        //资产资源名
        name = Name;
    }

    public void Reset()
    {
        Rect statusRect = new Rect(Style.Rect.position.x + Style.Rect.width - (Style.Rect.height / 2.5f * 2), Style.Rect.position.y + Style.Rect.height / 2.5f * 0.5f, Style.Rect.height / 2.5f, Style.Rect.height / 2.5f);
        m_Status = new NodeStatusStyles(statusRect);
        _curStyle = Style.NormalStyle;
        _curStatus = m_Status.NotWork;
        //_initialNodeSize = Style.Rect.size;
    }

    public void Draw()
    {
        GUI.Box(Style.Rect, Style.NodeName, _curStyle);
        GUI.Box(m_Status.Rect, "", _curStatus);
        if (!string.IsNullOrEmpty(Description))
        {
            GUI.Box(_descriptionRect, Description, Style.DescriptionStyle);
        }
    }

    public void OnDrag(Vector2 delta)
    {
        Style.Rect.position += delta;
        m_Status.Rect.position += delta;
        _descriptionRect.position += delta;
    }

    /// <summary>
    /// 设置节点的位置
    /// </summary>
    /// <param name="pos"></param>
    public void SetNodePosition(Vector2 pos)
    {
        m_Status.Rect.position += pos - Style.Rect.position;
        _descriptionRect.position += pos - Style.Rect.position;
        Style.Rect.position = pos;
    }

    /// <summary>
    /// 判断是否点击到了这个节点
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool ClickThisNode(Vector3 pos)
    {
        return Style.Rect.Contains(pos);
    }

    /// <summary>
    /// 切换节点的选中状态
    /// </summary>
    /// <param name="value">是否选中</param>
    public void SelectedNode(bool value)
    {
        if (value)
        {
            _curStyle = Style.SelectedStyle;
        }
        else
        {
            _curStyle = Style.NormalStyle;
        }
    }

    /// <summary>
    /// 重新加载连线
    /// </summary>
    /// <param name="createLineFunc"></param>
    public void Reload(Action<NodeBase, NodeBase> createLineFunc)
    {
        for (int i = 0; i < ChildList.Count; i++)
        {
            createLineFunc?.Invoke(this, ChildList[i]);
            //ChildList[i].Reload(createLineFunc);
        }
    }

    abstract public bool CanConnectLineAsParent();
    public bool CanConnectLineAsChild()
    {
        if (Name == "根节点")
        {
            return false;
        }
        return ParentNode == null;
    }

    //todo
    //public void OnScollWheel(float ratio, float gridSize, Vector2 windowCenter)
    //{
    //    SetNodeSize(ratio);
    //    RefreshNodePosition(gridSize, windowCenter);
    //}

    //void SetNodeSize(float ratio)
    //{
    //    _nodeSize.x = _initialNodeSize.x * ratio;
    //    _nodeSize.y = _initialNodeSize.y * ratio;
    //    Style.Rect.size = _nodeSize;
    //}

    //public void RefreshNodePosition(float gridSize, Vector2 windowCenter)
    //{
    //    _windowCenter = windowCenter;
    //    _nodePosition.x = Style.XYCount.x * gridSize;
    //    _nodePosition.y = Style.XYCount.y * gridSize;
    //    _nodePosition -= windowCenter;

    //    m_Status.Rect.position += _nodePosition - Style.Rect.position;
    //    Style.Rect.position = _nodePosition;
    //}
#endif
    #endregion

    /// <summary>
    /// 设置行为树
    /// </summary>
    /// <param name="tree"></param>
    public void SetTree(BehaviourTree tree)
    {
        m_Tree = tree;
        for (int i = 0; i < ChildList.Count; i++)
        {
            ChildList[i].SetTree(tree);
        }
    }

    /// <summary>
    /// 设置行为树控制的角色
    /// </summary>
    /// <param name="character"></param>
    public void SetCharacter(Character character)
    {
        m_Owner = character;
        m_Movement = character.GetComponent<CharacterMovement>();
        for (int i = 0; i < ChildList.Count; i++)
        {
            ChildList[i].SetCharacter(character);
        }
    }

    /// <summary>
    /// 添加子节点
    /// </summary>
    /// <param name="node"></param>
    public void AddChildNode(NodeBase node)
    {
        _childNodesList.Add(node);
    }

    /// <summary>
    /// 移除子节点
    /// </summary>
    /// <param name="node"></param>
    public void RemoveChildNode(NodeBase node)
    {
        if (_childNodesList.Contains(node))
        {
            _childNodesList.Remove(node);
        }
        else
            Debug.LogError("子节点列表中不包含：" + node.Name);
    }

    /// <summary>
    /// 改变节点状态显示
    /// </summary>
    /// <param name="status"></param>
    public void ChangeNodeStatus(GUIStyle status)
    {
        if (_curStatus != status)
        {
            Repaint?.Invoke();
            _curStatus = status;
        }
    }

    /// <summary>
    /// 将自身和所有子节点设为不活动状态
    /// </summary>
    public void SetNodeNotWork()
    {
        ChangeNodeStatus(m_Status.NotWork);
        for (int i = 0; i < ChildList.Count; i++)
        {
            ChildList[i].SetNodeNotWork();
        }
    }

    /// <summary>
    /// 初始化节点
    /// </summary>
    public virtual void Init()
    {
        for (int i = 0; i < ChildList.Count; i++)
        {
            ChildList[i].Init();
        }
    }

    /// <summary>
    /// 获取节点状态
    /// </summary>
    /// <returns></returns>
    public E_NodeStatus GetTrick()
    {
        E_NodeStatus status = Trick();
        switch (status)
        {
            case E_NodeStatus.NotWork:
                ChangeNodeStatus(m_Status.NotWork);
                break;
            case E_NodeStatus.Running:
                ChangeNodeStatus(m_Status.Running);
                break;
            case E_NodeStatus.Success:
                ChangeNodeStatus(m_Status.Success);
                break;
            case E_NodeStatus.Failure:
                ChangeNodeStatus(m_Status.Failure);
                break;
        }

        return status;
    }

    /// <summary>
    /// 检测节点状态
    /// </summary>
    /// <returns></returns>
    protected abstract E_NodeStatus Trick();

    /// <summary>
    /// 节点完成时调用
    /// </summary>
    public virtual void OnComplete()
    {

    }

    /// <summary>
    /// 重置该节点和其子节点的状态
    /// </summary>
    public void ResetStatus()
    {
        ChangeNodeStatus(m_Status.NotWork);
        for (int i = 0; i < ChildList.Count; i++)
        {
            ChildList[i].ChangeNodeStatus(m_Status.NotWork);
        }
    }
}

public enum E_NodeStatus
{
    NotWork,
    Running,
    Success,
    Failure
}

[Serializable]
public struct NodeEditorStyle
{
    public string NodeName;
    public GUIStyle NormalStyle;
    public GUIStyle SelectedStyle;
    public Rect Rect;
    public Vector2 XYCount;
    public GUIStyle DescriptionStyle;
    public NodeEditorStyle(Rect rect, Vector2 xyCount, string nodeName, GUIStyle normalSytle, GUIStyle selectedStyle)
    {
        this.Rect = rect;
        this.NodeName = nodeName;
        this.NormalStyle = normalSytle;
        this.SelectedStyle = selectedStyle;
        this.XYCount = xyCount;
        this.DescriptionStyle = NodeGUIStyle.DescriptionStyle;
    }
}
