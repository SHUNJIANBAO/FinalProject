using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillConfig : CsvCfg<SkillConfig>
{
    public string Name { get; protected set; }
    public int AnimId { get; protected set; }
    public int ColliderId { get; protected set; }
    public int BulletId { get; protected set; }
    public int NextSkillId { get; protected set; }
    public int UseMp { get; protected set; }
    public int HitFlyForce { get; protected set; }
    public int DamageRatio { get; protected set; }
    public float DamageTime { get; protected set; }
    public float ComboTime { get; protected set; }
    public float CanExitTime { get; protected set; }
    public float InvincibleTime { get; protected set; }
    public float InvincibleDuration { get; protected set; }
    public float MoveDistance { get; protected set; }
    public float MoveStartTime { get; protected set; }
    public float MoveDuration { get; protected set; }
    public RoleType RoleType { get; protected set; }
    public static string FilePath = "Config/Skill";

}
