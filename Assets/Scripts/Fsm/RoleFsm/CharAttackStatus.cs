using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharAttackStatus : CharFsmBase
{
    float _timeCount;
    SkillConfig _skill;
    string _skillName;
    public CharAttackStatus(Character owner, Animator animator) : base(owner, animator)
    {

    }

    public override bool CanEnter()
    {
        return true;
    }

    public override bool CanExit()
    {
        return _timeCount >= _skill.CanExitTime;
    }

    public override bool CanInterrupt()
    {
        return _timeCount >= _skill.ComboTime;
    }
    protected override void OnEnter(params object[] objs)
    {
        base.OnEnter(objs);
        _timeCount = 0;
        _skill = m_Owner.CurSkill;
        _skillName = AnimConfig.GetData(_skill.AnimId).Name;
        m_Animator.SetInteger("Index", _skill.AnimId);
    }

    protected override void OnStay()
    {
        base.OnStay();
        if (m_CurStateInfo.IsName(_skillName))
        {
            _timeCount += GameManager.DeltaTime;


            if (m_CurStateInfo.normalizedTime>=1f)
            {
                m_Owner.ChangeStatus(E_CharacterFsmStatus.Idle);
            }
        }
    }
}
