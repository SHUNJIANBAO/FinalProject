using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterMovement : MonoBehaviour
{
    Character _character;

    Vector3 _right = Vector3.one;
    Vector3 _left = new Vector3(-1, 1, 1);

    private void Awake()
    {
        _character = GetComponent<Character>();
    }

    public void Attack(int skillId, bool beForce = false, System.Action damageCallback = null)
    {
        if (!_character.CheckCanChangeStatus(E_CharacterFsmStatus.Attack, beForce)) return;
        var skillCfg = SkillConfig.GetData(skillId);
        if (CheckCanUse(skillCfg))
        {
            _character.CurSkill = skillCfg;
            _character.ChangeStatus(E_CharacterFsmStatus.Attack, beForce, damageCallback);
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

        _character.ChangeStatus(E_CharacterFsmStatus.Jump, beForce, jumpForce, jumpDownCallback);
    }

    /// <summary>
    /// 播放动画
    /// </summary>
    /// <param name="animIndex"></param>
    /// <param name="beForce"></param>
    public void PlayAnim(int animIndex, bool beForce = false)
    {
        _character.ChangeStatus(E_CharacterFsmStatus.Play, beForce, animIndex);
    }

    /// <summary>
    /// 移动到指定点
    /// </summary>
    /// <param name="targetPos"></param>
    public void MoveToPoint(Vector3 targetPos, Action<Character> moveCallback = null)
    {
        _character.MoveTarget = targetPos;
        if (!_character.CheckCanChangeStatus(E_CharacterFsmStatus.Move)) return;

        LookToTarget(targetPos);

        if (_character.IsGround && _character.CurStatus != E_CharacterFsmStatus.Move)
        {
            _character.ChangeStatus(E_CharacterFsmStatus.Move,false, moveCallback);
        }
    }

    public void LookToTarget(Vector3 targetPos)
    {
        if (targetPos.x > _character.transform.position.x)
        {
            _character.transform.localScale = _right;
        }
        if (targetPos.x < _character.transform.position.x)
        {
            _character.transform.localScale = _left;
        }
    }

    public void MoveEnd()
    {
        _character.MoveTarget = _character.transform.position;
    }

    /// <summary>
    /// 重生
    /// </summary>
    protected virtual void ReBorn()
    {
        _character.ResetAttributes();
    }

    /// <summary>
    /// 受击
    /// </summary>
    public virtual void Hurt(GameObject atkOwner, int damage, int hitForce)
    {
        if (_character.IsInvincible) return;
        _character.GetRangeAttribute(E_Attribute.Hp.ToString()).ChangeValue(-damage);
        if (_character.CurStatus == E_CharacterFsmStatus.FallDown) return;
        LookToTarget(atkOwner);
        var shield = _character.GetRangeAttribute(E_Attribute.Shield.ToString());
        shield.ChangeValue(-hitForce);
        if (shield.Current <= shield.GetMinTotalValue())
        {
            shield.Reset();
            _character.ChangeStatus(E_CharacterFsmStatus.HitFly, true);
        }
        else
        {
            _character.ChangeStatus(E_CharacterFsmStatus.Hurt, true);
        }
    }

    void LookToTarget(GameObject target)
    {
        if (target.transform.position.x > transform.position.x)
        {
            transform.localScale = Vector3.one;
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
