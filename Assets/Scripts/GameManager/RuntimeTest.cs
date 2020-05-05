using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeTest : MonoSingleton<RuntimeTest>
{
    [Header("显示攻击伤害范围")]
    public bool DrawCollider;
    [Header("关闭行为树")]
    public bool CloseAI;

    [Header("时间速率")]
    [Range(0, 1)]
    public float TimeScale = 1;
}
