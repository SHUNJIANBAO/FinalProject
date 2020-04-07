using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharHurtStatus : CharFsmBase
{
    bool _isComplete;
    public CharHurtStatus(Character owner, Animator animator) : base(owner, animator)
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
        if (m_CurStateInfo.IsName(E_AnimatorIndex.Hurt.ToString()))
            m_Animator.Play(E_AnimatorIndex.Hurt.ToString(), 0, 0);
        else
            m_Animator.SetInteger("Index", (int)E_AnimatorIndex.Hurt);
    }

    protected override void OnStay()
    {
        base.OnStay();
        if (m_CurStateInfo.IsName(E_AnimatorIndex.Hurt.ToString()) && m_CurStateInfo.normalizedTime >= 1f)
        {
            _isComplete = true;
            m_Owner.ChangeStatus(E_CharacterFsmStatus.Idle);
        }
    }
    protected override void OnExit()
    {
        base.OnExit();
    }
}
