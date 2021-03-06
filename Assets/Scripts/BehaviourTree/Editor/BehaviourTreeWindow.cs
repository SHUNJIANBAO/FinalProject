﻿using UnityEngine;
using UnityEditor;
using System;

public class BehaviourTreeWindow : EditorWindow
{
    public static BehaviourTreeWindow Instance;
    [MenuItem("工具/行为树")]
    static void OpenWindow()
    {
        var window = GetWindow<BehaviourTreeWindow>();
        window.titleContent = new GUIContent("行为树");
        window.Show();
    }

    BehaviourTreeNodeEditor _nodeEditor;

    private void OnEnable()
    {
        Instance = this;
        _nodeEditor = new BehaviourTreeNodeEditor(this);
        _nodeEditor.OnEnable();
        NodeBase.Repaint = Repaint;
        Selection.selectionChanged += SelectionAssets;
    } 

    private void OnDisable()
    {
        Selection.selectionChanged -= SelectionAssets;
    }

    void ResetWindow()
    {
        Ratio = 1;
        _gridSize = 20;
        Repaint();
    }

    private void OnGUI()
    {
        //SelectionAssets();

        DrawGrids(_gridSize * Ratio);
        _nodeEditor.OnGUI();
        ProcessEvent(Event.current);

        DrawButtons();
    }

    /// <summary>
    /// 窗口缩放比例
    /// </summary>
    public static float Ratio = 1;
    public Vector2 WindowCenter = Vector2.zero;

    static int _gridSize = 20;
    Vector3 fromPos = Vector3.zero;
    Vector3 toPos = Vector3.zero;
    Vector2 offest;
    Vector3 newOffest = Vector3.zero;
    Color gridColor = new Color(Color.gray.r, Color.gray.g, Color.gray.b, 0.5f);
    void DrawGrids(float width)
    {
        Handles.color = gridColor;
        int widthCount = Mathf.CeilToInt(position.width / width);
        int heightCount = Mathf.CeilToInt(position.height / width);
        Handles.BeginGUI();
        newOffest.x = offest.x % width;
        newOffest.y = offest.y % width;
        for (int i = 0; i < widthCount; i++)
        {
            fromPos.x = width * i + newOffest.x;
            fromPos.y = -width + newOffest.y;
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
            Handles.DrawLine(fromPos, toPos);
        }
        Handles.EndGUI();
    }

    void DrawButtons()
    {
        if (!Application.isPlaying)
        {
            if (GUILayout.Button(new GUIContent("文件"), GUILayout.Width(80), GUILayout.Height(20)))
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("新建"), false, () => { CreateNewTree(); });
                menu.AddItem(new GUIContent("读取"), false, () => { LoadAssets(null); });
                menu.AddItem(new GUIContent("保存"), false, () => { SaveAssets(); });
                menu.AddItem(new GUIContent("另存为"), false, () => { SaveNewAssets(); });
                menu.ShowAsContext();
            }
        }
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
                //case EventType.ScrollWheel:
                //    OnScrollWheel(e.delta);
                //    break;
        }
    }

    void OnDrag(Vector2 delta)
    {
        offest += delta;
        WindowCenter += delta;
        _nodeEditor.OnDrag(delta);
        Repaint();
    }

    void OnRightClick(Vector2 pos)
    {
        GenericMenu menu = new GenericMenu();
        Type[] compositeTypes = Util.GetTypes<CompositeNode>();
        Type[] decoratorTypes = Util.GetTypes<DecoratorNode>();
        Type[] actionTypes = Util.GetTypes<ActionNode>();
        CreateMenuItem("组合节点", compositeTypes, menu, pos, NodeGUIStyle.CompositeNormalStyle, NodeGUIStyle.CompositeSelectedStyle);
        CreateMenuItem("修饰节点", decoratorTypes, menu, pos, NodeGUIStyle.DecoratorNormalStyle, NodeGUIStyle.DecoratorSelectedStyle);
        CreateMenuItem("行为节点", actionTypes, menu, pos, NodeGUIStyle.ActionNormalStyle, NodeGUIStyle.ActionSelectedStyle);
        menu.ShowAsContext();
    }

    void CreateMenuItem(string typeName, Type[] typeArry, GenericMenu menu, Vector2 pos, GUIStyle nodeNormalStyle, GUIStyle nodeSelectedStyle)
    {
        for (int i = 0; i < typeArry.Length; i++)
        {
            var attr = typeArry[i].GetCustomAttributes(typeof(NodeAttribute), false)[0] as NodeAttribute;
            var path = attr.NodeName.Split('/');
            string nodeName = path[path.Length - 1];
            Type type = typeArry[i];
            menu.AddItem(new GUIContent(typeName + "/" + attr.NodeName), false, () => _nodeEditor.CreateNode(nodeName, type, GetXYCount(pos), nodeNormalStyle, nodeSelectedStyle));
        }
    }

    //void OnScrollWheel(Vector2 delta)
    //{
    //    Ratio = Mathf.Clamp(Ratio - delta.y / 30, 0.2f, 3f);
    //    _nodeEditor.OnScrollWheel(Ratio, _gridSize);
    //}

    /// <summary>
    /// 将坐标转换为规范坐标
    /// </summary>
    public Vector2 GetXYCount(Vector2 pos)
    {
        Vector2 newPos = new Vector2(Mathf.RoundToInt((pos.x - WindowCenter.x) / _gridSize), Mathf.RoundToInt((pos.y - WindowCenter.y) / _gridSize));
        return newPos;
    }

    /// <summary>
    /// 将xy轴的格数转化为坐标
    /// </summary>
    /// <param name="xyCount"></param>
    /// <returns></returns>
    public Vector2 GetPosition(Vector2 xyCount)
    {
        return new Vector2(xyCount.x * _gridSize, xyCount.y * _gridSize) + WindowCenter;
    }

    public void CreateNewTree()
    {
        _nodeEditor.Reset();
    }

    /// <summary>
    /// 选中Assets时在行为树中显示
    /// </summary>
    public void SelectionAssets()
    {
        if (Application.isPlaying)
        {
            if (Selection.activeGameObject == null) return;
            BehaviourTree tree = Selection.activeGameObject.GetComponent<BehaviourTree>();
            if (tree!=null)
            {
                var nodeAssets = tree.NodeAssets;
                if (nodeAssets != null)
                {
                    _nodeEditor.Refresh(nodeAssets);
                }
            }
        }
        else
        {
            if (Selection.activeObject is NodeAssets)
            {
                var nodeAssets = Selection.activeObject as NodeAssets;
                _nodeEditor.Refresh(nodeAssets);
            }
        }
    }

    /// <summary>
    /// 加载资源
    /// </summary>
    /// <param name="nodeAssets"></param>
    public void LoadAssets(NodeAssets nodeAssets)
    {
        if (nodeAssets==null)
        {
            string path= EditorUtility.OpenFilePanel("Load", "Assets/Resources/BehaviourTreeAssets", "asset");
            path = path.Replace(Application.dataPath, "Assets");
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            nodeAssets = AssetDatabase.LoadAssetAtPath(path, typeof(NodeAssets)) as NodeAssets;
        }
        if (nodeAssets != null)
            _nodeEditor.Refresh(nodeAssets);
    }

    /// <summary>
    /// 保存资源
    /// </summary>
    public void SaveAssets()
    {
       _nodeEditor.SaveNodeAssets();
    }

    /// <summary>
    /// 另存为资源
    /// </summary>
    public void SaveNewAssets()
    {
        _nodeEditor.SaveNewNodeAssets();
    }
}
