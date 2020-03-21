using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NodeGUIStyle 
{
    public static GUIStyle RootNormalStyle
    {
        get
        {
            GUIStyle nodeStyle = new GUIStyle();
            nodeStyle.normal.background = RootNormalNode;
            nodeStyle.alignment = TextAnchor.MiddleCenter;
            nodeStyle.normal.textColor = Color.white;
            nodeStyle.fontSize = 17;
            nodeStyle.border = new RectOffset(12, 12, 12, 12);
            return nodeStyle;
        }
    }
    public static GUIStyle RootSelectedStyle
    {
        get
        {
            GUIStyle nodeStyle = new GUIStyle();
            nodeStyle.normal.background = RootSelectedNode;
            nodeStyle.alignment = TextAnchor.MiddleCenter;
            nodeStyle.normal.textColor = Color.white;
            nodeStyle.fontSize = 17;
            nodeStyle.border = new RectOffset(12, 12, 12, 12);
            return nodeStyle;
        }
    }
    public static GUIStyle CompositeNormalStyle
    {
        get
        {
            GUIStyle nodeStyle = new GUIStyle();
            nodeStyle.normal.background = CompositeNormalNode;
            nodeStyle.alignment = TextAnchor.MiddleCenter;
            nodeStyle.normal.textColor = Color.white;
            nodeStyle.fontSize = 17;
            nodeStyle.border = new RectOffset(12, 12, 12, 12);
            return nodeStyle;
        }
    }
    public static GUIStyle CompositeSelectedStyle
    {
        get
        {
            GUIStyle nodeStyle = new GUIStyle();
            nodeStyle.normal.background = CompositeSelectedNode;
            nodeStyle.alignment = TextAnchor.MiddleCenter;
            nodeStyle.normal.textColor = Color.white;
            nodeStyle.fontSize = 17;
            nodeStyle.border = new RectOffset(12, 12, 12, 12);
            return nodeStyle;
        }
    }
    public static GUIStyle DecoratorNormalStyle
    {
        get
        {
            GUIStyle nodeStyle = new GUIStyle();
            nodeStyle.normal.background = DecoratorNormalNode;
            nodeStyle.alignment = TextAnchor.MiddleCenter;
            nodeStyle.normal.textColor = Color.white;
            nodeStyle.fontSize = 17;
            nodeStyle.border = new RectOffset(12, 12, 12, 12);
            return nodeStyle;
        }
    }
    public static GUIStyle DecoratorSelectedStyle
    {
        get
        {
            GUIStyle nodeStyle = new GUIStyle();
            nodeStyle.normal.background = DecoratorSelectedNode;
            nodeStyle.alignment = TextAnchor.MiddleCenter;
            nodeStyle.normal.textColor = Color.white;
            nodeStyle.fontSize = 17;
            nodeStyle.border = new RectOffset(12, 12, 12, 12);
            return nodeStyle;
        }
    }
    public static GUIStyle ActionNormalStyle
    {
        get
        {
            GUIStyle nodeStyle = new GUIStyle();
            nodeStyle.normal.background = ActionNormalNode;
            nodeStyle.alignment = TextAnchor.MiddleCenter;
            nodeStyle.normal.textColor = Color.white;
            nodeStyle.fontSize = 17;
            nodeStyle.border = new RectOffset(12, 12, 12, 12);
            return nodeStyle;
        }
    }
    public static GUIStyle ActionSelectedStyle
    {
        get
        {
            GUIStyle nodeStyle = new GUIStyle();
            nodeStyle.normal.background = ActionSelectedNode;
            nodeStyle.alignment = TextAnchor.MiddleCenter;
            nodeStyle.normal.textColor = Color.white;
            nodeStyle.fontSize = 17;
            nodeStyle.border = new RectOffset(12, 12, 12, 12);
            return nodeStyle;
        }
    }

    static Texture2D RootNormalNode
    {
        get
        {
            return Resources.Load<Texture2D>("0");
        }
    }
    static Texture2D RootSelectedNode
    {
        get
        {
            return Resources.Load<Texture2D>("0_1");
        }
    }
    static Texture2D CompositeNormalNode
    {
        get
        {
            return Resources.Load<Texture2D>("1");
        }
    }
    static Texture2D CompositeSelectedNode
    {
        get
        {
            return Resources.Load<Texture2D>("1_1");
        }
    }
    static Texture2D DecoratorNormalNode
    {
        get
        {
            return Resources.Load<Texture2D>("2");
        }
    }
    static Texture2D DecoratorSelectedNode
    {
        get
        {
            return Resources.Load<Texture2D>("2_1");
        }
    }
    static Texture2D ActionNormalNode
    {
        get
        {
            return Resources.Load<Texture2D>("3");
        }
    }
    static Texture2D ActionSelectedNode
    {
        get
        {
            return Resources.Load<Texture2D>("3_1");
        }
    }
}

public class NodeStatusStyles
{
    public GUIStyle NotWork;
    public GUIStyle Running;
    public GUIStyle Success;
    public GUIStyle Failure;
    public Rect Rect;
    public NodeStatusStyles(Rect rect)
    {
        this.Rect = rect;
        NotWork = new GUIStyle();
        NotWork.normal.background = NotWorkNode;
        Running = new GUIStyle();
        Running.normal.background = RunningNode;
        Success = new GUIStyle();
        Success.normal.background = SuccessNode;
        Failure = new GUIStyle();
        Failure.normal.background = FailureNode;
    }

    Texture2D NotWorkNode
    {
        get
        {
            return Resources.Load<Texture2D>("NotWorking_Icon");
        }
    }
    Texture2D RunningNode
    {
        get
        {
            return Resources.Load<Texture2D>("Running_Icon");
        }
    }
    Texture2D SuccessNode
    {
        get
        {
            return Resources.Load<Texture2D>("Success_Icon");
        }
    }
    Texture2D FailureNode
    {
        get
        {
            return Resources.Load<Texture2D>("Failure_Icon");
        }
    }
}
