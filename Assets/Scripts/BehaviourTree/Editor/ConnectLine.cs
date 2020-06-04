using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

public class ConnectLine  
{
    public NodeBase FromNode;
    public NodeBase ToNode;
    Action<ConnectLine> _destoryLine;

    Vector2 _center;
    Vector2 _offest=new Vector2(12,0);

    public ConnectLine(NodeBase fromNode,NodeBase toNode, Action<ConnectLine> destoryLine)
    {
        this.FromNode = fromNode;
        this.ToNode = toNode;
        this._destoryLine = destoryLine;

    }

    public void DrawLine()
    {
        Handles.color = Color.white;
        Handles.DrawLine(FromNode.Center, ToNode.Center);
        _center = (FromNode.Center + ToNode.Center) * 0.5f;
        Handles.Label(_center+ _offest, (FromNode.ChildList.IndexOf(ToNode)+1).ToString());
        if (Handles.Button(_center, Quaternion.identity,8,8, Handles.RectangleHandleCap))
        {
            this.FromNode.RemoveChildNode(this.ToNode);
            this.ToNode.ParentNode = null;
            _destoryLine(this);
        }
    }
}
