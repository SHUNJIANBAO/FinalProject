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


    [System.NonSerialized]
    EmitterManager _emitterManager;
    [System.NonSerialized]
    Vector3 _pos = Vector3.zero;

    public enum TargetPos
    {
        敌人位置,
        敌人X轴位置,
        固定位置,
    }

    [System.NonSerialized]
    bool _enter;
    [System.NonSerialized]
    Transform TargetTrans;
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
        _pos = Offest;
        TargetTrans = m_Owner.AttackTarget.transform;
    }

    public override void OnExit()
    {
    }

    public override void OnStay()
    {
        if (!_enter)
        {
            _enter = true;
            Transform parent = null;
            BarrageConfig bCfg = BarrageConfig.GetData(BarrageId);
            var bullet = ResourceManager.Load<GameObject>(PathManager.GetBulletPath("Bullet_" + BulletId));
            TimerManager.Instance.AddListener(Delay, () =>
            {
                switch (TargetPosType)
                {
                    case TargetPos.敌人位置:
                        parent = TargetTrans;
                        break;
                    case TargetPos.固定位置:
                        break;
                    case TargetPos.敌人X轴位置:
                        _pos.x = TargetTrans.position.x;
                        _pos.y = Offest.y;
                        break;
                }
                if (m_Owner==GameConfig.Player)
                {
                    _emitterManager = ShootManager.Instance.Shoot(parent, _pos, bullet, m_Owner.GetAttribute(E_Attribute.Atk.ToString()).GetTotalValue(), bCfg, GameConfig.Instance.EnemyLayer);
                }
                else
                {
                    _emitterManager = ShootManager.Instance.Shoot(parent, _pos, bullet, m_Owner.GetAttribute(E_Attribute.Atk.ToString()).GetTotalValue(), bCfg, GameConfig.Instance.PlayerLayer);
                }
                if (TargetPosType==TargetPos.敌人X轴位置)
                {
                    _emitterManager.OnUpdate += () =>
                    {
                        _pos.x = TargetTrans.position.x;
                        _pos.y = Offest.y;
                        _emitterManager.transform.position = _pos;
                    };
                }
            });
        }
    }
}
