using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterMovement : MonoBehaviour
{
    Character _character;

    //Vector3 _right = Vector3.one;
    //Vector3 _left = new Vector3(-1, 1, 1);

    private void Awake()
    {
        _character = GetComponent<Character>();
    }

    public void Attack(int skillId, bool beForce = false,System.Action enterCallback=null, System.Action damageCallback = null, System.Action endCallback = null)
    {
        if (!_character.CheckCanChangeStatus(E_CharacterFsmStatus.Attack, beForce)) return;
        var skillCfg = SkillConfig.GetData(skillId);
        if (CheckCanUse(skillCfg))
        {
            _character.CurSkill = skillCfg;
            enterCallback?.Invoke();
            _character.ChangeStatus(E_CharacterFsmStatus.Attack, beForce, endCallback, damageCallback);
        }
    }

    bool CheckCanUse(SkillConfig skillCfg)
    {
        var mpInstance = _character.GetRangeAttribute(E_Attribute.Mp.ToString());
        return mpInstance.Current >= skillCfg.UseMp;
    }

    public void Jump(float jumpForce, bool beForce, Action<Character> jumpDownCallback = null)
    {
        if (!_character.CheckCanChangeStatus(E_CharacterFsmStatus.Jump, beForce)) return;

        _character.ChangeStatus(E_CharacterFsmStatus.Jump, beForce, null, jumpForce, jumpDownCallback);
    }

    /// <summary>
    /// 播放动画
    /// </summary>
    /// <param name="animIndex"></param>
    /// <param name="beForce"></param>
    public void PlayAnim(int animIndex, bool beForce = false)
    {
        _character.ChangeStatus(E_CharacterFsmStatus.Play, beForce, null, animIndex);
    }

    /// <summary>
    /// 移动到指定点
    /// </summary>
    /// <param name="targetPos"></param>
    public void MoveToPoint(Vector3 targetPos, Action<Character> moveCallback = null)
    {
        _character.MoveTarget = targetPos;
        if (!_character.CheckCanChangeStatus(E_CharacterFsmStatus.Move)) return;

        _character.LookToTarget(targetPos);

        if (_character.IsGround && _character.CurStatus != E_CharacterFsmStatus.Move)
        {
            _character.ChangeStatus(E_CharacterFsmStatus.Move, false, null, moveCallback);
        }
    }


    public void MoveEnd()
    {
        _character.MoveTarget = _character.transform.position;
        if (_character.IsGround)
            _character.ChangeStatus(E_CharacterFsmStatus.Idle);
    }

    /// <summary>
    /// 重生
    /// </summary>
    protected virtual void ReBorn()
    {
        _character.ResetAttributes();
    }

}
