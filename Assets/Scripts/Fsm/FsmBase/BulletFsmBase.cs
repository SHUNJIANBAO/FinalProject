using UnityEngine;

public abstract class BulletFsmBase : FsmBase {

    protected BulletBase m_Owner;
    protected Animator m_Animator;
    protected AnimatorStateInfo m_CurStateInfo;

    public BulletFsmBase(BulletBase owner, Animator animator)
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
