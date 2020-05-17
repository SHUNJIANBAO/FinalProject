using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossConfig : CsvCfg<BossConfig>
{
    public string Name { get; protected set; }
    public int Hp { get; protected set; }
    public int Mp { get; protected set; }
    public int Attack { get; protected set; }
    public int MoveSpeed { get; protected set; }
    public int Shield { get; protected set; }
    public string FightBGM { get; protected set; }
    public static string FilePath = "Config/Boss";
}
