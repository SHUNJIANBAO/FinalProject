using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharMoveStatus : CharFsmBase 
{
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
        m_Animator.SetInteger("Index", (int)E_AnimatorIndex.Moving);
    }
    protected override void OnStay()
    {
        base.OnStay();
        if (Vector2.Distance( m_Owner.transform.position,m_Owner.MoveTarget)<0.1f)
        {
            m_Owner.ChangeStatus(E_CharacterFsmStatus.Idle);
        }
        else
        {
            m_Owner.transform.position = Vector3.MoveTowards(m_Owner.transform.position, m_Owner.MoveTarget, m_Owner.GetAttribute(E_Attribute.MoveSpeed.ToString()).GetTotalValue() * GameManager.DeltaTime);
        }
    }
}
