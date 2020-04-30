using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharAttackStatus : CharFsmBase
{
    float _timeCount;
    SkillConfig _skill;
    string _skillName;
    float _moveSpeed;

    Vector3 _blinkTarget = Vector3.zero;
    //Vector3 _tempVector = Vector3.zero;

    Action _damageCallback;
    int _damageIndex;

    public CharAttackStatus(Character owner, Animator animator) : base(owner, animator)
    {

    }

    public override bool CanEnter()
    {
        return true;
    }

    public override bool CanExit()
    {
        return _timeCount >= _skill.CanExitTime;//|| (m_CurStateInfo.IsName(_skillName) && m_CurStateInfo.normalizedTime >= 1f);
    }

    public override bool CanInterrupt()
    {
        return _timeCount >= _skill.ComboTime;
    }
    protected override void OnEnter(params object[] objs)
    {
        base.OnEnter(objs);
        if (objs != null && objs.Length > 0)
            _damageCallback = (Action)objs[0];
        else
            _damageCallback = null;
        _timeCount = 0;
        _damageIndex = 0;
        _skill = m_Owner.CurSkill;
        _skillName = AnimConfig.GetData(_skill.AnimId).Name;
        m_Animator.SetInteger("Index", _skill.AnimId);
        _moveSpeed = _skill.MoveDistance / _skill.MoveDuration;
        m_Owner.GetRangeAttribute(E_Attribute.Mp.ToString()).ChangeValue(-_skill.UseMp);
        if (m_Owner.IsFaceRight)
        {
            _blinkTarget.x = _skill.MoveDistance;
        }
        else
        {
            _blinkTarget.x = -_skill.MoveDistance;
        }
    }

    protected override void OnStay()
    {
        base.OnStay();
        if (m_CurStateInfo.IsName(_skillName))
        {
            _timeCount += GameManager.DeltaTime;

            CaculateDamageTime();
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
            m_Owner.Rigibody.velocity = Vector2.zero;
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
    /// 计算伤害时间点
    /// </summary>
    void CaculateDamageTime()
    {
        for (int i = _damageIndex; i < _skill.DamageTime.Count;)
        {
            if (_timeCount >= _skill.DamageTime[i])
            {
                _damageIndex++;
                i = _damageIndex;
                if (_skill.ColliderId.Count > 0)
                {
                    CreateCollider(i);
                }
                if (_skill.BarrageId.Count > 0 && _skill.BulletId != 0)
                {
                    CreateBarrage(i);
                }
            }
        }
    }

    /// <summary>
    /// 创建碰撞器
    /// </summary>
    void CreateCollider(int index)
    {
        float damageRatio = _skill.DamageRatio.Count > index ? _skill.DamageRatio[index] : _skill.DamageRatio[0];
        float damage = m_Owner.GetAttribute(E_Attribute.Atk.ToString()).GetTotalValue() * damageRatio / 100;

        var collider = PoolManager.InstantiateGameObject(PathManager.ColliderPath, PoolType.Collider);
        var ctrl = collider.GetComponent<ColliderCtrl>();
        int colliderId = _skill.ColliderId.Count > index ? _skill.ColliderId[index] : _skill.ColliderId[0];
        ctrl.Init(colliderId, m_Owner, (int)damage, _skill.HitFlyForce, _skill.HitEffect, _skill.HitEffectPosType);
        _damageCallback?.Invoke();
    }

    /// <summary>
    /// 创建弹幕
    /// </summary>
    void CreateBarrage(int index)
    {
        float damageRatio = _skill.DamageRatio.Count > index ? _skill.DamageRatio[index] : _skill.DamageRatio[0];
        float damage = m_Owner.GetAttribute(E_Attribute.Atk.ToString()).GetTotalValue() * damageRatio / 100;

        int barrageId = _skill.BarrageId.Count > index ? _skill.BarrageId[index] : _skill.BarrageId[0];
        var barrageCfg = BarrageConfig.GetData(barrageId);
        var bullet = ResourceManager.Load<GameObject>(PathManager.GetBulletPath("Bullet_" + _skill.BulletId));
        Transform parent = null;
        if (barrageCfg.IsFollow)
        {
            parent = m_Owner.transform;
        }
        ShootManager.Instance.Shoot(parent, m_Owner.transform.position, m_Owner.transform.right, bullet, damage, barrageCfg);
    }

    protected override void OnExit()
    {
        base.OnExit();
        _timeCount = 0;
        _damageCallback = null;
    }
}
