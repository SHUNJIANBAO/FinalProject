using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRunningStatus : BulletFsmBase
{
    public BulletRunningStatus(BulletBase owner, Animator animator):base(owner,animator)
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
        m_Animator.SetInteger("Index", (int)E_AnimatorIndex.Moving);
    }

    protected override void OnStay()
    {
        base.OnStay();
        m_Owner.Move();
    }
}
