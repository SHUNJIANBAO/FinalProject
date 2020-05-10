using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillConfig : CsvCfg<SkillConfig>
{
    public string Name { get; protected set; }
    public int AnimId { get; protected set; }
    public List<int> ColliderId { get; protected set; }
    public List<int> BarrageId { get; protected set; }
    public int BulletId { get; protected set; }
    public int NextSkillId { get; protected set; }
    public int UseMp { get; protected set; }
    public List<int> HitFlyForce { get; protected set; }
    public List<float> DamageRatio { get; protected set; }
    public List<float> DamageTime { get; protected set; }
    public float ComboTime { get; protected set; }
    public float CanExitTime { get; protected set; }
    public float InvincibleTime { get; protected set; }
    public float InvincibleDuration { get; protected set; }
    public List<float> MoveDistance { get; protected set; }
    public List<float> MoveStartTime { get; protected set; }
    public List<float> MoveDuration { get; protected set; }
    public RoleType RoleType { get; protected set; }
    public string HitEffect { get; protected set; }
    public E_HitEffectPosType HitEffectPosType { get; protected set; }
    public static string FilePath = "Config/Skill";

}
public enum E_HitEffectPosType
{
    HitPoint = 0,
    CharacterCenter = 1,
}
