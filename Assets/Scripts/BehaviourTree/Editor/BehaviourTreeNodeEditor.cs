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

    NodeAssets _curAssets;
    NodeBase _curNode;
    NodeBase CurNode
    {
        get { return _curNode; }
        set
        {
            if (_curNode == value) return;
            if (_curNode!=null)
            {
                _curNode.SelectedNode(false);
            }
            _curNode = value;
            if (_curNode != null)
            {
                _curNode.SelectedNode(true);
            }
            Selection.activeObject = value;
            _window.Repaint();
        }
    }

    List<NodeBase> _nodeList;
    List<ConnectLine> _lineList = new List<ConnectLine>();

    bool _isDrag;
    bool _isConnecting;

    public void OnEnable()
    {
        Reset();
    }


    public void Reset()
    {
        CurNode = null;
        _isDrag = false;
        _curAssets = new NodeAssets();
        _isConnecting = false;
        _nodeList = new List<NodeBase>();
        _lineList.Clear();
        CreateNode("根节点", typeof(SelectorNode), _window.GetXYCount(_window.position.center), NodeGUIStyle.RootNormalStyle, NodeGUIStyle.RootSelectedStyle);
        _window.Repaint();
    }

    public void Refresh(NodeAssets assets)
    {
        if (_curAssets == assets) return;
        _curAssets = assets;
        _nodeList = new List<NodeBase>(assets.NodesList);
        for (int i = 0; i < _nodeList.Count; i++)
        {
            _nodeList[i].Reset();
        }
        _nodeList[0].Reload(ReloadLine);
        _window.Repaint();
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
                            if (CurNode != _nodeList[i])
                            {
                                CreateLine(CurNode, _nodeList[i]);
                            }
                            return;
                        }
                        //左键点击事件和右键点击事件
                        CurNode = _nodeList[i];
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
                CurNode = null;
                _isConnecting = false;
                break;
            case EventType.MouseDrag:
                if (_isDrag)
                {
                    if (CurNode != null)
                        CurNode.OnDrag(e.delta);
                    e.Use();
                }
                break;
            case EventType.MouseUp:
                if (_isDrag)
                {
                    CurNode.SetNodePosition(_window.GetPosition(_window.GetXYCount(CurNode.Position)));
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
        menu.AddItem(new GUIContent("删除节点"), false, () => { DestroyNode(CurNode); });
        menu.ShowAsContext();
    }

    public NodeBase CreateNode(string name, Type nodeType, Vector2 xyCount, GUIStyle nodeNormalStyle, GUIStyle nodeSelectedStyle)
    {
        Vector2 pos = _window.GetPosition(xyCount);
        Rect rect = new Rect(pos.x, pos.y, 200, 50);
        NodeEditorStyle editorStyle = new NodeEditorStyle(rect, xyCount, name, nodeNormalStyle, nodeSelectedStyle);
        var node = ScriptableObject.CreateInstance(nodeType) as NodeBase;
        node.InitEditorNode(editorStyle);
        _nodeList.Add(node);
        _window.Repaint();
        return node;
    }

    public void DestroyNode(NodeBase node)
    {
        for (int i = 0; i < _nodeList.Count; i++)
        {
            if (_nodeList[i].ParentNode == node)
            {
                _nodeList[i].ParentNode = null;
            }
        }
        _nodeList.Find(n => n.ChildList.Contains(node))?.RemoveChildNode(node);
        _nodeList.Remove(node);

        //移除包含该node的线
        List<ConnectLine> lines = new List<ConnectLine>();
        for (int i = 0; i < _lineList.Count; i++)
        {
            if (_lineList[i].FromNode == node || _lineList[i].ToNode == node)
            {
                lines.Add(_lineList[i]);
            }
        }
        for (int i = 0; i < lines.Count; i++)
        {
            DestoryLine(lines[i]);
        }
        _window.Repaint();
    }

    public void DrawTempLine(Vector2 mousePosition)
    {
        if (_isConnecting)
        {
            Handles.color = Color.white;
            Handles.DrawLine(CurNode.Center, mousePosition);
            _window.Repaint();
        }
    }

    public void CreateLine(NodeBase fromNode, NodeBase toNode)
    {
        if (fromNode.CanConnectLineAsParent() && toNode.CanConnectLineAsChild())
        {
            fromNode.AddChildNode(toNode);
            toNode.ParentNode = fromNode;
            ConnectLine line = new ConnectLine(fromNode, toNode, DestoryLine);
            _lineList.Add(line);
        }
    }

    public void ReloadLine(NodeBase fromNode, NodeBase toNode)
    {
        if (toNode.Name == "根节点" ||
            (fromNode is ActionNode && toNode is ActionNode))
        {
            return;
        }
        ConnectLine line = new ConnectLine(fromNode, toNode, DestoryLine);
        _lineList.Add(line);
    }

    public void DestoryLine(ConnectLine line)
    {
        _lineList.Remove(line);
    }

    /// <summary>
    /// 保存资源
    /// </summary>
    public void SaveNodeAssets()
    {
        if (_curAssets == null || !AssetDatabase.Contains(_curAssets))
        {
            SaveNewNodeAssets();
        }
        else
        {
            List<NodeBase> destroyNodeList = new List<NodeBase>();
            for (int i = 0; i < _curAssets.NodesList.Count; i++)
            {
                if (!_nodeList.Contains(_curAssets.NodesList[i]))
                {
                    destroyNodeList.Add(_curAssets.NodesList[i]);
                }
            }
            for (int i = 0; i < destroyNodeList.Count; i++)
            {
                _curAssets.NodesList.Remove(destroyNodeList[i]);
                UnityEngine.Object.DestroyImmediate(destroyNodeList[i],true);
            }

            string path = AssetDatabase.GetAssetPath(_curAssets);
            for (int i = 0; i < _nodeList.Count; i++)
            {
                if (!_curAssets.NodesList.Contains(_nodeList[i]))
                {
                    AssetDatabase.AddObjectToAsset(_nodeList[i], path);
                    _curAssets.NodesList.Add(_nodeList[i]);
                }
            }
            EditorUtility.SetDirty(_curAssets);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

    /// <summary>
    /// 另存为资源
    /// </summary>
    public void SaveNewNodeAssets()
    {
        NodeAssets assets = ScriptableObject.CreateInstance(typeof(NodeAssets)) as NodeAssets;
        assets.RootNode = _nodeList[0];
        _curAssets = assets;
        string assetsPath = EditorUtility.SaveFilePanelInProject("Save", "new BehaviourTree", "asset", "", "Assets/Resources/BehaviourTreeAssets");
        if (string.IsNullOrEmpty(assetsPath)) return;
        AssetDatabase.CreateAsset(assets, assetsPath);
        for (int i = 0; i < _nodeList.Count; i++)
        {
            if (!assets.NodesList.Contains(_nodeList[i]))
            {
                AssetDatabase.AddObjectToAsset(_nodeList[i], assetsPath);
                assets.NodesList.Add(_nodeList[i]);
            }
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
