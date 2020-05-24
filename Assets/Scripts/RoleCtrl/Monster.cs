using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : EnemyBase
{
    MonsterConfig _cfg;
    public float SeekDistance;

    protected override void RegistAttribute()
    {
        base.RegistAttribute();
        _cfg = MonsterConfig.GetData(Id);

        gameObject.name = _cfg.Name;
        var _hp = new GameRangeAttribute(E_Attribute.Hp.ToString(), 0, _cfg.Hp);
        m_Hp = AddRangeAttribute(_hp);
        var _mp = new GameRangeAttribute(E_Attribute.Mp.ToString(), 0, _cfg.Mp, 0.1f);
        m_Mp = AddRangeAttribute(_mp);
        var _attack = new GameAttribute(E_Attribute.Atk.ToString(), _cfg.Attack);
        m_Attack = AddAttribute(_attack);
        var _moveSpeed = new GameAttribute(E_Attribute.MoveSpeed.ToString(), _cfg.MoveSpeed);
        m_MoveSpeed = AddAttribute(_moveSpeed);

        m_CaculateDelta = TimerManager.Instance.AddListener(0, 0.02f, m_MonoAttribute.CaculateRangeDelta, null, true);
        AttackTarget = GameManager.Player;

    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (AttackTarget == null)
        {
            AttackTarget = GameManager.Player;
        }
    }

    public override void Hurt(GameObject atkOwner, int damage, int hitForce)
    {
        base.Hurt(atkOwner, damage, hitForce);
       

        if (hitForce > 0)
        {
            if (CheckCanChangeStatus(E_CharacterFsmStatus.HitFly))
            {
                LookToTarget(atkOwner.transform.position);
            }
            ChangeStatus(E_CharacterFsmStatus.HitFly);
        }
        else
        {
            if (CheckCanChangeStatus(E_CharacterFsmStatus.Hurt))
            {
                LookToTarget(atkOwner.transform.position);
            }
            ChangeStatus(E_CharacterFsmStatus.Hurt);
        }

    }

}
