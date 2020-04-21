using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleConfig : CsvCfg<RoleConfig>
{
    public string Name { get; protected set; }
    public int Hp { get; protected set; }
    public int Mp { get; protected set; }
    public int Attack { get; protected set; }
    public int MoveSpeed { get; protected set; }
    public int HitFlyShield { get; protected set; }
    public RoleType RoleType { get; protected set; }
    public static string FilePath = "Config/Role";
}

public enum RoleType
{
    Player = 0,
    Enemy = 1,
    Boss = 2,
}