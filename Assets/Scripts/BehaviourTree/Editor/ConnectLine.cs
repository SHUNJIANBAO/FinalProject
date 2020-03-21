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
        if (Handles.Button((FromNode.Center+ToNode.Center)*0.5f,Quaternion.identity,8,8, Handles.RectangleHandleCap))
        {
            _destoryLine(this);
        }
    }

    
}
