using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class CharMoveStatus : CharFsmBase
{
    string _runAudio;
    float _speedCurve;
    Action<Character> _moveCallback;

    public CharMoveStatus(Character owner, Animator animator) : base(owner, animator)
    {

    }

    public override bool CanEnter()
    {
        return true;
    }

    public override bool CanExit()
    {
        return true;
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
            _moveCallback = (Action<Character>)objs[0];
        }
        else
        {
            _moveCallback = null;
        }
        m_Animator.SetInteger("Index", (int)E_AnimatorIndex.Moving);
        _speedCurve = 0;
        DOTween.To(() => _speedCurve, s => _speedCurve = s, 1, 0.15f);
        _runAudio = "";
        if (m_Owner.GetType().IsAssignableFrom(typeof(Player)))
        {
            _runAudio = PlayerConfig.GetData(m_Owner.Id).RunAudio;
        }
        else if (m_Owner.GetType().IsAssignableFrom(typeof(Monster)))
        {
            _runAudio = PlayerConfig.GetData(m_Owner.Id).RunAudio;
        }
        if (!string.IsNullOrEmpty(_runAudio))
        {
            AudioManager.Instance.PlayAudio(_runAudio, null, true);
        }
    }
    protected override void OnStay()
    {
        base.OnStay();
        if (Mathf.Abs(m_Owner.transform.position.x - m_Owner.MoveTarget.x) < 0.1f)
        {
            m_Owner.ChangeStatus(E_CharacterFsmStatus.Idle);
        }
        else
        {
            _moveCallback?.Invoke(m_Owner);
            if (m_Owner.IsFaceRight)
            {
                if (m_Owner == GameManager.Player)
                {
                    m_Owner.Rigibody.MovePosition(m_Owner.transform.position + Vector3.right * m_Owner.GetAttribute(E_Attribute.MoveSpeed.ToString()).GetTotalValue() * _speedCurve * m_Animator.speed * Time.deltaTime);
                }
                else
                {
                    m_Owner.transform.Translate(Vector3.right * m_Owner.GetAttribute(E_Attribute.MoveSpeed.ToString()).GetTotalValue() * _speedCurve * m_Animator.speed * Time.deltaTime);
                }
            }
            else
            {
                if (m_Owner == GameManager.Player)
                {
                    m_Owner.Rigibody.MovePosition(m_Owner.transform.position + Vector3.left * m_Owner.GetAttribute(E_Attribute.MoveSpeed.ToString()).GetTotalValue() * _speedCurve * m_Animator.speed * Time.deltaTime);
                }
                else
                {
                    m_Owner.transform.Translate(Vector3.left * m_Owner.GetAttribute(E_Attribute.MoveSpeed.ToString()).GetTotalValue() * _speedCurve * m_Animator.speed * Time.deltaTime);
                }
            }

            //m_Owner.transform.position = Vector3.MoveTowards(m_Owner.transform.position, m_Owner.MoveTarget, m_Owner.GetAttribute(E_Attribute.MoveSpeed.ToString()).GetTotalValue() * _speedCurve * m_Animator.speed * Time.deltaTime);
        }
    }

    protected override void OnExit()
    {
        base.OnExit();
        AudioManager.Instance.StopAudio(_runAudio);

    }
}
