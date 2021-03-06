﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharAttackStatus : CharFsmBase
{
    float _timeCount;
    SkillConfig _skill;
    string _skillName;
    float _moveSpeed;

    //Vector3 _blinkTarget = Vector3.zero;
    //Vector3 _tempVector = Vector3.zero;

    Action _damageCallback;
    int _damageIndex;
    int _blinkIndex;
    bool _effectPlayed;
    bool _audioPlayed;

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
        return true;
    }

    public bool CanCombo()
    {
        return _timeCount >= _skill.ComboTime;
    }

    protected override void OnInterrupt()
    {
        base.OnInterrupt();
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
        _blinkIndex = 0;
        _effectPlayed = false;
        _audioPlayed = false;
        _skill = m_Owner.CurSkill;
        _skillName = AnimConfig.GetData(_skill.AnimId).Name;
        m_Animator.SetInteger("Index", _skill.AnimId);
        m_Owner.GetRangeAttribute(E_Attribute.Mp.ToString()).ChangeValue(-_skill.UseMp);

        if (m_Owner.AttackTarget != null)
        {
            m_Owner.LookToTarget(m_Owner.AttackTarget.transform.position);
        }
        //_moveSpeed = _skill.MoveDistance / _skill.MoveDuration;
        //if (m_Owner.IsFaceRight)
        //{
        //    _blinkTarget.x = _skill.MoveDistance;
        //}
        //else
        //{
        //    _blinkTarget.x = -_skill.MoveDistance;
        //}
    }

    protected override void OnStay()
    {
        base.OnStay();
        if (m_CurStateInfo.IsName(_skillName))
        {
            _timeCount += m_Animator.speed * Time.deltaTime;

            CaculateDamageTime();
            CaculateInvincible();
            CaculateMove();
            CaculatePlayAudio();
            CaculatePlayEffect();

            if (m_Owner.IsGround && m_CurStateInfo.normalizedTime >= 0.9f)
            {
                m_Owner.ChangeStatus(E_CharacterFsmStatus.Idle);
            }
            else if (!m_Owner.IsGround && m_CurStateInfo.normalizedTime >= 0.9f)
            {
                m_Owner.ChangeStatus(E_CharacterFsmStatus.Jump, true);
            }

        }
    }

    /// <summary>
    /// 计算无敌时间
    /// </summary>
    void CaculateInvincible()
    {
        if ( _skill.InvincibleDuration == 0) return;
        if (_timeCount >= _skill.InvincibleTime && _timeCount < _skill.InvincibleTime + _skill.InvincibleDuration)
        {
            m_Owner.IsInvincible = true;
            Physics2D.IgnoreLayerCollision(GameConfig.Instance.PlayerLayer, GameConfig.Instance.EnemyLayer, true);
        }
        else
        {
            Physics2D.IgnoreLayerCollision(GameConfig.Instance.PlayerLayer, GameConfig.Instance.EnemyLayer, false);
            m_Owner.IsInvincible = false;
        }
    }

    /// <summary>
    /// 计算播放音效时间点
    /// </summary>
    void CaculatePlayAudio()
    {
        if (string.IsNullOrEmpty(_skill.PlayAudioName)||_audioPlayed)
        {
            return;
        }
        if (_timeCount>=_skill.PlayAudioTime)
        {
            _audioPlayed = true;
            AudioManager.Instance.PlayAudio(_skill.PlayAudioName); //, m_Owner.gameObject);
        }
    }

    /// <summary>
    /// 计算播放特效时间点
    /// </summary>
    void CaculatePlayEffect()
    {
        if (string.IsNullOrEmpty(_skill.PlayEffectName)||_effectPlayed)
        {
            return;
        }
        if (_timeCount >= _skill.PlayEffectTime)
        {
            _effectPlayed = true;
            if (_skill.EffectIsChild)
            {
                EffectManager.Instance.Play(_skill.PlayEffectName, _skill.EffectOffest, m_Owner.transform, _skill.EffectLife);
            }
            else
            {
                Vector3 offest = m_Owner.IsFaceRight ? _skill.EffectOffest : new Vector3(-_skill.EffectOffest.x, _skill.EffectOffest.y, _skill.EffectOffest.z);
                Vector3 pos = m_Owner.transform.position + offest;
                EffectManager.Instance.Play(_skill.PlayEffectName, pos, m_Owner.IsFaceRight, _skill.EffectLife);
            }
        }
    }

    /// <summary>
    /// 计算位移
    /// </summary>
    void CaculateMove()
    {
        for (int i = _blinkIndex; i < _skill.MoveStartTime.Count; i++)
        {
            if (_timeCount >= _skill.MoveStartTime[i])
            {
                if (_skill.MoveDistance[i] == 0 || _skill.MoveDuration[i] == 0) return;
                _moveSpeed = _skill.MoveDistance[i] / _skill.MoveDuration[i];
                if (m_CurStateInfo.normalizedTime < 0.7f)
                {
                    m_Owner.Rigibody.velocity = Vector2.zero;
                }
                else
                {
                    _moveSpeed -= _moveSpeed * (1 - m_CurStateInfo.normalizedTime);
                }
                if (m_Owner.transform.localScale.x > 0)
                {
                    m_Owner.transform.Translate(Vector3.right * _moveSpeed * m_Animator.speed * Time.deltaTime);
                }
                else
                {
                    m_Owner.transform.Translate(Vector3.left * _moveSpeed * m_Animator.speed * Time.deltaTime);
                }
                if (_timeCount > _skill.MoveStartTime[i] + _skill.MoveDuration[i])
                {
                    _blinkIndex++;
                    i = _blinkIndex;
                }
            }
        }
        //if (_skill.MoveDistance == 0 || _skill.MoveDuration == 0) return;
        //if (_timeCount >= _skill.MoveStartTime && _timeCount < _skill.MoveStartTime + _skill.MoveDuration)
        //{
        //    m_Owner.Rigibody.velocity = Vector2.zero;
        //    if (m_Owner.transform.localScale.x > 0)
        //    {
        //        m_Owner.transform.Translate(Vector3.right * _moveSpeed * m_Animator.speed * Time.deltaTime);
        //    }
        //    else
        //    {
        //        m_Owner.transform.Translate(Vector3.left * _moveSpeed * m_Animator.speed * Time.deltaTime);
        //    }
        //}
    }

    /// <summary>
    /// 计算伤害时间点
    /// </summary>
    void CaculateDamageTime()
    {
        for (int i = _damageIndex; i < _skill.DamageTime.Count; i++)
        {
            if (_timeCount >= _skill.DamageTime[i])
            {
                if (_skill.ColliderId.Count > 0)
                {
                    CreateCollider(i);
                }
                if (_skill.BarrageId.Count > 0 && _skill.BulletId != 0)
                {
                    CreateBarrage(i);
                }
                _damageIndex++;
                i = _damageIndex;
            }
        }
    }

    /// <summary>
    /// 创建碰撞器
    /// </summary>
    void CreateCollider(int index)
    {
        _damageCallback?.Invoke();
        if (_skill.ColliderId.Count == 0 || _skill.ColliderId.Count <= index || _skill.ColliderId[index] == 0)
        {
            return;
        }
        float damageRatio = _skill.DamageRatio.Count > index ? _skill.DamageRatio[index] : _skill.DamageRatio[0];
        float damage = m_Owner.GetAttribute(E_Attribute.Atk.ToString()).GetTotalValue() * damageRatio / 100;

        var collider = PoolManager.InstantiateGameObject(PathManager.ColliderPath, PoolType.Collider);
        var ctrl = collider.GetComponent<ColliderCtrl>();
        int colliderId = _skill.ColliderId.Count > index ? _skill.ColliderId[index] : _skill.ColliderId[0];
        int hitFlyForce = _skill.HitFlyForce.Count > index ? _skill.HitFlyForce[index] : _skill.HitFlyForce[0];
        ctrl.Init(colliderId, m_Owner, (int)damage, hitFlyForce, _skill);
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
        if (m_Owner == GameManager.Player)
        {
            ShootManager.Instance.Shoot(parent, m_Owner.gameObject, bullet, damage, barrageCfg, GameConfig.Instance.EnemyLayer);
        }
        else
        {
            ShootManager.Instance.Shoot(parent, m_Owner.gameObject, bullet, damage, barrageCfg, GameConfig.Instance.PlayerLayer);
        }
    }

    protected override void OnExit()
    {
        base.OnExit();
        _timeCount = 0;
        _damageCallback = null;
        Physics2D.IgnoreLayerCollision(GameConfig.Instance.PlayerLayer, GameConfig.Instance.EnemyLayer, false);
        m_Owner.IsInvincible = false;
    }
}
