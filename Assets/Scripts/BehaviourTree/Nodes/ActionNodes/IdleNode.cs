using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Node("待机")]
public class IdleNode : ActionNode
{
    public float DurationTime;

    [System.NonSerialized]
    float timeCount;

    public override void OnEnter()
    {
        timeCount = 0;
        m_Owner.ChangeStatus(E_CharacterFsmStatus.Idle);
    }

    public override void OnExit()
    {
        timeCount = 0;
    }

    public override void OnStay()
    {
        if (m_Owner.CurStatus == E_CharacterFsmStatus.Idle)
        {
            timeCount += Time.deltaTime;
        }
        if (m_Owner.CurStatus != E_CharacterFsmStatus.Idle)
        {
            m_Owner.ChangeStatus(E_CharacterFsmStatus.Idle);

        }
    }

    protected override E_NodeStatus Trick()
    {
        if (timeCount >= DurationTime)
        {
            return E_NodeStatus.Success;
        }
        return E_NodeStatus.Running;
    }
}
