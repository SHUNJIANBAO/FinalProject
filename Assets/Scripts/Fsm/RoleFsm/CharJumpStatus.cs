using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharJumpStatus : CharFsmBase
{
    float _jumpForce;
    bool _isJumping;

    public CharJumpStatus(Character owner, Animator animator) : base(owner, animator)
    {

    }

    public override bool CanEnter()
    {
        return true;
    }

    public override bool CanExit()
    {
        return m_Owner.IsGround;
    }

    public override bool CanInterrupt()
    {
        return true;
    }
    protected override void OnEnter(params object[] objs)
    {
        base.OnEnter(objs);
        if (objs != null && objs.Length > 0)
        {
            _isJumping = false;
            _jumpForce = float.Parse(objs[0].ToString());
            m_Animator.SetInteger("Index", (int)E_AnimatorIndex.JumpStart);
        }
        else if(!m_Owner.IsGround)
        {
            m_Animator.SetInteger("Index", (int)E_AnimatorIndex.JumpingDown);
        }
    }

    protected override void OnStay()
    {
        base.OnStay();
        if (m_CurStateInfo.IsName(E_AnimatorIndex.JumpStart.ToString()))
        {
            if (m_CurStateInfo.normalizedTime >= 1f && !m_Owner.IsGround)
            {
                m_Animator.SetInteger("Index", (int)E_AnimatorIndex.JumpingUp);
                m_Owner.Rigibody.velocity = new Vector2(0, _jumpForce);
            }
        }
        else
        {
            if (!m_Owner.IsGround)
            {
                _isJumping = true;
                if (m_CurStateInfo.IsName(E_AnimatorIndex.JumpingUp.ToString()))
                {
                    if (m_CurStateInfo.normalizedTime>=1f)
                    {
                        m_Animator.SetInteger("Index", (int)E_AnimatorIndex.JumpingDown);
                    }
                }
            }
            else if (_isJumping)
            {
                m_Owner.ChangeStatus(E_CharacterFsmStatus.Idle);
            }
        }
    }

    protected override void OnExit()
    {
        base.OnExit();
        _isJumping = false;
    }
}
