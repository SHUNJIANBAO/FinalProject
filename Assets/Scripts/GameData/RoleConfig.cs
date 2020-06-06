using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleConfig<T> : CsvCfg<T> where T:new()
{
    public string Name { get; protected set; }
    public int Hp { get; protected set; }
    public int Mp { get; protected set; }
    public int Attack { get; protected set; }
    public int MoveSpeed { get; protected set; }
    public string RunAudio { get; protected set; }
}
