using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEndStatus : BulletFsmBase
{
    public BulletEndStatus(BulletBase owner, Animator animator) : base(owner, animator)
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
        m_Animator.SetInteger("Index", (int)E_AnimatorIndex.MoveEnd);
    }

    protected override void OnStay()
    {
        base.OnStay();
        if (m_CurStateInfo.IsName(E_AnimatorIndex.MoveEnd.ToString())&&m_CurStateInfo.normalizedTime>=1)
        {
            MonoBehaviourManager.Remove(m_Owner);
            PoolManager.DestroyGameObject(m_Owner.gameObject, PoolType.Bullet);
        }
    }
}
