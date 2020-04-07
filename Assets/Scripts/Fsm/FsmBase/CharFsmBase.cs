
using UnityEngine;

public abstract class CharFsmBase : FsmBase 
{
    protected Character m_Owner;
    protected Animator m_Animator;
    protected AnimatorStateInfo m_CurStateInfo;

    public CharFsmBase(Character owner,Animator animator)
    {
        this.m_Animator = animator;
        this.m_Owner = owner;
    }
    protected override void OnStay()
    {
        base.OnStay();
        m_CurStateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
    }
}
