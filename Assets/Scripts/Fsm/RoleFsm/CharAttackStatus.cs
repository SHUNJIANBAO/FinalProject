using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharAttackStatus : CharFsmBase
{
    public CharAttackStatus(Character owner, Animator animator) : base(owner, animator)
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
        int animId = 0;
        m_Animator.SetInteger("Index", animId);
    }
}
