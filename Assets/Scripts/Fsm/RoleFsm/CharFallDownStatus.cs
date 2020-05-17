using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharFallDownStatus : CharFsmBase
{
    public CharFallDownStatus(Character owner, Animator animator) : base(owner, animator)
    {

    }
    bool _complete;
    public override bool CanEnter()
    {
        return true;
    }

    public override bool CanExit()
    {
        return _complete;
    }

    public override bool CanInterrupt()
    {
        return true;
    }
    protected override void OnEnter(params object[] objs)
    {
        base.OnEnter(objs);
        _complete = false;
        m_Animator.SetInteger("Index", (int)E_AnimatorIndex.FallDown);
    }

    protected override void OnStay()
    {
        base.OnStay();
        if (m_CurStateInfo.IsName(E_AnimatorIndex.FallDown.ToString()) && m_CurStateInfo.normalizedTime >= 1)
        {
            m_Animator.SetInteger("Index", (int)E_AnimatorIndex.StandUp);
        }
        if (m_CurStateInfo.IsName(E_AnimatorIndex.StandUp.ToString()) && m_CurStateInfo.normalizedTime >= 1)
        {
            _complete = true;
            m_Owner.ChangeStatus(E_CharacterFsmStatus.Idle);
        }
    }

    protected override void OnExit()
    {
        base.OnExit();
        var shield = m_Owner.GetRangeAttribute(E_Attribute.Shield.ToString());
        shield?.Reset();
    }
}
