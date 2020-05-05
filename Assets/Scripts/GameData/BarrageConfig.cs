using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrageConfig : CsvCfg<BarrageConfig>
{
    public string Name { get; protected set; }
    public bool IsFollow { get; protected set; }
    public Vector2 Offest { get; protected set; }
    public E_BarrageType BarrageType { get; protected set; }
    public int Count { get; protected set; }
    public float BirthIntervalTime { get; protected set; }
    public float BirthIntervalDistance { get; protected set; }
    public int Wave { get; protected set; }
    public float ShootIntervalTime { get; protected set; }
    public E_Direction Direction { get; protected set; }

    public static string FilePath = "Config/Barrage";
}

public enum E_Direction
{
    Up=0,
    Down=1,
    Left=2,
    Right=3,
}

