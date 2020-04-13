using System.Collections.Generic;
using UnityEngine;

public class ColliderConfig : CsvCfg<ColliderConfig>
{
    public string Name { get; protected set; }
    public Vector2 Size { get; protected set; }
    public Vector2 Offest { get; protected set; }
    public E_ColliderFollowType LifeType { get; protected set; }
    public E_DamageType DamageType { get; protected set; }
    public float DamageInterval { get; protected set; }
    public float LifeTime { get; protected set; }
    public bool IsAttacker { get; protected set; }

    public static string FilePath = "Config/Collider";

}

public enum E_ColliderFollowType
{
    None = 0,
    Follow = 1,
}

public enum E_DamageType
{
    Once = 0,
    Enter = 1,
    Repeat = 2,
}