using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class BehaviourTreeWindow : EditorWindow
{
    [MenuItem("工具/行为树")]
    static void OpenWindow()
    {
        var window = GetWindow<BehaviourTreeWindow>();
        window.titleContent = new GUIContent("行为树");
        window.Show();
    }

    private void OnGUI()
    {
        BehaviourTreeNodeEditor.Instance.DrawNodes();
        ProcessEvent(Event.current);
        DrawGrids(m_GridWidth);
    }


    int m_GridWidth = 20;
    Vector3 fromPos = Vector3.zero;
    Vector3 toPos = Vector3.zero;
    Vector2 offest;
    Vector3 newOffest=Vector3.zero;
    Color gridColor= new Color(Color.gray.r, Color.gray.g, Color.gray.b, 0.5f);
    void DrawGrids(float width)
    {
        Handles.color = gridColor;
        int widthCount = Mathf.CeilToInt(position.width / width);
        int heightCount = Mathf.CeilToInt(position.height / width);
        Handles.BeginGUI();
        newOffest.x=offest.x % width;
        newOffest.y = offest.y % width;
        for (int i = 0; i < widthCount; i++)
        {
            fromPos.x = width * i+newOffest.x ;
            fromPos.y = -width+newOffest.y;
            toPos.x = width * i + newOffest.x;
            toPos.y = width * heightCount + newOffest.y;
            Handles.DrawLine(fromPos, toPos);
        }
        for (int i = 0; i < heightCount; i++)
        {
            fromPos.x = -width + newOffest.x;
            fromPos.y = width * i + newOffest.y;
            toPos.x = width * widthCount + newOffest.x;
            toPos.y = width * i + newOffest.y;
            Handles.DrawLine(fromPos , toPos );
        }
        Handles.EndGUI();
    }

    void ProcessEvent(Event e)
    {
        switch (e.type)
        {
            case EventType.ContextClick:
                OnRightClick(e.mousePosition);
                break;
            case EventType.MouseDrag:
                OnDrag(e.delta);
                break;
        }
    }

    void OnDrag(Vector2 delta)
    {
        offest += delta;
        BehaviourTreeNodeEditor.Instance.OnDrag(delta);
        Repaint();
    }

    void OnRightClick(Vector2 pos)
    {
        GenericMenu menu = new GenericMenu();
        Type[] compositeTypes = Util.GetTypes<CompositeNode>();
        Type[] decoratorTypes = Util.GetTypes<DecoratorNode>();
        Type[] actionTypes = Util.GetTypes<ActionNode>();
        CreateMenuItem(compositeTypes, menu);
        CreateMenuItem(decoratorTypes, menu);
        CreateMenuItem(actionTypes, menu);
    }

    void CreateMenuItem(Type[] typeArry,GenericMenu menu)
    {
        for (int i = 0; i < typeArry.Length; i++)
        {
            //menu.AddItem()
        }
    }
}
