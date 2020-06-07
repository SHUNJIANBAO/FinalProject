using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharJumpStatus : CharFsmBase
{
    float _jumpForce;
    bool _isJumping;
    Action<Character> _jumpDownCallback;

    float _lastY;

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
        if (m_Owner.IsGround)
        {
            return m_CurStateInfo.IsName(E_AnimatorIndex.JumpEnd.ToString()) && m_CurStateInfo.normalizedTime > 0.2f;
        }
        return true;
    }

    public override bool CanInterrupt()
    {
        return true;
    }
    protected override void OnEnter(params object[] objs)
    {
        base.OnEnter(objs);
        m_Owner.MoveTarget = m_Owner.transform.position;
        if (!m_Owner.IsGround)
        {
            _isJumping = true;
            m_Animator.SetInteger("Index", (int)E_AnimatorIndex.JumpingDown);
        }
        if (objs != null)
        {
            if (objs.Length > 0)
            {
                _isJumping = false;
                _jumpForce = float.Parse(objs[0].ToString());
                m_Animator.SetInteger("Index", (int)E_AnimatorIndex.JumpStart);
            }
            if (objs.Length > 1)
            {
                _jumpDownCallback = objs[1] as Action<Character>;
            }
        }
        _lastY = m_Owner.transform.position.y;
    }
    
    protected override void OnStay()
    {
        base.OnStay();
        if (m_Animator.GetInteger("Index") == (int)E_AnimatorIndex.JumpStart)
        {
            if (m_CurStateInfo.normalizedTime >= 1f)
            {
                m_Animator.SetInteger("Index", (int)E_AnimatorIndex.JumpingUp);
                m_Owner.Rigibody.velocity += new Vector2(0, _jumpForce);
                if (m_Owner.IsGround)
                {
                    //m_Owner.Rigibody.velocity += new Vector2(0, _jumpForce);
                }
                else
                {
                    m_Owner.MoveTarget += m_Owner.IsFaceRight ? new Vector3(_jumpForce / 3, 0, 0) : new Vector3(-_jumpForce / 3, 0, 0);
                    //m_Owner.Rigibody.velocity += m_Owner.IsFaceRight ? new Vector2(_jumpForce / 3, _jumpForce) : new Vector2(-_jumpForce / 3, _jumpForce);
                }
            }
            else
            {
                m_Owner.Rigibody.velocity = Vector2.zero;
            }
        }
        else 
        {
            if (!m_Owner.IsGround)
            {
                //跳跃时做x轴的位移，到指定点
                m_Owner.MoveTarget.y = m_Owner.transform.position.y;
                //if (Mathf.Abs(m_Owner.transform.position.x- m_Owner.MoveTarget.x)>0.1f)
                //{
                //    Vector3 dir = m_Owner.transform.position.x > m_Owner.MoveTarget.x ? Vector3.left : Vector3.right;
                //    m_Owner.Rigibody.v(m_Owner.transform.position + dir * m_Owner.GetAttribute(E_Attribute.MoveSpeed.ToString()).GetTotalValue() * m_Animator.speed * Time.deltaTime);
                //}
                m_Owner.transform.position = Vector3.MoveTowards(m_Owner.transform.position, m_Owner.MoveTarget, m_Owner.GetAttribute(E_Attribute.MoveSpeed.ToString()).GetTotalValue() * _jumpMoveSpeedRatio * m_Animator.speed * Time.deltaTime);
                if (Mathf.Abs(m_Owner.transform.position.x - m_Owner.MoveTarget.x) > 0.1f)
                {
                    m_Owner.LookToTarget(m_Owner.MoveTarget);
                }


                _isJumping = true;
                if (m_CurStateInfo.IsName(E_AnimatorIndex.JumpingUp.ToString()))
                {
                    if (m_CurStateInfo.normalizedTime >= 1f)
                    {
                        m_Animator.SetInteger("Index", (int)E_AnimatorIndex.JumpingDown);
                    }
                }
            }
            else if(_isJumping)
            {
                //if (m_CurStateInfo.IsName(E_AnimatorIndex.JumpEnd.ToString()))
                if (m_Animator.GetInteger("Index") == (int)E_AnimatorIndex.JumpEnd)
                {
                    if (m_CurStateInfo.normalizedTime >= 1f)
                    {
                        m_Owner.ChangeStatus(E_CharacterFsmStatus.Idle);
                    }
                }
                else  if(m_Owner.transform.position.y<_lastY) //if (m_CurStateInfo.IsName(E_AnimatorIndex.JumpingDown.ToString()))
                {
                    _jumpDownCallback?.Invoke(m_Owner);
                    m_Owner.MoveTarget = m_Owner.transform.position;
                    m_Animator.SetInteger("Index", (int)E_AnimatorIndex.JumpEnd);
                    return;
                }
                else if (m_Owner.transform.position.y == _lastY)
                {
                    m_Owner.transform.position = Vector3.MoveTowards(m_Owner.transform.position, m_Owner.MoveTarget, m_Owner.GetAttribute(E_Attribute.MoveSpeed.ToString()).GetTotalValue() * _jumpMoveSpeedRatio * m_Animator.speed * Time.deltaTime);
                    return;
                }
            }
        }
        _lastY = m_Owner.transform.position.y;
    }

    protected override void OnExit()
    {
        base.OnExit();
        _isJumping = false;
    }
}
