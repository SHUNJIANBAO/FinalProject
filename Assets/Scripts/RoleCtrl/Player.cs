using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    PlayerConfig _cfg;

    protected override void RegistAttribute()
    {
        base.RegistAttribute();

        _cfg = PlayerConfig.GetData(Id);

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


        UIBattleWindow.OnPlayerHpChange(m_Hp.Current, m_Hp.GetMinTotalValue(), m_Hp.GetMaxTotalValue());
        UIBattleWindow.OnPlayerMpChange(m_Mp.Current, m_Mp.GetMinTotalValue(), m_Mp.GetMaxTotalValue());
        GetRangeAttribute(E_Attribute.Hp.ToString()).OnValueChanged += (delta) => { UIBattleWindow.OnPlayerHpChange(m_Hp.Current, m_Hp.GetMinTotalValue(), m_Hp.GetMaxTotalValue()); };
        GetRangeAttribute(E_Attribute.Mp.ToString()).OnValueChanged += (delta) => { UIBattleWindow.OnPlayerMpChange(m_Mp.Current, m_Mp.GetMinTotalValue(), m_Mp.GetMaxTotalValue()); };

        GameConfig.Player = this;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void Hurt(GameObject atkOwner, int damage, int hitForce)
    {
        if (IsInvincible) return;
        if (CurStatus == E_CharacterFsmStatus.FallDown) return;
        base.Hurt(atkOwner, damage, hitForce);
        LookToTarget(atkOwner.transform.position);

        if (hitForce > 0)
        {
            ChangeStatus(E_CharacterFsmStatus.HitFly, true);
        }
        else
        {
            ChangeStatus(E_CharacterFsmStatus.Hurt, true);
        }

    }
}
