using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig : MonoSingleton<GameConfig> 
{
    [HideInInspector]
    public LayerMask Plane;//地面层
    [HideInInspector]
    public LayerMask Player;//玩家
    [HideInInspector]
    public LayerMask Enemy;//敌人

    private void Awake()
    {
        Plane = LayerMask.GetMask("Plane");
        Player = LayerMask.GetMask("Player");
        Enemy = LayerMask.GetMask("Enemy");
    }
}
