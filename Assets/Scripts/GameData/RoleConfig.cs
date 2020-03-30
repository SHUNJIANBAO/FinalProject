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
    public bool IsBoss { get; protected set; }
    public static string FilePath = "Config/Role";
}

