using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharDieStatus : CharFsmBase
{
    public CharDieStatus(Character owner, Animator animator) : base(owner, animator)
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
        return false;
    }
    protected override void OnEnter(params object[] objs)
    {
        base.OnEnter(objs);
        m_Animator.SetInteger("Index", (int)E_AnimatorIndex.Die);
    }

    protected override void OnStay()
    {
        base.OnStay();
        if (m_CurStateInfo.IsName(E_AnimatorIndex.Die.ToString())&&m_CurStateInfo.normalizedTime>=1f)
        {
            m_Owner.OnDestory();
        }
    }
}
