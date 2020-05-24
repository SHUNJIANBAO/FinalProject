using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Node("技能节点")]
public class SkillNode : ActionNode
{
    public int SkillId;
    public bool BeForce = false;

    [System.NonSerialized]
    bool _skillEnd;
    [System.NonSerialized]
    bool _enter;
    /// <summary>
    /// 检测能否运行
    /// </summary>
    /// <returns></returns>
    protected override E_NodeStatus Trick()
    {
        if (_skillEnd)
        {
            _skillEnd = false;
            _enter = false;
            return E_NodeStatus.Success;
        }else if (!_enter&&!m_Owner.CheckCanChangeStatus(E_CharacterFsmStatus.Attack, BeForce))
        {
            return E_NodeStatus.Failure;
        }
        return E_NodeStatus.Running;
    }

    public override void OnEnter()
    {
        _skillEnd = false;
        _enter = true;
        m_Movement.Attack(SkillId, BeForce,null, null, () =>
        {
            _skillEnd = true;
        });
    }

    public override void OnStay()
    {

    }

    public override void OnExit()
    {
    }
}
