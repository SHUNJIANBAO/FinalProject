using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class BehaviourTreeNodeEditor
{
    BehaviourTreeWindow _window;
    public BehaviourTreeNodeEditor(BehaviourTreeWindow window)
    {
        _window = window;
    }
    BehaviourTreeEventEditor _nodeEvent = new BehaviourTreeEventEditor();

    NodeBase curNode;
    List<NodeBase> _nodeList = new List<NodeBase>();
    List<ConnectLine> _lineList = new List<ConnectLine>();

    //float _ratio;
    //float _gridSize;
    bool _isDrag;
    bool _isConnecting;

    void Init()
    {

    }

    public void OnGUI()
    {
        DrawTempLine(Event.current.mousePosition);
        DrawLines();
        DrawNodes();
        ProcessEvent(Event.current);
    }

    void DrawNodes()
    {
        for (int i = 0; i < _nodeList.Count; i++)
        {
            _nodeList[i].Draw();
        }
    }

    void DrawLines()
    {
        for (int i = 0; i < _lineList.Count; i++)
        {
            _lineList[i].DrawLine();
        }
    }

    public void OnDrag(Vector2 delta)
    {
        for (int i = 0; i < _nodeList.Count; i++)
        {
            _nodeList[i].OnDrag(delta);
        }
    }

    //public void OnScrollWheel(float ratio, float gridSize)
    //{
    //    _ratio = ratio;
    //    _gridSize = gridSize;
    //    for (int i = 0; i < _nodeList.Count; i++)
    //    {
    //        _nodeList[i].OnScollWheel(ratio, gridSize, _window.WindowCenter);
    //    }
    //}

    void ProcessEvent(Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                for (int i = _nodeList.Count - 1; i >= 0; i--)
                {
                    //如果点击到Node执行以下逻辑
                    if (_nodeList[i].ClickThisNode(e.mousePosition))
                    {
                        //判断是否处于连线状态,如果处于连线状态，只处理连线的逻辑
                        if (_isConnecting)
                        {
                            _isConnecting = false;
                            if (curNode!= _nodeList[i])
                            {
                                CreateLine(curNode, _nodeList[i]);
                            }
                            return;
                        }
                        //左键点击事件和右键点击事件
                        curNode = _nodeList[i];
                        if (e.button == 0)
                        {
                            _isDrag = true;
                        }
                        else if (e.button == 1)
                        {
                            OnRightClick(e.mousePosition);
                        }
                        e.Use();
                        _isConnecting = false;
                        return;
                    }
                }
                _isDrag = false;
                curNode = null;
                _isConnecting = false;
                break;
            case EventType.MouseDrag:
                if (_isDrag)
                {
                    if (curNode != null)
                        curNode.OnDrag(e.delta);
                    e.Use();
                }
                break;
            case EventType.MouseUp:
                if (_isDrag)
                {
                    curNode.SetNodePosition(_window.GetPosition(_window.GetXYCount(curNode.Position)));
                    _isDrag = false;
                    _window.Repaint();
                }
                break;
            //case EventType.ScrollWheel:

            //    break;
        }
    }

    void OnRightClick(Vector3 pos)
    {
        GenericMenu menu = new GenericMenu();
        menu.AddItem(new GUIContent("添加连线"), false, () => { _isConnecting = true; });
        menu.AddItem(new GUIContent("删除节点"), false, () => { DestroyNode(curNode); });
        menu.ShowAsContext();
    }

    public void CreateNode(string name, Type nodeType, Vector2 xyCount, GUIStyle nodeNormalStyle, GUIStyle nodeSelectedStyle)
    {
        Vector2 pos = _window.GetPosition(xyCount);
        Rect rect = new Rect(pos.x, pos.y, 200, 50);
        NodeEditorStyle editorStyle = new NodeEditorStyle(rect, xyCount, name, nodeNormalStyle, nodeSelectedStyle);
        var node = ScriptableObject.CreateInstance(nodeType) as NodeBase;
        node.InitEditorNode(editorStyle);
        _nodeList.Add(node);
        _window.Repaint();
    }

    public void DestroyNode(NodeBase node)
    {
        _nodeList.Remove(node);
        _window.Repaint();
    }

    public void DrawTempLine(Vector2 mousePosition)
    {
        if (_isConnecting)
        {
            Handles.color = Color.white;
            Handles.DrawLine(curNode.Center, mousePosition);
            _window.Repaint();
        }
    }

    public void CreateLine(NodeBase fromNode,NodeBase toNode)
    {
        ConnectLine line = new ConnectLine(fromNode, toNode, DestoryLine);
        _lineList.Add(line);
    }

    public void DestoryLine(ConnectLine line)
    {
        _lineList.Remove(line);
    }

    public void SaveNodeAssest()
    {

    }
}
