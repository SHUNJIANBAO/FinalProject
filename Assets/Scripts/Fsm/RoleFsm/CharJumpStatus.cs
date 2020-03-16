﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharJumpStatus : CharFsmBase
{
    public CharJumpStatus(Character owner, Animator animator) : base(owner, animator)
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
        m_Animator.SetInteger("Index", (int)E_AnimatorIndex.JumpStart);
    }
}
