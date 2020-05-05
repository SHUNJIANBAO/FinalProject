using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Node("弹幕节点")]
public class BarrageNode : ActionNode
{
    public int BarrageId;
    public int BulletId;
    public TargetPos TargetPosType;
    public Vector3 Offest;

    public float Delay;

    public enum TargetPos
    {
        敌人位置,
        固定位置,
    }

    [System.NonSerialized]
    bool _enter;
    protected override E_NodeStatus Trick()
    {
        if (_enter)
        {
            _enter = false;
            return E_NodeStatus.Success;
        }
        return E_NodeStatus.Running;
    }

    public override void OnEnter()
    {

    }

    public override void OnExit()
    {
    }

    public override void OnStay()
    {
        if (!_enter)
        {
            _enter = true;
            Vector3 pos = Offest;
            Transform parent = null;
            switch (TargetPosType)
            {
                case TargetPos.敌人位置:
                    parent = m_Owner.AttackTarget.transform;
                    break;
                case TargetPos.固定位置:
                    break;
            }
            BarrageConfig bCfg = BarrageConfig.GetData(BarrageId);
            var bullet = ResourceManager.Load<GameObject>(PathManager.GetBulletPath("Bullet_" + BulletId));
            TimerManager.Instance.AddListener(Delay, () =>
            {
                if (m_Owner.RoleType == RoleType.Player)
                {
                    ShootManager.Instance.Shoot(parent, pos, bullet, m_Owner.GetAttribute(E_Attribute.Atk.ToString()).GetTotalValue(), bCfg, GameConfig.Instance.EnemyLayer);
                }
                else
                {
                    ShootManager.Instance.Shoot(parent, pos, bullet, m_Owner.GetAttribute(E_Attribute.Atk.ToString()).GetTotalValue(), bCfg, GameConfig.Instance.PlayerLayer);
                }
            });
        }
    }
}
