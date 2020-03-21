using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class NodeBase : ScriptableObject
{
#if UNITY_EDITOR
    protected NodeEditorStyle m_Style;
    protected NodeStatusStyles m_Status;
    [NonSerialized]
    GUIStyle _curStyle;
    [NonSerialized]
    GUIStyle _curStatus;

    Vector2 _nodeSize;
    Vector2 _initialNodeSize;
    Vector2 _nodePosition;
    Vector2 _windowCenter;

    public Vector2 Position
    {
        get
        {
            return m_Style.Rect.position;
        }
    }
    public Vector2 Center
    {
        get
        {
            return m_Style.Rect.center;
        }
    }
    public Vector2 XYCount
    {
        get
        {
            return m_Style.XYCount;
        }
    }

    public void InitEditorNode(NodeEditorStyle style)
    {
        this.m_Style = style;
        _curStyle = m_Style.NormalStyle;
        Rect statusRect = new Rect(m_Style.Rect.position.x + m_Style.Rect.width - (m_Style.Rect.height / 2.5f * 2), m_Style.Rect.position.y + m_Style.Rect.height / 2.5f * 0.5f, m_Style.Rect.height / 2.5f, m_Style.Rect.height / 2.5f);
        m_Status = new NodeStatusStyles(statusRect);
        _curStatus = m_Status.NotWork;
        _initialNodeSize = style.Rect.size;
    }

    public void Draw()
    {
        GUI.Box(m_Style.Rect, m_Style.NodeName, _curStyle);
        GUI.Box(m_Status.Rect, "", _curStatus);
    }

    public void OnDrag(Vector2 delta)
    {
        m_Style.Rect.position += delta;
        m_Status.Rect.position += delta;
    }

    public void SetNodePosition(Vector2 pos)
    {
        m_Status.Rect.position += pos - m_Style.Rect.position;
        m_Style.Rect.position = pos;
    }

    public bool ClickThisNode(Vector3 pos)
    {
        return m_Style.Rect.Contains(pos);
    }

    public void SelectedNode(bool value)
    {
        if (value)
        {
            _curStyle = m_Style.NormalStyle;
        }
        else
        {
            _curStatus = m_Style.SelectedStyle;
        }
    }

    //todo
    public void OnScollWheel(float ratio, float gridSize, Vector2 windowCenter)
    {
        SetNodeSize(ratio);
        RefreshNodePosition(gridSize, windowCenter);
    }

    void SetNodeSize(float ratio)
    {
        _nodeSize.x = _initialNodeSize.x * ratio;
        _nodeSize.y = _initialNodeSize.y * ratio;
        m_Style.Rect.size = _nodeSize;
    }

    public void RefreshNodePosition(float gridSize, Vector2 windowCenter)
    {
        _windowCenter = windowCenter;
        _nodePosition.x = m_Style.XYCount.x * gridSize;
        _nodePosition.y = m_Style.XYCount.y * gridSize;
        _nodePosition -= windowCenter;

        m_Status.Rect.position += _nodePosition - m_Style.Rect.position;
        m_Style.Rect.position = _nodePosition;
    }
#endif
}

public enum E_NodeStatus
{
    NotWork,
    Running,
    Success,
    Failure
}

public struct NodeEditorStyle
{
    public string NodeName;
    public GUIStyle NormalStyle;
    public GUIStyle SelectedStyle;
    public Rect Rect;
    public Vector2 XYCount;
    public NodeEditorStyle(Rect rect, Vector2 xyCount, string nodeName, GUIStyle normalSytle, GUIStyle selectedStyle)
    {
        this.Rect = rect;
        this.NodeName = nodeName;
        this.NormalStyle = normalSytle;
        this.SelectedStyle = selectedStyle;
        this.XYCount = xyCount;
    }
}

public struct NodeEditorStatusStyle
{
    //public GUIStyle 
}
