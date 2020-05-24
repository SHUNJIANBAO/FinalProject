using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectCtrl : MonoEntity 
{
    AnimatorStateInfo _curState;

    EffectConfig _cfg;
    float _lifeTime;
    float _timeCount;

    protected override void OnAwake()
    {
        base.OnAwake();
        m_Animator = GetComponent<Animator>();
    }

    protected override void OnInit(params object[] objs)
    {
        base.OnInit(objs);
        _timeCount = 0;
        _cfg = (EffectConfig)objs[0];
        m_Animator.Play("Play", 0, 0);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (_cfg.LifeTime!=0)
        {
            _timeCount += Time.deltaTime* m_Animator.speed * Time.deltaTime;
            if (_timeCount>_cfg.LifeTime)
            {
                Destroy();
            }
        }
        else
        {
            _curState = m_Animator.GetCurrentAnimatorStateInfo(0);
            if (_curState.normalizedTime >= 1f)
            {
                Destroy();
            }
        }
    }

    void Destroy()
    {
        MonoBehaviourManager.Remove(this);
        PoolManager.DestroyGameObject(gameObject, PoolType.Effect);
    }
}

public struct EffectConfig
{
    public float LifeTime;
}
