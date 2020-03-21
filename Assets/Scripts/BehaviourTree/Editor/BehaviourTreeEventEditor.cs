using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BehaviourTreeEventEditor
{

    /// <summary>
    /// 创建一个Node节点
    /// </summary>
    /// <param name="name">Node名称</param>
    /// <param name="nodeType">Node类型</param>
    /// <param name="pos">Window中的坐标</param>
    /// <param name="nodeNormalStyle">默认皮肤</param>
    /// <param name="nodeSelectedStyle">选中皮肤</param>
    /// <returns></returns>
    public NodeBase CreateNode(string name, Type nodeType,Vector2 pos, Vector2 xyCount, GUIStyle nodeNormalStyle, GUIStyle nodeSelectedStyle)
    {
        Rect rect = new Rect(pos.x, pos.y, 200, 50);
        NodeEditorStyle editorStyle = new NodeEditorStyle(rect, xyCount, name, nodeNormalStyle, nodeSelectedStyle);
        var node= ScriptableObject.CreateInstance(nodeType) as NodeBase;
        node.InitEditorNode(editorStyle);
        return node;
    }

    public void DestroyNode()
    {

    }

    public void CreateConnectLine(NodeBase fromNode,NodeBase toNode)
    {

    }

    public void DestroyConnectLine()
    {

    }
}
