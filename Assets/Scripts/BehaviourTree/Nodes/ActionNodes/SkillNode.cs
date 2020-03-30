using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Node("技能节点")]
public class SkillNode : ActionNode
{
    public int SkillId;
    public bool BeForce=false;

    /// <summary>
    /// 检测能否运行
    /// </summary>
    /// <returns></returns>
    protected override E_NodeStatus Trick()
    {
        return E_NodeStatus.Success;
    }

    public override void OnEnter()
    {
        m_Movement.Attack(SkillId, BeForce);
    }

    public override void OnStay()
    {
        
    }

    public override void OnExit()
    {

    }
}
