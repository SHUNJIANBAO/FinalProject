using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectCtrl : MonoEntity
{
    AnimatorStateInfo _curState;
    ParticleSystem _particle;

    EffectConfig _cfg;
    float _lifeTime;
    float _timeCount;

    protected override void OnAwake()
    {
        base.OnAwake();
        m_Animator = GetComponent<Animator>();
        _particle = GetComponent<ParticleSystem>();
    }

    protected override void OnInit(params object[] objs)
    {
        base.OnInit(objs);
        _timeCount = 0;
        _cfg = (EffectConfig)objs[0];
        if (m_Animator != null)
        {
            m_Animator.Play("Play", 0, 0);
        }
        else if (_particle != null)
        {
            _particle.Play();
        }
        Debug.Log(_cfg.LifeTime);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (_cfg.LifeTime != 0)
        {
            _timeCount += Time.deltaTime;
            Debug.Log(_timeCount);
            if (_timeCount > _cfg.LifeTime)
            {
                Destroy();
            }
        }
        else
        {
            if (m_Animator != null)
            {
                _curState = m_Animator.GetCurrentAnimatorStateInfo(0);
                if (_curState.normalizedTime >= 1f)
                {
                    Destroy();
                }
            }
            else
            {
                Destroy();
            }
        }
    }

    void Destroy()
    {
        Debug.Log("Destroy");
        if (_particle != null)
        {
            _particle.Stop();
        }
        MonoBehaviourManager.Remove(this);
        PoolManager.DestroyGameObject(gameObject, PoolType.Effect);
    }
}

public struct EffectConfig
{
    public float LifeTime;
}
