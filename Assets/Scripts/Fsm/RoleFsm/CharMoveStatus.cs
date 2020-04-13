using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class CharMoveStatus : CharFsmBase
{
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
        if (objs!=null&&objs.Length>0)
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
    }
    protected override void OnStay()
    {
        base.OnStay();
        if (Vector2.Distance(m_Owner.transform.position, m_Owner.MoveTarget) < 0.1f)
        {
            m_Owner.ChangeStatus(E_CharacterFsmStatus.Idle);
        }
        else
        {
            _moveCallback?.Invoke(m_Owner);
            m_Owner.transform.position = Vector3.MoveTowards(m_Owner.transform.position, m_Owner.MoveTarget, m_Owner.GetAttribute(E_Attribute.MoveSpeed.ToString()).GetTotalValue() * _speedCurve * GameManager.DeltaTime);
        }
    }
}
