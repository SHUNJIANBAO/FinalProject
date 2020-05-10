using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Node("技能节点")]
public class SkillNode : ActionNode
{
    public int SkillId;
    public bool BeForce = false;


    bool _skillEnd;
    /// <summary>
    /// 检测能否运行
    /// </summary>
    /// <returns></returns>
    protected override E_NodeStatus Trick()
    {
        if (_skillEnd)
        {
            _skillEnd = false;
            return E_NodeStatus.Success;
        }
        return E_NodeStatus.Running;
    }

    public override void OnEnter()
    {
        _skillEnd = false;
        m_Movement.Attack(SkillId, BeForce, null, () => _skillEnd = true);
    }

    public override void OnStay()
    {

    }

    public override void OnExit()
    {
    }
}
