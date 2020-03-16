using UnityEngine;

public class CharIdleStatus : CharFsmBase
{
    public CharIdleStatus(Character owner, Animator animator):base(owner,animator)
    {

    }

    public override bool CanEnter()
    {
        return true;
    }

    public override bool CanInterrupt()
    {
        return true;
    }

    public override bool CanExit()
    {
        return true;
    }

    protected override void OnEnter(params object[] objs)
    {
        base.OnEnter();
        m_Animator.SetInteger("Index", (int)E_AnimatorIndex.Idle);
    }

    protected override void OnStay()
    {
        base.OnStay();
    }

    protected override void OnExit()
    {
        base.OnExit();
    }

    protected override void OnInterrupt()
    {
        base.OnInterrupt();
    }

}
