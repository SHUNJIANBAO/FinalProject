﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharHitFlyStatus : CharFsmBase
{
    bool _isComplete;
    bool _isFly;
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
        _isFly = false;
        m_Owner.IsInvincible = true;
        m_Animator.SetInteger("Index", (int)E_AnimatorIndex.HurtFlyStart);
    }

    protected override void OnStay()
    {
        base.OnStay();
        if (m_CurStateInfo.IsName(E_AnimatorIndex.HurtFlyStart.ToString()) && m_CurStateInfo.normalizedTime >= 1f)
        {
            m_Owner.Rigibody.velocity = m_Owner.IsFaceRight ? new Vector2(-3, 10) : new Vector2(3, 10);
            m_Animator.SetInteger("Index", (int)E_AnimatorIndex.HurtFlying);
        }
        else if (m_CurStateInfo.IsName(E_AnimatorIndex.HurtFlying.ToString()))
        {
            if (m_Owner.IsGround && _isFly)
            {
                m_Animator.SetInteger("Index", (int)E_AnimatorIndex.HurtFlyEnd);
            }
            else if (m_CurStateInfo.normalizedTime > 0.2f)
            {
                _isFly = true;
            }
        }
        else if (m_CurStateInfo.IsName(E_AnimatorIndex.HurtFlyEnd.ToString()) && m_CurStateInfo.normalizedTime >= 1f)
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
