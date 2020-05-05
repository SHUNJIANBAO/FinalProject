using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GameConfig : MonoSingleton<GameConfig>
{
    [HideInInspector]
    public LayerMask PlaneMask;//地面层
    [HideInInspector]
    public int PlaneLayer;//地面层
    [HideInInspector]
    public LayerMask PlayerMask;//玩家
    [HideInInspector]
    public int PlayerLayer;//玩家
    [HideInInspector]
    public LayerMask EnemyMask;//敌人
    [HideInInspector]
    public int EnemyLayer;//敌人

    private void Awake()
    {
        PlaneMask = LayerMask.GetMask("Plane");
        PlaneLayer = LayerMask.NameToLayer("Plane");
        PlayerMask = LayerMask.GetMask("Player");
        PlayerLayer = LayerMask.NameToLayer("Player");
        EnemyMask = LayerMask.GetMask("Enemy");
        EnemyLayer = LayerMask.NameToLayer("Enemy");
    }

#if UNITY_EDITOR
    public Texture2D Texture;
    public Texture2D ClickTexture;
    GUIStyle _style=new GUIStyle();
    Rect _rect = new Rect(Vector2.zero + Vector2.one * 10, new Vector2(250, 60));
    GUIContent _content = new GUIContent("加载配置");
    private void OnGUI()
    {
        _style.fontSize = 28;
        _style.active.background = ClickTexture;
        _style.normal.background = Texture;
        _style.alignment = TextAnchor.MiddleCenter;
        if (GUI.Button(_rect, _content, _style))
        {
            Main.Instance.LoadConfigs();
            Debug.Log("加载成功");
        }
    }
#endif
}
