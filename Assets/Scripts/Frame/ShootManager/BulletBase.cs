using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletBase : MonoEntity
{
    public E_BulletFsmStatus CurStatus;
    protected FsmManager m_Fsm;
    protected int _damage;
    protected int _targetLayer;

    protected BulletConfig m_BulletCfg;

    float _timeCount;
    protected override void OnInit(params object[] objs)
    {
        base.OnInit(objs);
        m_Animator = GetComponent<Animator>();
        _damage = int.Parse(objs[0].ToString());
        _targetLayer = int.Parse(objs[1].ToString());
        m_BulletCfg = BulletConfig.GetData(Id);

        _timeCount = 0;

        RegistFsm();
    }

    void RegistFsm()
    {
        m_Fsm = new FsmManager();
        BulletStartStatus start = new BulletStartStatus(this, m_Animator);
        m_Fsm.AddStatus((int)E_BulletFsmStatus.Start, start);
        BulletRunningStatus run = new BulletRunningStatus(this, m_Animator);
        m_Fsm.AddStatus((int)E_BulletFsmStatus.Running, run);
        BulletEndStatus end = new BulletEndStatus(this, m_Animator);
        m_Fsm.AddStatus((int)E_BulletFsmStatus.End, end);

        ChangeStatus(E_BulletFsmStatus.Start);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        m_Fsm.OnStay();

        _timeCount += GameManager.DeltaTime;
        if (_timeCount >= m_BulletCfg.Life && CurStatus == E_BulletFsmStatus.Running)
        {
            _timeCount = 0;
            MonoBehaviourManager.Remove(this);
            PoolManager.DestroyGameObject(gameObject, PoolType.Bullet);
        }
    }

    public abstract void Move();

    /// <summary>
    /// 检测是否可以切换到指定状态
    /// </summary>
    /// <param name="targetStatus"></param>
    /// <param name="beForce"></param>
    /// <returns></returns>
    public bool CheckCanChangeStatus(E_BulletFsmStatus targetStatus, bool beForce = false)
    {
        return m_Fsm.CheckCanChangeStatus((int)targetStatus, beForce);
    }

    /// <summary>
    /// 切换状态
    /// </summary>
    /// <param name="status">状态类型</param>
    /// <param name="beForce">是否强制切换</param>
    /// <returns>切换是否成功</returns>
    public bool ChangeStatus(E_BulletFsmStatus status, bool beForce = false, System.Action onCompelte = null, params object[] objs)
    {
        bool result = m_Fsm.ChangeStatus((int)status, beForce, objs);
        if (result)
        {
            CurStatus = status;
            var fsmStatus = m_Fsm.GetStatus((int)status);
            fsmStatus.OnComplete = onCompelte;
        }
        return result;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Character target = collision.GetComponent<Character>();
        if (target != null && target.gameObject.layer == _targetLayer)
        {
            Damage(target);
        }
        switch (m_BulletCfg.Type)
        {
            case E_BulletType.Collide:
                if (target != null && target.gameObject.layer == _targetLayer)
                {
                    ChangeStatus(E_BulletFsmStatus.End, true);
                    if (!string.IsNullOrEmpty(m_BulletCfg.DestroyEffect))
                    {
                        EffectManager.Instance.Play(m_BulletCfg.DestroyEffect, transform.position, true);
                    }
                }
                break;
            case E_BulletType.Trigger:
                break;
            case E_BulletType.CollidePlane:
                if (collision.gameObject.layer == GameConfig.Instance.PlaneLayer)
                {
                    ChangeStatus(E_BulletFsmStatus.End, true);
                    if (!string.IsNullOrEmpty(m_BulletCfg.DestroyEffect))
                    {
                        EffectManager.Instance.Play(m_BulletCfg.DestroyEffect, transform.position, true);
                    }
                }
                break;
        }
    }

    void Damage(Character target)
    {
        target.Hurt(gameObject, _damage, m_BulletCfg.HitForce);

        if (!string.IsNullOrEmpty(m_BulletCfg.DamageEffect))
        {
            Vector2 dir = target.transform.position - transform.position;
            var hit = Physics2D.Raycast(transform.position, dir, 5);
            if (hit.transform.GetComponent<Character>() == target)
            {
                EffectManager.Instance.Play(m_BulletCfg.DamageEffect, hit.point, true);
            }
        }
    }

}
