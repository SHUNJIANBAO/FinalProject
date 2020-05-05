using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletConfig : CsvCfg<BulletConfig>
{
    public string Name { get; protected set; }
    public float MoveSpeed { get; protected set; }
    public E_BulletType Type { get; protected set; }
    public float Life { get; protected set; }
    public int HitForce { get; protected set; }
    public string DamageEffect { get; protected set; }
    public string DestroyEffect { get; protected set; }

    public static string FilePath = "Config/Bullet";

}

public enum E_BulletType
{
    Collide=0,
    Trigger=1,
    CollidePlane=2,
}