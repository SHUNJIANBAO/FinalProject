﻿

using UnityEngine;

[Node("移动/移动到目标位置")]
public class MoveToTargetNode : ActionNode
{
    public float UntilDistance;

    public override void OnEnter()
    {
        
    }

    public override void OnExit()
    {
        
    }

    public override void OnStay()
    {
        m_Movement.MoveToPoint(m_Owner.AttackTarget.transform.position);
    }

    protected override E_NodeStatus Trick()
    {
        if (Vector2.Distance(m_Owner.transform.position,m_Owner.AttackTarget.transform.position)>UntilDistance)
        {
            return E_NodeStatus.Running;
        }
        return E_NodeStatus.Success;
    }
}