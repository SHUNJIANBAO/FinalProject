using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharJumpStatus : CharFsmBase
{
    float _jumpForce;
    bool _isJumping;

    float _jumpMoveSpeedRatio = 0.65f;

    public CharJumpStatus(Character owner, Animator animator) : base(owner, animator)
    {

    }

    public override bool CanEnter()
    {
        return true;
    }

    public override bool CanExit()
    {
        if (!_isJumping) return false;
        m_CurStateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
        //if (m_CurStateInfo.IsName(E_AnimatorIndex.JumpEnd.ToString()))
        //{
        //    return m_CurStateInfo.normalizedTime >1f;
        //}
        return m_CurStateInfo.IsName(E_AnimatorIndex.JumpEnd.ToString())&& m_CurStateInfo.normalizedTime > 0.2f;
    }

    public override bool CanInterrupt()
    {
        return true;
    }
    protected override void OnEnter(params object[] objs)
    {
        base.OnEnter(objs);
        m_Owner.MoveTarget = m_Owner.transform.position;
        if (objs != null && objs.Length > 0)
        {
            _isJumping = false;
            _jumpForce = float.Parse(objs[0].ToString());
            m_Animator.SetInteger("Index", (int)E_AnimatorIndex.JumpStart);
        }
        else if (!m_Owner.IsGround)
        {
            m_Animator.SetInteger("Index", (int)E_AnimatorIndex.JumpingDown);
        }
    }

    protected override void OnStay()
    {
        base.OnStay();
        if (m_CurStateInfo.IsName(E_AnimatorIndex.JumpStart.ToString()))
        {
            if (m_CurStateInfo.normalizedTime >= 1f)
            {
                m_Animator.SetInteger("Index", (int)E_AnimatorIndex.JumpingUp);
                m_Owner.Rigibody.velocity += new Vector2(0, _jumpForce);
            }
        }
        else
        {
            if (!m_Owner.IsGround)
            {
                //跳跃时做x轴的位移，到指定点
                m_Owner.MoveTarget.y = m_Owner.transform.position.y;
                m_Owner.transform.position = Vector3.MoveTowards(m_Owner.transform.position, m_Owner.MoveTarget, m_Owner.GetAttribute(E_Attribute.MoveSpeed.ToString()).GetTotalValue() * _jumpMoveSpeedRatio * GameManager.DeltaTime);


                _isJumping = true;
                if (m_CurStateInfo.IsName(E_AnimatorIndex.JumpingUp.ToString()))
                {
                    if (m_CurStateInfo.normalizedTime >= 1f)
                    {
                        m_Animator.SetInteger("Index", (int)E_AnimatorIndex.JumpingDown);
                    }
                }
            }
            else if (_isJumping)
            {
                if (m_CurStateInfo.IsName(E_AnimatorIndex.JumpEnd.ToString()))
                {
                    if (m_CurStateInfo.normalizedTime >= 1f)
                    {
                        m_Owner.ChangeStatus(E_CharacterFsmStatus.Idle);
                    }
                }
                else //if (m_CurStateInfo.IsName(E_AnimatorIndex.JumpingDown.ToString()))
                {
                    m_Animator.SetInteger("Index", (int)E_AnimatorIndex.JumpEnd);
                }
            }
        }
    }

    protected override void OnExit()
    {
        base.OnExit();
        _isJumping = false;
    }
}
