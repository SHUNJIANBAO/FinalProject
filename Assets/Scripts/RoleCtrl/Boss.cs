using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : EnemyBase
{
    BossConfig _cfg;

    GameRangeAttribute _shield;
    protected GameRangeAttributeInstance m_Shield;

    protected override void RegistAttribute()
    {
        base.RegistAttribute();

        _cfg = BossConfig.GetData(Id);

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


        _shield = new GameRangeAttribute(E_Attribute.Shield.ToString(), 0, _cfg.Shield, 0.02f);
        m_Shield = AddRangeAttribute(_shield);


        UIBattleWindow.ShowBossUI();
        UIBattleWindow.OnBossHpChange(m_Hp.Current, m_Hp.GetMinTotalValue(), m_Hp.GetMaxTotalValue());
        UIBattleWindow.OnBossShieldChange(m_Shield.Current, m_Shield.GetMinTotalValue(), m_Shield.GetMaxTotalValue());
        GetRangeAttribute(E_Attribute.Hp.ToString()).OnValueChanged += (delta) => { UIBattleWindow.OnBossHpChange(m_Hp.Current, m_Hp.GetMinTotalValue(), m_Hp.GetMaxTotalValue()); };
        GetRangeAttribute(E_Attribute.Shield.ToString()).OnValueChanged += (delta) => { UIBattleWindow.OnBossShieldChange(m_Shield.Current, m_Shield.GetMinTotalValue(), m_Shield.GetMaxTotalValue()); };

        AudioManager.Instance.PlayBGM(_cfg.FightBGM);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (AttackTarget == null)
        {
            AttackTarget = GameConfig.Player;
        }
    }

    public override void Hurt(GameObject atkOwner, int damage, int hitForce)
    {
        base.Hurt(atkOwner, damage, hitForce);
        var shield = GetRangeAttribute(E_Attribute.Shield.ToString());
        shield.ChangeValue(-hitForce);

        if (shield.Current <= 0 && CurStatus != E_CharacterFsmStatus.FallDown)
        {
            LookToTarget(atkOwner.transform.position);
            ChangeStatus(E_CharacterFsmStatus.HitFly, true);
        }
    }

    public override void OnDestory()
    {
        base.OnDestory();
        UIBattleWindow.HideBossUI();

    }

}
