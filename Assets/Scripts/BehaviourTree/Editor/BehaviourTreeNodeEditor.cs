using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BehaviourTreeNodeEditor
{
    private static BehaviourTreeNodeEditor _instance;
    public static BehaviourTreeNodeEditor Instance
    {
        get
        {
            if (_instance==null)
            {
                _instance = new BehaviourTreeNodeEditor();
                _instance.Init();
            }
            return _instance;
        }
    }
    public BehaviourTreeNodeEventEditor NodeEvent = new BehaviourTreeNodeEventEditor();


    List<NodeBase> m_NodeList;

    void Init()
    {

    }

    public void DrawNodes()
    {

    }

    public void OnDrag(Vector2 delta)
    {

    }

}
