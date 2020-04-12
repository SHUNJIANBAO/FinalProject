using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharAttackStatus : CharFsmBase
{
    float _timeCount;
    SkillConfig _skill;
    string _skillName;
    float _moveSpeed;

    bool _createCollider;
    bool _createBullet;

    public CharAttackStatus(Character owner, Animator animator) : base(owner, animator)
    {

    }

    public override bool CanEnter()
    {
        return true;
    }

    public override bool CanExit()
    {
        return _timeCount >= _skill.CanExitTime || m_CurStateInfo.normalizedTime >= 1f;
    }

    public override bool CanInterrupt()
    {
        return _timeCount >= _skill.ComboTime;
    }
    protected override void OnEnter(params object[] objs)
    {
        base.OnEnter(objs);
        _timeCount = 0;
        _createCollider = false;
        _skill = m_Owner.CurSkill;
        _skillName = AnimConfig.GetData(_skill.AnimId).Name;
        m_Animator.SetInteger("Index", _skill.AnimId);
        _moveSpeed = _skill.MoveDistance / _skill.MoveDuration;
        m_Owner.GetRangeAttribute(E_Attribute.Mp.ToString()).ChangeValue(-_skill.UseMp);
    }

    protected override void OnStay()
    {
        base.OnStay();
        if (m_CurStateInfo.IsName(_skillName))
        {
            _timeCount += GameManager.DeltaTime;

            CreateBullet();
            CreateCollider();
            CaculateInvincible();
            CaculateMove();

            if (m_CurStateInfo.normalizedTime >= 1f)
            {
                m_Owner.ChangeStatus(E_CharacterFsmStatus.Idle);
            }
        }
    }

    /// <summary>
    /// 计算无敌时间
    /// </summary>
    void CaculateInvincible()
    {
        if (_skill.InvincibleTime == 0 || _skill.InvincibleDuration == 0) return;
        if (_timeCount >= _skill.InvincibleTime && _timeCount < _skill.InvincibleTime + _skill.InvincibleDuration)
        {
            m_Owner.IsInvincible = true;
        }
        else
        {
            m_Owner.IsInvincible = false;
        }
    }

    /// <summary>
    /// 计算位移
    /// </summary>
    void CaculateMove()
    {
        if (_skill.MoveDistance == 0 || _skill.MoveDuration == 0) return;
        if (_timeCount >= _skill.MoveStartTime && _timeCount < _skill.MoveStartTime + _skill.MoveDuration)
        {
            if (m_Owner.transform.localScale.x > 0)
            {
                m_Owner.transform.Translate(Vector3.right * _moveSpeed * GameManager.DeltaTime);
            }
            else
            {
                m_Owner.transform.Translate(Vector3.left * _moveSpeed * GameManager.DeltaTime);
            }
        }
    }

    /// <summary>
    /// 造成伤害
    /// </summary>
    void CreateCollider()
    {
        if (!_createCollider && _skill.ColliderId != 0 && _timeCount >= _skill.DamageTime)
        {
            _createCollider = true;
            var collider = PoolManager.InstantiateGameObject(PathManager.ColliderPath, PoolType.Collider);
            var ctrl = collider.GetComponent<ColliderCtrl>();
            float damage = m_Owner.GetAttribute(E_Attribute.Atk.ToString()).GetTotalValue() * _skill.DamageRatio / 100;
            ctrl.Init(_skill.ColliderId, m_Owner, (int)damage, _skill.HitFlyForce);
        }
    }

    /// <summary>
    /// 生成弹幕
    /// </summary>
    void CreateBullet()
    {
        if (!_createBullet && _skill.BulletId != 0 && _timeCount >= _skill.DamageTime)
        {
            _createBullet = true;
            Debug.LogError("弹幕未完成");
        }
    }

    protected override void OnExit()
    {
        base.OnExit();
        _timeCount = 0;
    }
}
