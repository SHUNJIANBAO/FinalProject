using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffConfig : CsvCfg<BuffConfig>
{
    public string Name { get; protected set; }
    public int Value { get; protected set; }
    public BuffType BuffType { get; protected set; }
    public ValueType ValueType { get; protected set; }
    public int DurationTime { get; protected set; }

    public static string FilePath = "Config/Buff";

}

public enum ValueType
{
    Hp=0, //血量
    MaxHp=1, //最大血量
    Attack=2, //攻击力
    Defense=3, //防御力
}

public enum BuffType
{
    Once=0,  //一次性
    TimeLimit=1,  //限时
    Duration =2,  //持续增加
}
