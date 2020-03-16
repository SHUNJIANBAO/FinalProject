using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharHurtStatus : CharFsmBase
{
    E_HurtType m_HurtType;
    public CharHurtStatus(Character owner, Animator animator) : base(owner, animator)
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
        m_HurtType = (E_HurtType)objs[0];
        switch (m_HurtType)
        {
            case E_HurtType.Normal:
                m_Animator.SetInteger("Index", (int)E_AnimatorIndex.Hurt);
                break;
            case E_HurtType.HitFly:
                m_Animator.SetInteger("Index", (int)E_AnimatorIndex.HurtFlyStart);
                break;
        }
    }
}

public enum E_HurtType
{
    Normal,
    HitFly,
}