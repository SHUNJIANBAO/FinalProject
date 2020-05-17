using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterConfig : CsvCfg<MonsterConfig>
{
    public string Name { get; protected set; }
    public int Hp { get; protected set; }
    public int Mp { get; protected set; }
    public int Attack { get; protected set; }
    public int MoveSpeed { get; protected set; }
    public static string FilePath = "Config/Monster";

}
