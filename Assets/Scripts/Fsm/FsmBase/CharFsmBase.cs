
using UnityEngine;

public abstract class CharFsmBase : FsmBase 
{
    protected Character m_Owner;
    protected Animator m_Animator;

    public CharFsmBase(Character owner,Animator animator)
    {
        this.m_Animator = animator;
        this.m_Owner = owner;
    }
}
