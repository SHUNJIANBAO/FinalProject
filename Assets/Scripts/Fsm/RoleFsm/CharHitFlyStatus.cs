using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharHitFlyStatus : CharFsmBase
{
    bool _isComplete;

    public CharHitFlyStatus(Character owner, Animator animator) : base(owner, animator)
    {

    }

    public override bool CanEnter()
    {
        return true;
    }

    public override bool CanExit()
    {
        return _isComplete;
    }

    public override bool CanInterrupt()
    {
        return false;
    }
    protected override void OnEnter(params object[] objs)
    {
        base.OnEnter(objs);
        _isComplete = false;
        m_Owner.IsInvincible = true;
        m_Animator.SetInteger("Index", (int)E_AnimatorIndex.HurtFlyStart);
    }

    protected override void OnStay()
    {
        base.OnStay();
        if (m_CurStateInfo.IsName(E_AnimatorIndex.HurtFlyStart.ToString())&&m_CurStateInfo.normalizedTime>=1f)
        {
            m_Animator.SetInteger("Index", (int)E_AnimatorIndex.HurtFlying);
        }
        else if (m_CurStateInfo.IsName(E_AnimatorIndex.HurtFlying.ToString()))
        {
            if (m_Owner.IsGround)
            {
                m_Animator.SetInteger("Index", (int)E_AnimatorIndex.HurtFlyEnd);
            }
        }
        else if (m_CurStateInfo.IsName(E_AnimatorIndex.HurtFlyEnd.ToString())&&m_CurStateInfo.normalizedTime>=1f)
        {
            _isComplete = true;
            m_Owner.ChangeStatus(E_CharacterFsmStatus.FallDown);
        }

    }

    protected override void OnExit()
    {
        base.OnExit();
        m_Owner.IsInvincible = false;
    }

}
